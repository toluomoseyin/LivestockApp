using System;
using System.IO;

using AnimalFarmsMarket.Commons;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/[controller]")]
    public class LogsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLogs()
        {
            var assemblyFullPath = Path.GetFullPath(@"../AnimalFarmsMarket.Core/bin/Debug/netcoreapp3.1/Logs");
            //var dir = Environment;
            string[] filePaths = Directory.GetFiles(assemblyFullPath);
            return Ok(filePaths);
        }

        [HttpGet("{fileId}")]
        public IActionResult GetLogContent(string fileId)
        {
            var assemblyFullPath = Path.GetFullPath(@"../AnimalFarmsMarket.Core/bin/Debug/netcoreapp3.1/Logs");
            if (string.IsNullOrWhiteSpace(fileId))
                return BadRequest(Utilities.CreateResponse<string>("No file name found", null, ""));

            if (!System.IO.File.Exists($"{assemblyFullPath}/" + fileId))
                return NotFound(Utilities.CreateResponse<string>($"File {fileId} does not exist", null, ""));

            var res = System.IO.File.ReadAllText(assemblyFullPath + $"/{fileId}");
            return Ok(Utilities.CreateResponse<string>("File read successfully", null, res));
        }

        [HttpPost("{id}")]
        public IActionResult Delete(string id)
        {
            var assemblyFullPath = Path.GetFullPath(@"../AnimalFarmsMarket.Core/bin/Debug/netcoreapp3.1/Logs");
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(Utilities.CreateResponse<string>("No file name found", null, ""));

            if (!System.IO.File.Exists($"{assemblyFullPath}/" + id))
                return NotFound(Utilities.CreateResponse<string>($"File {id} does not exist", null, ""));

            System.IO.File.Delete($"{assemblyFullPath}/" + id);
            return Ok(Utilities.CreateResponse<string>("File Deleted successfully", null, ""));
        }
    }
}
