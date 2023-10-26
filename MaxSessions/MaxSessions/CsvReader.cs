namespace MaxSessions;

public class CsvReader
{
    public List<List<Record>> ReadSessionsDataFromFile(string path)
    {
        var recordsByDays = new List<List<Record>>();

        using (var reader = new StreamReader(path))
        {
            var headers = reader.ReadLine();

            var firstLine = reader.ReadLine();
            var firstValues = firstLine.Split(';');

            var firstRecord = new Record()
            {
                StartDate = DateTime.Parse(firstValues[0]),
                EndDate = DateTime.Parse(firstValues[1]),
                Project = firstValues[2],
                Operator = firstValues[3],
                State = firstValues[4],
                Duration = int.Parse(firstValues[5])
            };

            var currentDayRecords = new List<Record>();
            currentDayRecords.Add(firstRecord);

            var currentDate = firstRecord.StartDate.Date;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                var record = new Record()
                {
                    StartDate = DateTime.Parse(values[0]),
                    EndDate = DateTime.Parse(values[1]),
                    Project = values[2],
                    Operator = values[3],
                    State = values[4],
                    Duration = int.Parse(values[5])
                };

                if (record.StartDate.Date == currentDate)
                    currentDayRecords.Add(record);
                else
                {
                    recordsByDays.Add(currentDayRecords);
                    currentDayRecords = new List<Record>();
                    currentDayRecords.Add(record);
                    currentDate = record.StartDate.Date;
                }
            }

            recordsByDays.Add(currentDayRecords);
        }

        return recordsByDays;
    }
}