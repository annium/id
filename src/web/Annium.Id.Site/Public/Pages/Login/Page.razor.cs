using System;
using Annium.Blazor.Core.Extensions;
using Annium.Components.State;

namespace Annium.Id.Site.Public.Pages.Login
{
    public partial class Page : IDisposable
    {
        private IObjectContainer<LoginData> State => Store.State;

        private IDisposable _observerDisposer = default!;

        protected override void OnInitialized()
        {
            _observerDisposer = this.ObserveState();
        }

        public void Dispose()
        {
            _observerDisposer.Dispose();
        }
    }
}