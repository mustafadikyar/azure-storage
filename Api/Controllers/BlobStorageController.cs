using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Enums;

namespace Api.Controllers
{
    [Route("api/blob-storage")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorage;
        public BlobStorageController(IBlobStorageService blobStorage) => _blobStorage = blobStorage;

        [HttpGet("names")]
        public async Task<IActionResult> GetNames()
        {
            var names = _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
            var urls = names.Select(c => new { Name = c, Url = $"{blobUrl}/{c}" }).ToList();

            var log = await _blobStorage.GetLog($"log-{DateTime.Now.ToShortDateString()}.txt");

            return Ok(new { urls = urls, log = log });
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(IFormFile file)
        {
            await _blobStorage.SetLog("Start.", $"log-{DateTime.Now.ToShortDateString()}.txt");

            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            await _blobStorage.Upload(file.OpenReadStream(), newFileName, EContainerName.pictures);

            await _blobStorage.SetLog("End.", $"log-{DateTime.Now.ToShortDateString()}.txt");

            return Ok(newFileName);
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string fileName)
        {
            var stream = await _blobStorage.Download(fileName, EContainerName.pictures);
            return File(stream, "application/octet-stream", fileName);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _blobStorage.Delete(fileName, EContainerName.pictures);
            return NoContent();
        }
    }
}
