using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataSample
{
    class Program
    {
        static string accountName = "sarm";
        static string accountKey = "yIUy29gGSIC2mECSZDLUBlR1Fie9FPiw+rPP4Ltb6qBFX9G9AIhBA96mez/pqCXZG+T8oWNrmjTa07QiJ64iyw==";

        static void createMetadataBlob(CloudBlobContainer container, string filename)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);
            //agregamos metadatos
            blob.Metadata.Add("Autor", "PinkFloyd");
            blob.Metadata.Add("Imagen", "HD");
            blob.Metadata.Add("Subidopor", "LeonardoC");
            blob.Metadata.Add("Comentario", "Mi wallpaper");
            using (Stream file = System.IO.File.OpenRead(@"C:\Users\slave\Pictures\pinkwall.jpg"))
            {
                blob.UploadFromStream(file);
            }
        }

        static void getMetaData(CloudBlobContainer container, string filename)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);
            blob.FetchAttributes();
            foreach (var atr in blob.Metadata)
            {
                Console.WriteLine(atr.Key + " " + atr.Value);
            }
        }


        static void Main(string[] args)
        {
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer sampleContainer = client.GetContainerReference("contenedor");

                sampleContainer.CreateIfNotExists();
                //crea blob y persiste en Azure Storage
                //createMetadataBlob(sampleContainer, "pinkfloyd.jpg");
                getMetaData(sampleContainer, "pinkfloyd.jpg");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Finalizado, presione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}
