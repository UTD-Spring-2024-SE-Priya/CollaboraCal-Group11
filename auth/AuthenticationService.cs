using System;
using System.Diagnostics;
using System.Security.Cryptography;

// AUTHENTICATION SYSTEM: "Is the user who they say they are?"
// DIFFERENT THAN THE AUTHORIZATION SYSTEM, which is "Does this user have permission to do this?"
public class AuthenticationSystem
{

    public AuthenticationSystem()
    {
        activeSessions = new();
    }

    private Dictionary<string, LoginSession> activeSessions;

    private bool HasExpired(LoginSession session) => session.Expires < DateTime.Now;

    private User? GetUserFromToken(string auth)
    {
        if(activeSessions.TryGetValue(auth, out LoginSession? session))
        {
            if (HasExpired(session))
            {
                activeSessions.Remove(auth);
                return null;
            }
            return session?.User;
        }
        return null;
    }


    public void CullActiveSessions()
    {
        List<string> removeList = new();
        foreach (KeyValuePair<string, LoginSession> kvp in activeSessions)
        {
            if (HasExpired(kvp.Value))
                removeList.Add(kvp.Key);
        }
        foreach(string token in removeList)
        {
            activeSessions.Remove(token);
        }
    }

    public bool ValidateAuthentication(User user, string? auth)
    {
        if (auth == null) return false;
        if (!activeSessions.ContainsKey(auth)) return false;
        LoginSession session = activeSessions[auth];
        if (session.User != user) return false;
        if (HasExpired(session)) return false;
        return true;
    }

    public bool DoesUserHaveActiveSession(User user)
    {
        if (user.Session == null) return false;
        if (HasExpired(user.Session))
        {
            user.Session = null;
            return false;
        }
        return ValidateAuthentication(user, user.Session.Authentication);
    }

    private string GenerateUniqueRandomAuthcode()
    {
        string authtoken;
        do
        {
            authtoken = new SecureHash<SHA256>(Random.Shared.Next()).ToString();
        }
        while(activeSessions.ContainsKey(authtoken));
        return authtoken;
    }

    public string? Login(string username, string password)
    {
        User? user = Application.Database.GetUserFromUsername(username);
        if (user == null) return null;
        
        if(DoesUserHaveActiveSession(user))
        {
            return user.Session?.Authentication;
        }

        var suppliedHash = new SecureHash<SHA256>(password);
        var realHash = user.PasswordHashData;

        if (suppliedHash == realHash)
        {

        }

        return GenerateUniqueRandomAuthcode();
    }

}