function formatTime(date)
{
    let ampm = " AM";
    let hours = date.getHours()
    if (hours == 0)
    {
        hours = 12;
        ampm = " AM";
    }
    else if (hours == 12)
    {
        ampm = " PM";
    }
    else if (hours > 12)
    {
        hours -= 12;
        ampm = " PM";
    }

    return hours + ":" + date.getMinutes().toString().padStart(2, '0') + ampm;
}

class EventData
{
    constructor(id, name, description, date)
    {
        this.ID = id;
        this.Name = name;
        this.Description = description;
        this.Date = new Date(Date.parse(date));
        this.Time = formatTime(this.Date);
    }
}

export default EventData

// record EventListResponse(string? name, string? description, string? location, DateTime start, DateTime end);
