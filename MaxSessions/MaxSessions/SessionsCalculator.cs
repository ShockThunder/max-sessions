namespace MaxSessions;

public class SessionsCalculator
{
    public List<ReportLine> CalculateSessions(List<Record> records)
    {
        var report = new List<ReportLine>();
        var recordsByDays = records.GroupBy(x => x.DayOfSession);

        Parallel.ForEach(recordsByDays, recordsByDay =>
        {
            var result = CalculateMaxSessionsInDay(recordsByDay.ToList());

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
            var startTimeBoundary = currentRecord.StartDate;
            var endTimeBoundary = currentRecord.EndDate;
            var searchFromIndex = i + 1;

            for (int j = searchFromIndex; j < maxIndex; j++)
            {
                var newRec = recordsFirstDay[j];
                if (IsIntersect(startTimeBoundary, endTimeBoundary, newRec))
                    curMax++;
                else
                    break;
                
                if (newRec.StartDate > startTimeBoundary)
                    startTimeBoundary = newRec.StartDate;
                if (newRec.EndDate < endTimeBoundary)
                    endTimeBoundary = newRec.EndDate;
            }

            if (curMax > maxSessions)
                maxSessions = curMax;
        }

        return maxSessions;
    }

    private bool IsIntersect(DateTime startBoundary, DateTime endBoundary, Record checkedRecord)
    {
        if (checkedRecord.StartDate < startBoundary && checkedRecord.EndDate > endBoundary)
            return true;
        if (checkedRecord.StartDate < startBoundary && checkedRecord.EndDate > startBoundary)
            return true;
        if (checkedRecord.StartDate < endBoundary && checkedRecord.EndDate > endBoundary)
            return true;
        if (checkedRecord.StartDate > startBoundary && checkedRecord.EndDate < endBoundary)
            return true;

        return false;
    }
}