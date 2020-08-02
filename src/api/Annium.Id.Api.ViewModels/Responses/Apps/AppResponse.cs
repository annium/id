using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.ViewModels.Responses.Apps
{
    public class AppResponse : IResponse<App>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}