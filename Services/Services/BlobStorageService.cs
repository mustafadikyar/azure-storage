using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Services.Config;
using Services.Interfaces;
using Services.Models.Enums;

namespace Services.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public string BlobUrl => "your-blob-url";

        private readonly BlobServiceClient _client;
        public BlobStorageService() => _client = new BlobServiceClient(AppSettings.AzureStorageConnectionString);

        public async Task Delete(string fileName, EContainerName eContainerName)
        {
            var container = _client.GetBlobContainerClient(eContainerName.ToString());
            var client = container.GetBlobClient(fileName);
            await client.DeleteAsync();
        }

        public async Task<Stream> Download(string fileName, EContainerName eContainerName)
        {
            var container = _client.GetBlobContainerClient(eContainerName.ToString());
            var client = container.GetBlobClient(fileName);
            var info = await client.DownloadAsync();
            return info.Value.Content;
        }

        public async Task<List<string>> GetLog(string fileName)
        {
            List<string> logs = new List<string>();
            var container = _client.GetBlobContainerClient(EContainerName.logs.ToString());
            await container.CreateIfNotExistsAsync();

            var client = container.GetAppendBlobClient(fileName);
            await client.CreateIfNotExistsAsync();

            var info = await client.DownloadAsync();

            using (StreamReader sr = new StreamReader(info.Value.Content))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    logs.Add(line);
                }
            }
            return logs;
        }

        public List<string> GetNames(EContainerName eContainerName)
        {
            List<string> blobNames = new List<string>();
            var container = _client.GetBlobContainerClient(eContainerName.ToString());
            var blobs = container.GetBlobs();
            blobs.ToList().ForEach(x =>
            {
                blobNames.Add(x.Name);
            });

            return blobNames;
        }

        public async Task SetLog(string text, string fileName)
        {
            var container = _client.GetBlobContainerClient(EContainerName.logs.ToString());

            var client = container.GetAppendBlobClient(fileName);
            await client.CreateIfNotExistsAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.Write($"{DateTime.Now}: {text}\n");
                    sw.Flush();
                    ms.Position = 0;
                    await client.AppendBlockAsync(ms);
                }
            }
        }

        public async Task Upload(Stream fileStream, string fileName, EContainerName eContainerName)
        {
            var container = _client.GetBlobContainerClient(eContainerName.ToString());
            await container.CreateIfNotExistsAsync();
            
            await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            
            var blobClient = container.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, true);
        }
    }
}
