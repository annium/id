using Bogus;

namespace Server.Host.TestClient.Clients;

internal static class Helper
{
    public static Faker Faker { get; } = new();
}
