using System;
using System.Security.Cryptography;
using System.Text;

namespace Annium.Id.Api.Application.Tools
{
    internal class SecurityManager : ISecurityManager, IDisposable
    {
        private readonly HashAlgorithm hashAlgorithm = new SHA512CryptoServiceProvider();

        public string Hash(string data)
        {
            return Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data)));
        }

        public void Dispose()
        {
            hashAlgorithm.Dispose();
        }
    }
}