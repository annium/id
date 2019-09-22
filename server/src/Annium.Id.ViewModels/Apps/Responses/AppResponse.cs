using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Apps.Responses
{
    public class AppResponse : IResponse<App>
    {
        public Guid Id { get; }
        public string Key { get; }
        public string Name { get; }

        public AppResponse(
            Guid id,
            string key,
            string name
        )
        {
            Id = id;
            Key = key;
            Name = name;
        }
    }
}