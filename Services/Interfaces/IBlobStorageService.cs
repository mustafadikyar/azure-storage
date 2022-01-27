using Services.Models.Enums;

namespace Services.Interfaces
{
    public interface IBlobStorageService
    {
        public string BlobUrl { get; }
        
        List<string> GetNames(EContainerName eContainerName);

        Task Upload(Stream fileStream, string fileName, EContainerName eContainerName);
        Task<Stream> Download(string fileName, EContainerName eContainerName);
        Task Delete(string fileName, EContainerName eContainerName);
        
        Task SetLog(string text, string fileName);
        Task<List<string>> GetLog(string fileName);        
    }
}
