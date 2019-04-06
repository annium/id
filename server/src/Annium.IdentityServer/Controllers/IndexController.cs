using Microsoft.AspNetCore.Mvc;

namespace Annium.IdentityServer.Controllers
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
            return Ok("Hello World from Annium.IdentityServer");
        }
    }
}