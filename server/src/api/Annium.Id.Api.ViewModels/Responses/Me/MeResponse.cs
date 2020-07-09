using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.ViewModels.Responses.Me
{
    public class MeResponse : IResponse<User>
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid? ReferralId { get; set; }
    }
}