using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Roles;

namespace Annium.Id.Api.ViewModels.Roles.Requests
{
    public class ListRolesRequest : IRequest<ListRolesQuery>
    {
        public Guid AppId { get; set; }
    }
}