using System;
using Annium.Id.AspNetCore;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.DemoClient.Controllers
{
    [Route("/")]
    public class IndexController : ControllerBase
    {
        private readonly ITokenAccessor tokenAccessor;

        public IndexController(
            ITokenAccessor tokenAccessor
        )
        {
            this.tokenAccessor = tokenAccessor;
        }

        [HttpGet("base")]
        [Authorize]
        public IActionResult Base()
        {
            return new JsonResult(tokenAccessor.GetToken());
        }

        [HttpGet("isAdmin")]
        [Authorize("isAdmin")]
        public IActionResult IsAdmin()
        {
            return new JsonResult(tokenAccessor.GetToken());
        }

        [HttpGet("hasPaymentsAccess")]
        [Authorize("hasPaymentsAccess")]
        public IActionResult HasPaymentsAccess()
        {
            return new JsonResult(tokenAccessor.GetToken());
        }

        [HttpGet("hasCompanyPaymentsAccess/{companyId:guid}")]
        [Authorize("hasCompanyPaymentsAccess")]
        public IActionResult HasCompanyPaymentsAccess(Guid companyId)
        {
            return new JsonResult(tokenAccessor.GetToken());
        }
    }
}