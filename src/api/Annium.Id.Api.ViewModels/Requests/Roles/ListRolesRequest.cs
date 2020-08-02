using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Roles;

namespace Annium.Id.Api.ViewModels.Requests.Roles
{
    public class ListRolesRequest : IRequest<ListRolesQuery>
    {
        public Guid AppId { get; set; }
    }
}