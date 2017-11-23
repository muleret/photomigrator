using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: PhotoMigrator.exe <folder>");
                Console.ReadKey();
                return;
            }

            string dir = args[0];

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();

            //var cont2 = blobClient.GetContainerReference("images2");
            //cont2.CreateIfNotExists();

            var container = blobClient.GetContainerReference("images1");

            string[] files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                // Create or overwrite the "myblob" blob with contents from a local file.
                using (var fileStream = System.IO.File.OpenRead(file))
                {
                    blockBlob.UploadFromStream(fileStream);
                    Console.WriteLine("Finished uploading " + fileName);
                }
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
