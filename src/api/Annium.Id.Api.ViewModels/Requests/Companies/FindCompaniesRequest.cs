using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Queries.Companies;

namespace Annium.Id.Api.ViewModels.Requests.Companies;

public class FindCompaniesRequest : IRequest<FindCompaniesQuery>
{
    public string Query { get; set; } = string.Empty;
}