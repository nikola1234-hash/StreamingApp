using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StreamingApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        private readonly string _videoStreamPath;

        public StreamController(IConfiguration configuration)
        {
            _videoStreamPath = configuration.GetValue<string>("VideoStreamPath");
        }

        [HttpGet("{videoName}")]
        public IActionResult StreamVideo(string videoName)
        {
            var filepath = Path.Combine(_videoStreamPath, videoName);
            if (!System.IO.File.Exists(filepath))
                return NotFound();

            return new FileStreamResult(new FileStream(filepath, FileMode.Open, FileAccess.Read), "video/mp4");
        }
    }
}
