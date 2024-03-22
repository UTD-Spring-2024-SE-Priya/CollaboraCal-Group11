using System;
using System.Diagnostics;
using System.Security.Principal;

class LoginSession
{
    public DateTime Expires { get; set; }
    public User? User { get; set; }
    public Authentication Authentication { get; set; }
}

public struct Authentication
{
    
}

// AUTHENTICATION SYSTEM: "Is the user who they say they are?"
// DIFFERENT THAN THE AUTHORIZATION SYSTEM, which is "Does this user have permission to do this?"
public static class AuthenticationSystem
{

    static AuthenticationSystem()
    {
        activeSessions = new();
    }

    private static Dictionary<Authentication, LoginSession> activeSessions;

    private static bool HasExpired(LoginSession session) => session.Expires < DateTime.Now;

    private static User? GetUserFromToken(Authentication auth)
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

    

    public static void CullActiveSessions()
    {
        List<Authentication> removeList = new();
        foreach (KeyValuePair<Authentication, LoginSession> kvp in activeSessions)
        {
            if (HasExpired(kvp.Value))
                removeList.Add(kvp.Key);
        }
        foreach(Authentication token in removeList)
        {
            activeSessions.Remove(token);
        }
    }

    public static bool ValidateAuthentication(User user, Authentication auth)
    {
        if (!activeSessions.ContainsKey(auth)) return false;
        LoginSession session = activeSessions[auth];
        if (session.User != user) return false;
        if (HasExpired(session)) return false;
        return true;
    }

}