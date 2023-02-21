using Bogus;

namespace Server.Host.TestClient;

internal static class Helper
{
    public static Faker Faker { get; } = new();
}