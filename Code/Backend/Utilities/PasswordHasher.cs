using BCrypt.Net;

namespace ReMarket.Utilities
{
public static class PasswordHasher
{
    private const int WorkFactor = 12;

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}}