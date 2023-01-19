using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Companies;

namespace Server.ViewModels.Requests.Companies;

public record ListMyCompaniesRequest : IRequest<ListMyCompaniesQuery>
{
}