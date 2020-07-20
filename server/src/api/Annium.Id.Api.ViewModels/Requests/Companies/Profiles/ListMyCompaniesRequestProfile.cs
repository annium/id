using Annium.Core.Mapper;
using Annium.Id.Api.Application.Queries.Companies;

namespace Annium.Id.Api.ViewModels.Requests.Companies.Profiles
{
    internal class ListMyCompaniesRequestProfile : Profile
    {
        public ListMyCompaniesRequestProfile()
        {
            Map<ListMyCompaniesRequest, ListMyCompaniesQuery>(r => new ListMyCompaniesQuery());
        }
    }
}