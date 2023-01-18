using Bogus;

namespace Server.TestClient;

internal static class Helper
{
    public static Faker Faker { get; } = new();
}