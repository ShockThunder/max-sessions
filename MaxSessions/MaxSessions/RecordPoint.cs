namespace MaxSessions;

public class RecordPoint
{
    public DateTime Date { get; set; }
    public PointType Type { get; set; }
}

public enum PointType
{
    Start,
    End
}