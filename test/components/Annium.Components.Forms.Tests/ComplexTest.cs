using System;
using System.Collections.Generic;
using System.Linq;
using Annium.Testing;
using Xunit;

namespace Annium.Components.Forms.Tests
{
    public class ComplexTest : TestBase
    {
        [Fact]
        public void Init_Ok()
        {
            // arrange
            var factory = GetFactory();
            var value = Arrange();

            // act
            var state = factory.Create(value);

            // assert
            state.At(x => x.Name).Value.IsEqual(value.Name);
            state.At(x => x.Author).Value.IsEqual(value.Author);
            state.At(x => x.Author).At(x => x.Name).Value.IsEqual(value.Author.Name);
            state.At(x => x.Messages).At(x => x[0]).Value.IsEqual(value.Messages.ElementAt(0));
            state.At(x => x.Messages).At(x => x[0]).At(x => x.Text).Value.IsEqual("one");
            state.At(x => x.Messages).At(x => x[0]).At(x => x.IsRead).Value.IsEqual(true);
            state.HasChanged.IsFalse();
            state.HasBeenTouched.IsFalse();
        }

        private Blog Arrange() => new Blog
        {
            Name = "Sample",
            Author = new User { Name = "Max" },
            Messages = new[] { new Message { Text = "one", IsRead = true }, new Message { Text = "two" } },
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