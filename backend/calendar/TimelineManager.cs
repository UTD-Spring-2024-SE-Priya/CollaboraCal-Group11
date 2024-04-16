struct TimelineManager
{

    DateTime Start { get; }
    DateTime End { get; }

    public TimelineManager(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public bool Within(DateTime dt)
    {
        return dt >= Start && dt <= End;
    }

    public bool Overlaps(DateTime start, DateTime end)
    {
        return !(start > End || end < Start);
    }

}