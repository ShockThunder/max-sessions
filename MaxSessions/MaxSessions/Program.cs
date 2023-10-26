namespace MaxSessions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(args[0]);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Please enter path to existing file");
            }
            
            var csvReader = new CsvReader();
            var recordsByDays = csvReader.ReadSessionsDataFromFile(path);

            var sessionsCalculator = new SessionsCalculator();
            var sessions = sessionsCalculator.CalculateSessions(recordsByDays);

            var reportGenerator = new ReportGenerator();
            Console.WriteLine(reportGenerator.GenerateMaxSessionsReport(sessions));
            Console.WriteLine("---------------------------------");
            Console.WriteLine(reportGenerator.GenerateStatesTimeReport(recordsByDays));
            
            Console.ReadKey();
        }
    }
}