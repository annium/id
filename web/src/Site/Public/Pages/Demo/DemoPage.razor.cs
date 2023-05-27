using System.Collections.Generic;
using Annium.Components.State.Forms;
using Annium.Components.State.Forms.Extensions;

namespace Site.Public.Pages.Demo;

public partial class DemoPage
{
    private IObjectContainer<Blog> _state = default!;

    protected override void OnInitialized()
    {
        _state = StateFactory.CreateObject(new Blog
        {
            Name = "Demo",
            Author = new User
            {
                Name = "",
            },
            Messages = new List<Message>
            {
                new()
                {
                    Text = "Hi"
                }
            }
        });
        ObserveStates();
        _state.UseValidator(Validator);
    }
}