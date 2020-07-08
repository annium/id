using System;
using Annium.Core.Mapper;
using Annium.Id.AspNetCore;
using Annium.Id.Core;
using Annium.Id.Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Demo.Controllers
{
    [Route("/")]
    public class IndexController : ControllerBase
    {
        private readonly ITokenAccessor _tokenAccessor;
        private readonly IMapper _mapper;

        public IndexController(
            ITokenAccessor tokenAccessor,
            IMapper mapper
        )
        {
            _tokenAccessor = tokenAccessor;
            _mapper = mapper;
        }

        [HttpGet("base")]
        [Authorize]
        public IdTokenResponse Base()
        {
            return _mapper.Map<IdTokenResponse>(_tokenAccessor.GetToken());
        }

        [HttpGet("isAdmin")]
        [Authorize("isAdmin")]
        public IdTokenResponse IsAdmin()
        {
            return _mapper.Map<IdTokenResponse>(_tokenAccessor.GetToken());
        }

        [HttpGet("hasPaymentsAccess")]
        [Authorize("hasPaymentsAccess")]
        public IdTokenResponse HasPaymentsAccess()
        {
            return _mapper.Map<IdTokenResponse>(_tokenAccessor.GetToken());
        }

        [HttpGet("hasCompanyPaymentsAccess/{companyId:guid}")]
        [Authorize("hasCompanyPaymentsAccess")]
        public IdTokenResponse HasCompanyPaymentsAccess(Guid companyId)
        {
            return _mapper.Map<IdTokenResponse>(_tokenAccessor.GetToken());
        }
    }
}