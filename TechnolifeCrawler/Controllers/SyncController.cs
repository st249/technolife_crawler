using Microsoft.AspNetCore.Mvc;

namespace TechnolifeCrawler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {

        public SyncController()
        {

        }

        [HttpGet(Name ="test")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
