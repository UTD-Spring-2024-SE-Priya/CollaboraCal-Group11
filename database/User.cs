using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

[Serializable]
public class User
{
    public int ID { get; set; }
    public string? EMail { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; private set; }

    [NotMapped]
    public SecureHash<SHA256>? PasswordHashData
    {
        get => SecureHash<SHA256>.FromHexString(PasswordHash);
        set => PasswordHash = value?.ToString();
    }

    [NotMapped]
    public LoginSession? Session { get; set; }
}

