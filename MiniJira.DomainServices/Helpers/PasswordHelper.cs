using System.Security.Cryptography;

namespace MiniJira.DomainServices.Helpers;

public static class PasswordHelper
{
    private const int SaltSize = 0x10;
    private const int IterationsCount = 0x3e8;
    
    public static string HashPassword(string password)
    {
        byte[] salt;
        byte[] buffer2;
        ArgumentNullException.ThrowIfNull(password);

        using (var bytes = new Rfc2898DeriveBytes(password, SaltSize, IterationsCount, HashAlgorithmName.SHA1))
        {
            salt = bytes.Salt;
            buffer2 = bytes.GetBytes(0x20);
        }

        var dst = new byte[0x31];
        Buffer.BlockCopy(salt, 0, dst, 1, SaltSize);
        Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        return Convert.ToBase64String(dst);
    }
    
    public static bool VerifyHashedPassword(string? hashedPassword, string password)
    {
        byte[] buffer4;
        if (hashedPassword == null)
        {
            return false;
        }
        ArgumentNullException.ThrowIfNull(password);
        var src = Convert.FromBase64String(hashedPassword);
        if ((src.Length != 0x31) || (src[0] != 0))
        {
            return false;
        }
        var dst = new byte[SaltSize];
        Buffer.BlockCopy(src, 1, dst, 0, SaltSize);
        var buffer3 = new byte[0x20];
        Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
        using (var bytes = new Rfc2898DeriveBytes(password, dst, IterationsCount, HashAlgorithmName.SHA1))
        {
            buffer4 = bytes.GetBytes(0x20);
        }
        return buffer3.SequenceEqual(buffer4);
    }
}