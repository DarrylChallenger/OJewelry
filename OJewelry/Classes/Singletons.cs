using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Classes
{
    public class Singletons
    {
        public static AzureBlobStorageContainer azureBlobStorage = new AzureBlobStorageContainer();
    }
}