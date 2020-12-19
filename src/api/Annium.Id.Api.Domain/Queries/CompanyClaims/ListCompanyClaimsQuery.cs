using System;
using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Queries.CompanyClaims
{
    public class ListCompanyClaimsQuery : IQuery
    {
        public Guid AppId { get; }
        public App App { get; private set; } = null!;

        public ListCompanyClaimsQuery(Guid appId)
        {
            AppId = appId;
        }
    }
}