using System.Text;

namespace MaxSessions;

public class ReportGenerator
{
    public string GenerateMaxSessionsReport(List<ReportLine> lines)
    {
        lines.Sort((a,b) => a.Date.CompareTo(b.Date));
        var sb = new StringBuilder();
        foreach (var line in lines) 
            sb.AppendLine(line.Line);

        return sb.ToString();
    }

    public string GenerateStatesTimeReport(List<Record> records)
    {
        records.Sort((a,b) => String.Compare(a.Operator, b.Operator, StringComparison.Ordinal));
        
        var currentOperator = records[0].Operator;
        var currentOperatorStates = new Dictionary<string, int>();
        var sb = new StringBuilder();
        string reportLine;
        
        for (int i = 0; i < records.Count; i++)
        {
            var currentRecord = records[i];
            if (currentRecord.Operator == currentOperator)
            {
                if (currentOperatorStates.ContainsKey(currentRecord.State))
                    currentOperatorStates[currentRecord.State] += currentRecord.Duration;
                else
                    currentOperatorStates[currentRecord.State] = currentRecord.Duration;
            }
            else
            {
                reportLine = currentOperator;
                foreach (var operatorState in currentOperatorStates)
                    reportLine += $" {operatorState.Key} - {operatorState.Value}";

                sb.AppendLine(reportLine);
                
                currentOperator = currentRecord.Operator;
                currentOperatorStates = new Dictionary<string, int>();
                currentOperatorStates[currentRecord.State] = currentRecord.Duration;
            }
        }
        
        //fill last operator
        reportLine = currentOperator;
        foreach (var operatorState in currentOperatorStates)
            reportLine += $" {operatorState.Key} - {operatorState.Value}";

        sb.AppendLine(reportLine);

        return sb.ToString();
    }
}