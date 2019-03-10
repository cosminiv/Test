
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args) {
            string sourceFile = @"C:\Temp\log.txt";
            string destFile = @"C:\Temp\log.json";
            LogToJson.LogFileToJsonFile(sourceFile, destFile);
        }
    }
}
