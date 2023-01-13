using Annium.Blazor.Core.Extensions;
using Annium.Components.State.Forms;
using Annium.Components.State.Forms.Extensions;

namespace Site.Public.Pages.Demo;

public partial class DemoPage
{
    private IObjectContainer<Blog> _state = default!;

    protected override void OnInitialized()
    {
        _state = StateFactory.Create(new Blog
        {
            Name = "Demo",
            Author = new User
            {
                Name = "",
            },
            Messages = new[]
            {
                new Message
                {
                    Text = "Hi"
                }
            }
        });
        this.ObserveState();
        _state.UseValidator(Validator);
    }
}