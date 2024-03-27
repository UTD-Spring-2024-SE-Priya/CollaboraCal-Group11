using System;
using System.Linq;
using System.Security.Cryptography;

public class AccountController
{
    
    public string CreateUser(string email, string name, string password)
    {
        (string, bool) userCredValidation = EmailPassValid(email, password);
        if (userCredValidation.Item2)
        {
            User user = new()
            {
                EMail = email,
                PasswordHashData = new SecureHash<SHA256>(password),
                Name = name,
            };
            Application.Database.AddUser(user);
        }
        return userCredValidation.Item1;
    }

    private (string, bool) EmailPassValid(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            return ("Email field is blank", false);
        if (string.IsNullOrWhiteSpace(password))
            return ("Password field is blank", false);
        if (Application.Database.GetUserFromEmail(email) != null)
            return ("Email already exists", false);
        if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsNumber))
            return ("Password must be at least 8 characters long with at least one uppercase letter, lowercase letter, and number", false);
        return ("Email and Password are Valid", true);
    }
}
