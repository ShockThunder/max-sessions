namespace MaxSessions;

public class Record
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Project { get; set; }
    public string Operator { get; set; }
    public string State { get; set; }
    public int Duration { get; set; }
}