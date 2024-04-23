using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using CollaboraCal.JsonRequests;
using Microsoft.EntityFrameworkCore.Storage;

namespace CollaboraCal;




public class CalendarManager
{

    public int USER_CALENDAR_LIMIT = 25;

    public bool DoesCalendarBelongToUser(string email, int calendarID)
    {
        Calendar? heavyCalendar = Application.Database.GetHeavyCalendar(calendarID);
        if(heavyCalendar == null) return false;
        return DoesCalendarBelongToUser(email, heavyCalendar);
    }

    public bool DoesCalendarBelongToUser(string email, Calendar calendar)
    {
        if(calendar == null) return false;
        if (calendar.Users == null)
        {
            throw new Exception("DoesCalendarBelongToUser(string,Calendar) ONLY accents HEAVY Calendar");
        }
        return calendar.Users.Exists(a => a.EMail == email);
    }

    public Calendar? CreateCalendar(string ownerEmail, NewCalendarData data)
    {
        User? user = Application.Database.GetHeavyUserFromEmail(ownerEmail);

        if (user == null) return null;
        if (user.Calendars?.Count >= USER_CALENDAR_LIMIT) return null;
        if(string.IsNullOrEmpty(data.Name) || string.IsNullOrEmpty(data.Description)) return null;

        Calendar calendar = new Calendar()
        {
            Name = data.Name,
            Description = data.Description,

            Events = new(),
            Users = new List<User>() { user },
        };

        user.Calendars?.Add(calendar);
        Application.Database.AddCalendar(calendar);

        return calendar;
    }

    public bool DeleteCalendar(int calendarID)
    {
        Calendar? calendar = Application.Database.GetHeavyCalendar(calendarID);
        if (calendar == null) return false;
        if (calendar.Users == null) return false;

        foreach(User user in calendar.Users)
        {
            User? heavy = Application.Database.GetHeavyUserFromID(user.ID);
            if (heavy == null) continue;

            heavy.Calendars?.RemoveAll(a => a.ID == calendar.ID);
        }

        Application.Database.Context.Calendars.Remove(calendar);
        Application.Database.Context.SaveChanges();
        return true;
    }

    public bool ShareCalendar(string fromEmail, CalendarShareRequest request)
    {
        if (request.ShareEmail == null) return false;
        Calendar? calendar = Application.Database.GetUserHeavyCalendar(request.CalendarID);
        if (calendar == null) return false;
        if (calendar.Users == null) return false;

        if (!DoesCalendarBelongToUser(fromEmail, calendar))
        {
            return false;
        }

        User? toUser = Application.Database.GetHeavyUserFromEmail(request.ShareEmail);
        if (toUser == null) return false;
        if (toUser.Calendars == null) return false;

        toUser.Calendars.Add(calendar);
        calendar.Users.Add(toUser);
        Application.Database.Context.SaveChanges();

        return true;
    }

    public bool CreateEvent(string email, NewEventData data)
    {
        Calendar? heavyCalendar = Application.Database.GetHeavyCalendar(data.CalendarID);
        if (heavyCalendar == null) return false;

        if (!DoesCalendarBelongToUser(email, heavyCalendar))
        {
            return false;
        }

        Event ev = new Event()
        {
            Name = data.Name,
            Description = data.Description,
            Location = data.Location,
            Start = data.StartTime,
            End = data.EndTime,
            Calendar = heavyCalendar,
        };

        heavyCalendar.Events?.Add(ev);
        Application.Database.AddEvent(ev);

        return true;
    }

    public bool DeleteEvent(string email, int eventID)
    {
        Event? e = Application.Database.GetEventFromID(eventID);
        if (e == null) return false;
        if (e.Calendar == null) return false;

        if (DoesCalendarBelongToUser(email, e.Calendar.ID))
        {
            Application.Database.Context.Events.Remove(e);
            Application.Database.Context.SaveChanges();
            return true;
        }
        return false;
    }

}