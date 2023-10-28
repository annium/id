using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Models;

namespace Server.ViewModels.Responses.Apps;

public record AppResponse : IResponse<App>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
