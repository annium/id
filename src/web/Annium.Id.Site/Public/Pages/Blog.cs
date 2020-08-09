using System;
using System.Collections.Generic;

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
}