using System;
using System.Configuration;
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

        public async Task<string> Upload(HttpPostedFileBase fb)
        {
            // check bInitialized
            string filename = Path.GetFileNameWithoutExtension(fb.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(fb.FileName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.Properties.ContentType = fb.ContentType;
            await blockBlob.UploadFromStreamAsync(fb.InputStream);
            // Make sure file is accessible as URL
            return blockBlob.StorageUri.PrimaryUri.ToString();
        }
        /*
        public void Download(string fn)
        {
            // check bInitialized
            using (MemoryStream memstream = new MemoryStream())
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fn);
                blockBlob.DownloadToStreamAsync();
            }
        }
        */
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