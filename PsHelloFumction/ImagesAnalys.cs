using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PsHelloFumction
{
    public static class ImagesAnalys
    {
        [FunctionName("ImagesAnalys")]
        public static void Run( [BlobTrigger("images/{name}", Connection = "ImgStorage")]
                                CloudBlockBlob blob, string name, 
                                TraceWriter log )
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{blob.Name} \n Size: {blob.Properties.Length} Bytes");
            var sas = GetSas(blob);
            var url = blob.Uri + sas;
            log.Info($"blob url is  { url }");

        }
        // generate sas signature for private acces of blob
        public static string GetSas(CloudBlockBlob blob)
        {

            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions=SharedAccessBlobPermissions.Read,
                SharedAccessStartTime= DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };
            var sas = blob.GetSharedAccessSignature(sasPolicy);
            return sas;
        }
    }
}
