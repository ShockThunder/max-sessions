namespace MaxSessions // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var records = new List<Record>();

            using(var reader = new StreamReader(@"C:\test_data_onemonth.csv"))
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
                        Duration = values[5]
                    };
                    records.Add(record);
                }
            }
            
            var allDaysChecked = false;

            var currentDay = records.First();
            
            while (!allDaysChecked)
            {
                if (currentDay is null)
                {
                    allDaysChecked = true;
                    continue;
                }

                var recordsNextDay = records.Where(x => x.StartDate.Date == currentDay.StartDate.Date).ToList();
                var resultNextDay = CalculateMaxSessionsInDay(recordsNextDay);

                Console.WriteLine($"{currentDay.StartDate.Date.ToShortDateString()} - {resultNextDay}");
                
                currentDay = records.FirstOrDefault(x => x.StartDate.Date > currentDay.StartDate.Date);
            }

            Console.ReadKey();
        }

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