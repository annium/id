using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Apps;

namespace Annium.Id.Api.ViewModels.Requests.Apps
{
    public class FindAppsRequest : IRequest<FindAppsQuery>
    {
        public string Query { get; set; } = string.Empty;
    }
}