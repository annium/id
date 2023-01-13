using System;
using Annium.Architecture.ViewModel;
using Core.Domain.Entities;

namespace Server.ViewModels.Responses.Apps;

public class AppResponse : IResponse<App>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}