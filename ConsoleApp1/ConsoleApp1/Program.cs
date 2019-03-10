
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args) {
            string sourceFile = @"C:\Cosmin\AnalysisAccessLog\log.txt";
            string destFile = @"C:\Cosmin\AnalysisAccessLog\log.json";
            LogToJson.LogFileToJsonFile(sourceFile, destFile);
        }
    }
}
