namespace MaxSessions;

public class SessionsCalculator
{
    public List<ReportLine> CalculateSessions(List<List<Record>> recordsByDays)
    {
        var report = new List<ReportLine>();

        Parallel.ForEach(recordsByDays, recordsByDay =>
        {
            var result = CalculateMaxSessionsInDay(recordsByDay);

            report.Add(new ReportLine()
            {
                Date = recordsByDay.First().StartDate.Date,
                Line = $"{recordsByDay.First().StartDate.Date.ToShortDateString()} - {result}"
            });
        });

        return report;
    }
    
    private int CalculateMaxSessionsInDay(List<Record> recordsFirstDay)
    {
        var maxSessions = 1;

        var maxIndex = recordsFirstDay.Count();

        for (int i = 0; i < maxIndex; i++)
        {
            var curMax = 1;
            var currentRecord = recordsFirstDay[i];
            var searchFromIndex = i + 1;

            for (int j = searchFromIndex; j < maxIndex; j++)
            {
                var newRec = recordsFirstDay[j];
                if (newRec.StartDate < currentRecord.EndDate)
                    curMax++;
                else
                    break;
            }

            if (curMax > maxSessions)
                maxSessions = curMax;
        }

        return maxSessions;
    }
}