using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Companies;

namespace Server.ViewModels.Requests.Companies;

public class FindCompaniesRequest : IRequest<FindCompaniesQuery>
{
    public string Query { get; set; } = string.Empty;
}