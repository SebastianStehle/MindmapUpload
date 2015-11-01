// ==========================================================================
// AzureBlobFileStorage.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.IO;
using System.Threading.Tasks;
using GP.Utils;
using Microsoft.Framework.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SE.Upload.Web.Contracts;

namespace SE.Upload.Web.Implementations
{
    public sealed class AzureBlobFileStorage : IFileStorage
    {
        private readonly CloudBlobContainer container;
        private const int ValidHours = 24;

        public AzureBlobFileStorage(IConfiguration configuration)
        {
            Guard.NotNull(configuration, nameof(configuration));

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration["storage:azure:connectionString"]);

            CloudBlobClient client = storageAccount.CreateCloudBlobClient();

            container = client.GetContainerReference(configuration["storage:azure:container"]);
        }
        
        public async Task<string> UploadAsync(FileUpload file)
        {
            Guard.NotNull(file, nameof(file));

            file.Uploaded = DateTime.UtcNow;

            file.Id = file.Id ?? IdGenerator.GenerateId();

            CloudBlockBlob blob = container.GetBlockBlobReference($"{file.Id}.json");

            string json = JsonConvert.SerializeObject(file);
            
            blob.Properties.ContentType = "application.json";

            await blob.UploadTextAsync(json);
            await blob.SetPropertiesAsync();

            return file.Id;
        }

        public async Task<FileUpload> GetFileAsync(string id)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference($"{id}.json");

            if (!await blob.ExistsAsync())
            {
                throw new FileNotFoundException();
            }

            string json = await blob.DownloadTextAsync();

            FileUpload file = JsonConvert.DeserializeObject<FileUpload>(json);

            if (file.Uploaded.AddHours(ValidHours) < DateTime.UtcNow)
            {
                throw new FileExpiredException();
            }

            return file;
        }
    }
}
