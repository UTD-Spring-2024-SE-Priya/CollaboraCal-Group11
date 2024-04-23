using Newtonsoft.Json;
using CollaboraCal;

namespace CollaboraCal.JsonRequests;

public record NewCalendarData(
    [JsonProperty("name")] string Name,
    [JsonProperty("desc")] string Description
);

public record NewEventData(
    [JsonProperty("name")] string? Name,
    [JsonProperty("description")] string? Description,
    [JsonProperty("location")] string? Location,
    [JsonProperty("start")] DateTime StartTime,
    [JsonProperty("end")] DateTime EndTime,
    [JsonProperty("calendarID")] int CalendarID
);


public record CalendarEventRequest(
    [JsonProperty("start")] DateTime StartTime,
    [JsonProperty("end")] DateTime EndTime,
    [JsonProperty("calendarID")] int CalendarID
);

public record CalendarShareRequest(
    [JsonProperty("calendarID")] int CalendarID,
    [JsonProperty("to")] string? ShareEmail
);