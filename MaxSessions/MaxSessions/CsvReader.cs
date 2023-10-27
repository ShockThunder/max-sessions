namespace MaxSessions;

public class CsvReader
{
    public List<Record> ReadSessionsDataFromFile(string path)
    {
        var recordsByDays = new List<List<Record>>();
        var records = new List<Record>();

        using (var reader = new StreamReader(path))
        {
            var headers = reader.ReadLine();
            
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
                    Duration = int.Parse(values[5]),
                    DayOfSession = DateTime.Parse(values[0]).Date
                };

                records.Add(record);
            }
        }
        return records.OrderBy(x => x.StartDate).ToList();
    }
}