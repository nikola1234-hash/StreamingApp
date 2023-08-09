using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StreamingApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _fileDownloadPath;

        public FileController(IConfiguration configuration)
        {
            _fileDownloadPath = configuration.GetValue<string>("FileDownloadPath");
        }


        [HttpGet("{fileName}")]
        public IActionResult Download(string fileName)
        {
            var filepath = Path.Combine(_fileDownloadPath, fileName);
            if (!System.IO.File.Exists(filepath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filepath), Path.GetFileName(filepath));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
           
            };
        }
    }
}
