using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.DemoClient.Controllers
{
    [Route("/")]
    public class IndexController : ControllerBase
    {
        public IndexController()
        {

        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello World from Annium.Id.DemoClient");
        }
    }
}