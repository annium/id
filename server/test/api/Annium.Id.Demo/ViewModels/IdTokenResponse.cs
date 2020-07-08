using System;
using System.Collections.Generic;

namespace Annium.Id.Demo.ViewModels
{
    public class IdTokenResponse
    {
        public Guid UserId { get; private set; }
        public Guid LoginId { get; private set; }
        public AppTokenResponse App { get; private set; } = default!;
        public IReadOnlyCollection<CompanyTokenResponse> Companies { get; private set; } = Array.Empty<CompanyTokenResponse>();
    }

    public class AppTokenResponse
    {
        public Guid Id { get; private set; }
        public IReadOnlyCollection<string> Roles { get; private set; } = Array.Empty<string>();
        public IReadOnlyDictionary<string, string> Claims { get; private set; } = new Dictionary<string, string>();
    }

    public class CompanyTokenResponse
    {
        public Guid Id { get; private set; }
        public IReadOnlyCollection<string> Roles { get; private set; } = Array.Empty<string>();
        public IReadOnlyDictionary<string, string> Claims { get; private set; } = new Dictionary<string, string>();
    }
}