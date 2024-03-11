using System.Security.Cryptography;
using System.Text;

namespace WorkTimer.Api.Utils;

public static class Extensions
{
    public static string GetHash(this string value)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}