using System;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.DemoClient.Controllers
{
    [Route("/")]
    public class IndexController : ControllerBase
    {
        public IndexController()
        {

        }

        [HttpGet("base")]
        [Authorize]
        public IActionResult Base()
        {
            return new JsonResult(this.GetBaseId());
        }

        [HttpGet("isAdmin")]
        [Authorize("isAdmin")]
        public IActionResult IsAdmin()
        {
            return new JsonResult(this.GetAppId());
        }

        [HttpGet("hasPaymentsAccess")]
        [Authorize("hasPaymentsAccess")]
        public IActionResult HasPaymentsAccess()
        {
            return new JsonResult(this.GetAppId());
        }

        [HttpGet("isCompanyOwner/{companyId:guid}")]
        [Authorize("isCompanyOwner")]
        public IActionResult IsCompanyOwner(Guid companyId)
        {
            return new JsonResult(this.GetAppId());
        }

        [HttpGet("hasCompanyPaymentsAccess/{companyId:guid}")]
        [Authorize("hasCompanyPaymentsAccess")]
        public IActionResult HasCompanyPaymentsAccess(Guid companyId)
        {
            return new JsonResult(this.GetAppId());
        }
    }
}