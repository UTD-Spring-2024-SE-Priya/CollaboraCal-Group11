using System;
using System.Linq;
using System.Security.Cryptography;

public class AccountController
{
    DatabaseController dbController;

    public AccountController(DatabaseController databaseController) 
    { 
        dbController = databaseController; 
    }
    
    public string createUser(string username, string password)
    {
        (string, bool) userCredValidation = UserPassValid(username, password);
        if (userCredValidation.Item2)
        {
            User user = new()
            {
                Username = username,
                PasswordHashData = new SecureHash<SHA256>(password)
            };
            dbController.AddUser(user);
        }
        return userCredValidation.Item1;
    }

    private (string, bool) UserPassValid(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return ("Username field is blank", false);
        if (string.IsNullOrWhiteSpace(password))
            return ("Password field is blank", false);
        if (dbController.GetUserFromUsername(username) != null)
            return ("Username already exists", false);
        if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsNumber))
            return ("Password must be at least 8 characters long with at least one uppercase letter, lowercase letter, and number", false);
        return ("Username and Password are Valid", true);
    }
}
