using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderMgmt.Services.Storage
{
   public interface ICloudStorageService
    {
        Task UploadFileToBlob(IFormFileCollection formfiles, Guid folderId, string containerName);
        Task SendMessageToQueue(string queueName, string message);
    }
}
