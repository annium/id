using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Queries.Roles;

namespace Annium.Id.ViewModels.Roles.Requests
{
    public class ListRolesRequest : IRequest<ListRolesQuery>
    {
        public Guid AppId { get; set; }
    }
}