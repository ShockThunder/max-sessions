using System.Diagnostics;
using System.Reflection;
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
            Console.WriteLine(reportGenerator.GenerateReport(sessions));

            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            Console.ReadKey();
        }

        
    }
}