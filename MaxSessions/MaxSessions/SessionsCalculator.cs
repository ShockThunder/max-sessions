namespace MaxSessions;

public class SessionsCalculator
{
    public List<ReportLine> CalculateSessions(List<Record> records)
    {
        var report = new List<ReportLine>();
        var recordsByDays = GroupByDays(records);

        Parallel.ForEach(recordsByDays, recordsByDay =>
        {
            var result = CalculateMaxSessionsInDay(recordsByDay.ToList());
            var currentDate = recordsByDay.First().EndDate.Date;
            report.Add(new ReportLine()
            {
                Date = currentDate,
                Line = $"{currentDate.ToShortDateString()} - {result}"
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

    private List<List<Record>> GroupByDays(List<Record> records)
    {
        var result = new List<List<Record>>();

        //parameter to handle possible midnight intersections
        var earliestIntersectedRecordIndex = -1;
        
        var currentDay = records[0].StartDate.Date;
        var currentDayRecords = new List<Record>();
        var maxIndex = records.Count;

        for (int i = 0; i < maxIndex; i++)
        {
            var currentRecord = records[i];

            if (currentRecord.StartDate.Date == currentDay && currentRecord.EndDate.Date == currentDay)
                currentDayRecords.Add(currentRecord);
            
            else if (currentRecord.StartDate.Date == currentDay && currentRecord.EndDate.Date != currentDay)
            {
                if(earliestIntersectedRecordIndex == -1)
                    earliestIntersectedRecordIndex = i;
                
                currentDayRecords.Add(currentRecord);
            }
            else
            {
                result.Add(currentDayRecords);
                currentDayRecords = new List<Record>();
                
                if (earliestIntersectedRecordIndex != -1)
                {
                    i = earliestIntersectedRecordIndex;
                    earliestIntersectedRecordIndex = -1;
                }
                else
                    currentDayRecords.Add(currentRecord);

                currentDay = currentRecord.StartDate.Date;
            }

            if (i == maxIndex - 1) 
                result.Add(currentDayRecords);
        }

        return result;
    }
}