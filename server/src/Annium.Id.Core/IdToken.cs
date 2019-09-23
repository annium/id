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
        public IEnumerable<CompanyToken> Companies { get; }

        public IdToken(
            Guid userId,
            Guid loginId,
            AppToken app,
            IEnumerable<CompanyToken> companies
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
        public string Key { get; }

        [Key(2)]
        public Guid OwnerId { get; }

        [Key(3)]
        public IEnumerable<string> Roles { get; }

        [Key(4)]
        public IReadOnlyDictionary<string, string> Claims { get; }

        public AppToken(
            Guid id,
            string key,
            Guid ownerId,
            IEnumerable<string> roles,
            IReadOnlyDictionary<string, string> claims
        )
        {
            Id = id;
            Key = key;
            OwnerId = ownerId;
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
        public string Key { get; }

        [Key(2)]
        public Guid OwnerId { get; }

        [Key(3)]
        public IEnumerable<string> Roles { get; }

        [Key(4)]
        public IReadOnlyDictionary<string, string> Claims { get; }

        public CompanyToken(
            Guid id,
            string key,
            Guid ownerId,
            IEnumerable<string> roles,
            IReadOnlyDictionary<string, string> claims
        )
        {
            Id = id;
            Key = key;
            OwnerId = ownerId;
            Roles = roles;
            Claims = claims;
        }
    }
}