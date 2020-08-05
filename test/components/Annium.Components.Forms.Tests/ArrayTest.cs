using System;
using System.Collections.Generic;
using System.Linq;
using Annium.Testing;
using Xunit;

namespace Annium.Components.Forms.Tests
{
    public class ArrayTest : TestBase
    {
        [Fact]
        public void Init_Ok()
        {
            // arrange
            var factory = GetFactory();
            var value = Arrange();

            // act
            var state = factory.Create(value);
            state.At(x => x.Author).At(x => x.Name).Set("yyy");
            state.Messages[1].Text.Set("xxx");
            state.At(x => x.Messages[1].Text).Set("xxx");
            state.At(x => x.Messages).At(1).At(x => x.Text).Set("yyy");

            // assert
            // access value by path
            state.At(x => x.Messages).At(0).Value.Text.IsEqual("one");
            state.At(x => x.Messages).At(0).At(x => x.Text).Value.IsEqual("one");
            state.At(x => x.Messages.ElementAt(0).Text).Value.IsEqual("one");
            // modify state
            state.At(x => x.Messages).Add(new Message { Text = "three" });
            state.At(x => x.Messages.ElementAt(2).IsRead).Set(true);
            state.At(x => x.Messages.ElementAt(2)).Value.IsEqual(new Message { Text = "three", IsRead = true });
        }

        private Blog Arrange() => new Blog
        {
            Name = "Sample",
            Author = new User { Name = "Max" },
            Messages = new[] { new Message { Text = "one" }, new Message { Text = "two" } },
        };

        private class Blog
        {
            public string Name { get; set; } = string.Empty;
            public User Author { get; set; } = default!;
            public IEnumerable<Message> Messages { get; set; } = Array.Empty<Message>();
        }

        private class User
        {
            public string Name { get; set; } = string.Empty;
        }

        private class Message
        {
            public string Text { get; set; } = string.Empty;
            public bool IsRead { get; set; }
        }
    }
}