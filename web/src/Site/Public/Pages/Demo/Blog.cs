using System.Collections.Generic;
using Annium.Extensions.Validation;

namespace Site.Public.Pages.Demo;

public class Blog
{
    public string Name { get; set; } = string.Empty;
    public User Author { get; set; } = new User();
    public List<Message> Messages { get; set; } = new();
}

public class User
{
    public string Name { get; set; } = string.Empty;
}

public class Message
{
    public string Text { get; set; } = string.Empty;
    public bool IsRead { get; set; }
}

internal class BlogValidator : Validator<Blog>
{
    public BlogValidator()
    {
        Field(x => x.Name).Required().Then().Length(3, 20);
    }
}
