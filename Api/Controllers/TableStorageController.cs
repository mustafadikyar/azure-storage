using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models;

namespace Api.Controllers
{
    [Route("api/table-storage")]
    [ApiController]
    public class TableStorageController : ControllerBase
    {
        private readonly INoSqlStorage<Product> _noSqlStorage;
        public TableStorageController(INoSqlStorage<Product> noSqlStorage) => _noSqlStorage = noSqlStorage;

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _noSqlStorage.GetAll();
            return Ok(response.ToList());
        }

        [HttpGet("single")]
        public async Task<IActionResult> GetSingleAsync(string rowKey, string partitionKey)
        {
            var response = await _noSqlStorage.GetSingle(rowKey, partitionKey);
            return Ok(response);
        }

        [HttpGet("{query}")]
        public IActionResult Query(string query)
        {
            var response = _noSqlStorage.Query(c => c.Name.Contains(query));
            return Ok(response.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product model)
        {
            await _noSqlStorage.Add(model);
            return Created("", null);
        }

        [HttpPut]
        public async Task<IActionResult> Put(string rowKey, string partitionKey)
        {
            var entity = await _noSqlStorage.GetSingle(rowKey, partitionKey);
            await _noSqlStorage.Update(entity);
            return Ok(entity);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string rowKey, string partitionKey)
        {
            await _noSqlStorage.Delete(rowKey, partitionKey);
            return NoContent();
        }
    }
}
