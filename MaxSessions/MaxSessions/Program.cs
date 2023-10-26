using System.Diagnostics;

namespace MaxSessions // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var sw = new Stopwatch();
            sw.Start();
            
            var recordsByDays = new List<List<Record>>();

            using(var reader = new StreamReader(@"C:\test_data_onemonth.csv"))
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
            
            var report = new List<(DateTime, string)>();
            Parallel.ForEach(recordsByDays, recordsByDay =>
            {
                var result = CalculateMaxSessionsInDay(recordsByDay);
            
                report.Add((recordsByDay.First().StartDate.Date, $"{recordsByDay.First().StartDate.Date.ToShortDateString()} - {result}"));
            });
            
            
            var orderedReport = report.OrderBy(t => t.Item1);

            foreach (var tuple in orderedReport)
            {
                Console.WriteLine(tuple.Item2);
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            Console.ReadKey();
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
}