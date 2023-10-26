using System.Text;

namespace MaxSessions;

public class ReportGenerator
{
    public string GenerateMaxSessionsReport(List<ReportLine> lines)
    {
        var orderedLines = lines.OrderBy(rl => rl.Date);
        var sb = new StringBuilder();
        foreach (var line in orderedLines)
        {
            sb.AppendLine(line.Line);
        }

        return sb.ToString();
    }

    public string GenerateStatesTimeReport(List<List<Record>> records)
    {
        var recordsForMonth = records.SelectMany(x => x).ToList();
        var groupedByOperator = recordsForMonth.GroupBy(x => x.Operator);
        var sb = new StringBuilder();
        foreach (var group in groupedByOperator)
        {
            var operatorName = group.Key;
            var groupedByStates = group.GroupBy(x => x.State);
            var reportLine = operatorName;
            foreach (var groupedByState in groupedByStates)
            {
                reportLine += $" {groupedByState.Key} - {groupedByState.Sum(x => x.Duration)}";
            }

            sb.AppendLine(reportLine);
        }

        return sb.ToString();
    }
}