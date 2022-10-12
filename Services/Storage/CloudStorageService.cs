using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services.Storage
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        public CloudStorageService(string connectionstring)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
        }

        public async Task SendMessageToQueue(string queueName, string message)
        {
            CloudQueueClient cloudQueueClient = _cloudStorageAccount.CreateCloudQueueClient();
            CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(queueName);
            await cloudQueue.CreateIfNotExistsAsync();
            CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(message);
            await cloudQueue.AddMessageAsync(cloudQueueMessage);

        }

        public async Task UploadFileToBlob(IFormFileCollection formfiles, Guid folderId, string containerName)
        {
            foreach (IFormFile file in formfiles)
            {
                Stream stream = file.OpenReadStream();
                CloudBlobClient blobClient = _cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(containerName);
                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new
                        BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                }
                await cloudBlobContainer.CreateIfNotExistsAsync();
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer
                    .GetBlockBlobReference(Path.Combine(folderId.ToString(), file.FileName));
                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }
        }
    }
}
