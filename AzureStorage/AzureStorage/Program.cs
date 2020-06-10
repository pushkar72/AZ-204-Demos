using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureStorage {
    class Program {
        static async Task Main (string[] args) {
          //azure storage connection string
          var connectionString="DefaultEndpointsProtocol=https;AccountName=pushkarstg;AccountKey=de0zwvdQzcW21jLscSUWCZOfDhG4Ou5zKKAb0k8vKMCNkGG0dkPpZKgNZ6Pj3VLK1H1ybfibO+niz2hMmhPZ/g==;EndpointSuffix=core.windows.net";
          var p=new Program();
            // p.CreateContainer(connectionString);
        
        var blobServiceClient = new BlobServiceClient (connectionString);
        var containerRef= blobServiceClient.GetBlobContainerClient("documents");
        await p.DeleteContainer(containerRef);
        

        }

        public async Task CreateContainer (string connectionString) {

            var blobServiceClient = new BlobServiceClient (connectionString);

            string containerName = "documents";

            var containerClient = await blobServiceClient.CreateBlobContainerAsync (containerName);

            Console.WriteLine("Container Created");
        }

        public async void Upload (BlobContainerClient containerClient) {
            // Create a local file in the ./data/ directory for uploading and downloading
            string localPath = "./data/";
            string fileName = "flower.jpg";
            string localFilePath = Path.Combine (localPath, fileName);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient (fileName);

            Console.WriteLine ("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Open the file and upload its data
            using FileStream uploadFileStream = File.OpenRead (localFilePath);
            await blobClient.UploadAsync (uploadFileStream, true);
            uploadFileStream.Close ();
            Console.WriteLine("File Uploaded");
        }

        public async void Download (BlobContainerClient containerClient) {
        
            string downloadFilePath = @".\downloads\download.jpg";

            Console.WriteLine ("\nDownloading blob to\n\t{0}\n", downloadFilePath);
            BlobClient blobClient = containerClient.GetBlobClient ("flower.jpg");
            
            BlobDownloadInfo download = await blobClient.DownloadAsync ();

            using (FileStream downloadFileStream = File.OpenWrite (downloadFilePath)) {
                await download.Content.CopyToAsync (downloadFileStream);
                downloadFileStream.Close ();
            }
        }

        public async Task DeleteContainer (BlobContainerClient containerClient) {

            Console.Write ("Press any key to begin clean up");
            Console.ReadLine ();

            Console.WriteLine ("Deleting blob container...");
            await containerClient.DeleteAsync ();

            Console.WriteLine ("Done");
        }
    }
}