
using System;

[Serializable]
public class User
{
    public int ID { get; set; }
    public string? EMail { get; set; }
    public string? Username { get; set; }
    //public SecureSHA256? HashedPassword { get; set; }
}