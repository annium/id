using System;

namespace Annium.Id.Api.AppAuth
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AuthorizeAppAttribute : Attribute { }
}