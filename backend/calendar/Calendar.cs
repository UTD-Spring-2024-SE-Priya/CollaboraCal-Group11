using System;
using System.Collections.Generic;

namespace CollaboraCal;

[Serializable]
public class Calendar
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Event>? Events { get; set; }
    public List<User>? Users { get; set; }
}