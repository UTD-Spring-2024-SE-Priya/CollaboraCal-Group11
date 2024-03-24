using System;
using System.Diagnostics;
using System.Security.Principal;

public struct Authentication
{
    
}

// AUTHENTICATION SYSTEM: "Is the user who they say they are?"
// DIFFERENT THAN THE AUTHORIZATION SYSTEM, which is "Does this user have permission to do this?"
public class AuthenticationSystem
{

    public AuthenticationSystem()
    {
        activeSessions = new();
    }

    private Dictionary<Authentication, LoginSession> activeSessions;

    private bool HasExpired(LoginSession session) => session.Expires < DateTime.Now;

    private User? GetUserFromToken(Authentication auth)
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

    public bool ValidateAuthentication(User user, Authentication auth)
    {
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

    public Authentication? Login(string username, string password)
    {
        
        return new Authentication();
    }

}