using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using TicketingSystem.Application.Common.Security;

namespace TicketingSystem.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int IterationCount = 10000;
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public string HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        var hash = KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA256,
            IterationCount,
            KeySize);

        var result = new byte[SaltSize + KeySize];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, result, SaltSize, KeySize);

        return Convert.ToBase64String(result);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var decoded = Convert.FromBase64String(hashedPassword);

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(decoded, 0, salt, 0, SaltSize);

        var storedHash = new byte[KeySize];
        Buffer.BlockCopy(decoded, SaltSize, storedHash, 0, KeySize);

        var hash = KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA256,
            IterationCount,
            KeySize);

        return CryptographicOperations.FixedTimeEquals(storedHash, hash);
    }
}