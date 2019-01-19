using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace OJewelry.Classes 
{
    public class AzureBlobStorageContainer
    {
        private CloudStorageAccount storageAccount { get; set; }
        private CloudBlobClient client { get; set; }
        private CloudBlobContainer container { get; set; }

        public void Init(string connStr, string dirName) 
        {
            // Get Storage Acct
            CreateStorageAccountFromConnectionString(connStr);
            // Get Blob Client
            client = storageAccount.CreateCloudBlobClient();
            container = client.GetContainerReference(dirName);
            BlobRequestOptions requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
            container.CreateIfNotExists(requestOptions, null);
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        public async Task<string> Upload(HttpPostedFileBase fb, string fn)
        {
            // check bInitialized
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fn);
            blockBlob.Properties.ContentType = fb.ContentType;
            await blockBlob.UploadFromStreamAsync(fb.InputStream);
            // Make sure file is accessible as URL
            return blockBlob.StorageUri.PrimaryUri.ToString();
        }

        
        public async Task<Stream> Download(string uri)
        {
            // check bInitialized
            CloudBlockBlob blockBlob = new CloudBlockBlob(new Uri(uri));
            MemoryStream m = new MemoryStream();
            await blockBlob.DownloadToStreamAsync(m);           
            return m;
        }
        /*
        public void Move(string fnSource, string fnTarget) { }
        */

        public async Task<string> Copy(string uriSource, string fnTarget)
        {
            CloudBlockBlob sourceBlob = container.GetBlockBlobReference(uriSource);
            CloudBlockBlob targetBlob = container.GetBlockBlobReference(fnTarget);
            using (MemoryStream sourceStream = new MemoryStream())
            {
                await sourceBlob.DownloadToStreamAsync(sourceStream);         
                targetBlob.Properties.ContentType = sourceBlob.Properties.ContentType;
                sourceStream.Seek(0, SeekOrigin.Begin);
                await targetBlob.UploadFromStreamAsync(sourceStream);             
            }
            return targetBlob.StorageUri.PrimaryUri.ToString();
        }

        public void Delete(string uriSource)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference(uriSource);
            blob.Delete();
        }


        private void CreateStorageAccountFromConnectionString(string connStr)
        {
            try
            {
                storageAccount = CloudStorageAccount.Parse(connStr);
            }
            catch (FormatException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

    }
}