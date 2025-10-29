namespace Core;

public static class Password
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string providedPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, storedHash);
    }
}