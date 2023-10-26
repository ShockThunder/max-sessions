using System.Diagnostics;
using System.Text;

namespace MaxSessions // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var sw = new Stopwatch();
            sw.Start();

            var recordsByDays = ReadDataFromFile(@"C:\test_data_onemonth.csv");

            var report = CalculateSessions(recordsByDays);
            
            var orderedReport = report.OrderBy(rl => rl.Date).ToList();
            
            Console.WriteLine(BuildReport(orderedReport));

            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            Console.ReadKey();
        }

        private static List<List<Record>> ReadDataFromFile(string path)
        {
            var recordsByDays = new List<List<Record>>();
            
            using(var reader = new StreamReader(path))
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
                    Duration = firstValues[5]
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
                        Duration = values[5]
                    };
                    
                    if(record.StartDate.Date == currentDate)
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
        
        
        private static List<ReportLine> CalculateSessions(List<List<Record>> recordsByDays)
        {
            var report = new List<ReportLine>();

            Parallel.ForEach(recordsByDays, recordsByDay =>
            {
                var result = CalculateMaxSessionsInDay(recordsByDay);
            
                report.Add(new ReportLine()
                {
                    Date = recordsByDay.First().StartDate.Date,
                    ReportString = $"{recordsByDay.First().StartDate.Date.ToShortDateString()} - {result}"
                });
            });

            return report;
        }
        private static string BuildReport(List<ReportLine> lines)
        {
            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                sb.AppendLine(line.ReportString);
            }

            return sb.ToString();
        }

        // подумать, как захватывать первую запись предыдущего дня.
        private static int CalculateMaxSessionsInDay(List<Record> recordsFirstDay)
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

    public class Record
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Project { get; set; }
        public string Operator { get; set; }
        public string State { get; set; }
        public string Duration { get; set; }
    }

    public class ReportLine
    {
        public DateTime Date { get; set; }
        public string ReportString { get; set; }
    }
}