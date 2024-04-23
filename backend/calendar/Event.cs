using System;
using System.Collections.Generic;

namespace CollaboraCal;

public class Event
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public Calendar? Calendar { get; set; }
}