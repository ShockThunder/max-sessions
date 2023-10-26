using System.Text;

namespace MaxSessions;

public class ReportGenerator
{
    public string GenerateReport(List<ReportLine> lines)
    {
        var orderedLines = lines.OrderBy(rl => rl.Date);
        var sb = new StringBuilder();
        foreach (var line in orderedLines)
        {
            sb.AppendLine(line.ReportString);
        }

        return sb.ToString();
    }
}