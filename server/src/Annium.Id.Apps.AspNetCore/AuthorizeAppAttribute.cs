using System;

namespace Annium.Id.Apps.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AuthorizeAppAttribute : Attribute { }
}