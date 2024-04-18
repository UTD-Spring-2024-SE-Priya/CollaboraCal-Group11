using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using CollaboraCal.JsonRequests;

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
        if (calendar.Users == null)
        {
            throw new Exception("DoesCalendarBelongToUser(string,Calendar) ONLY accents HEAVY Calendar");
        }
        return calendar.Users.Exists(a => a.EMail == email);
    }

    public bool CreateCalendar(string ownerEmail, NewCalendarData data)
    {
        User? user = Application.Database.GetHeavyUserFromEmail(ownerEmail);

        if (user == null) return false;
        if (user.Calendars?.Count >= USER_CALENDAR_LIMIT) return false;

        Calendar calendar = new Calendar()
        {
            Name = data.Name,
            Description = data.Description,

            Events = new(),
            Users = new List<User>() { user },
        };

        user.Calendars?.Add(calendar);
        Application.Database.AddCalendar(calendar);

        return true;
    }

    public bool CreateEvent(string email, NewEventData data)
    {

        Calendar heavyCalendar = Application.Database.GetHeavyCalendar(data.CalendarID);
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

        Application.Database.AddEvent(ev);

        return true;
    }

}