using Annium.Core.Mapper;
using Server.Domain.Queries.Companies;

namespace Server.ViewModels.Requests.Companies.Profiles;

internal class ListMyCompaniesRequestProfile : Profile
{
    public ListMyCompaniesRequestProfile()
    {
        Map<ListMyCompaniesRequest, ListMyCompaniesQuery>(r => new ListMyCompaniesQuery());
    }
}