using System;
using System.Collections.Generic;
using MessagePack;

namespace Annium.Id.Core
{
    [MessagePackObject]
    public class IdToken
    {
        [Key(0)]
        public Guid UserId { get; }

        [Key(1)]
        public Guid LoginId { get; }

        [Key(2)]
        public AppToken App { get; }

        [Key(3)]
        public IReadOnlyCollection<CompanyToken> Companies { get; }

        public IdToken(
            Guid userId,
            Guid loginId,
            AppToken app,
            IReadOnlyCollection<CompanyToken> companies
        )
        {
            UserId = userId;
            LoginId = loginId;
            App = app;
            Companies = companies;
        }
    }

    [MessagePackObject]
    public class AppToken
    {
        [Key(0)]
        public Guid Id { get; }

        [Key(1)]
        public IReadOnlyCollection<string> Roles { get; }

        [Key(2)]
        public IReadOnlyDictionary<string, string> Claims { get; }

        public AppToken(
            Guid id,
            IReadOnlyCollection<string> roles,
            IReadOnlyDictionary<string, string> claims
        )
        {
            Id = id;
            Roles = roles;
            Claims = claims;
        }
    }

    [MessagePackObject]
    public class CompanyToken
    {
        [Key(0)]
        public Guid Id { get; }

        [Key(1)]
        public IReadOnlyCollection<string> Roles { get; }

        [Key(2)]
        public IReadOnlyDictionary<string, string> Claims { get; }

        public CompanyToken(
            Guid id,
            IReadOnlyCollection<string> roles,
            IReadOnlyDictionary<string, string> claims
        )
        {
            Id = id;
            Roles = roles;
            Claims = claims;
        }
    }
}