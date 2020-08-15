using System;
using System.Collections.Generic;
using Annium.Extensions.Validation;

namespace Annium.Id.Site.Public.Pages
{
    public class Blog
    {
        public string Name { get; set; } = string.Empty;
        public User Author { get; set; } = new User();
        public IEnumerable<Message> Messages { get; set; } = Array.Empty<Message>();
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
}