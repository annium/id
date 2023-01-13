using System;
using System.Security.Cryptography;
using System.Text;

namespace Server.Application.Tools;

internal class SecurityManager : ISecurityManager, IDisposable
{
    private readonly HashAlgorithm _hashAlgorithm = SHA512.Create();

    public string Hash(string data)
    {
        return Convert.ToBase64String(_hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data)));
    }

    public void Dispose()
    {
        _hashAlgorithm.Dispose();
    }
}