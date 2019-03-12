using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class LogToJson
    {
        static Regex _uncommonCharsRegex = new Regex("[\x00-\x1F]{1}");  // http://www.asciitable.com/
        static int _currentLineIndex;

        public static void LogFileToJsonFile(string sourceFile, string destFile) {
            List<LogRecord> logRecords = ParseLogFile(sourceFile);
            SerializeLogRecords(logRecords, destFile);
        }

        private static List<LogRecord> ParseLogFile(string sourceFile) {
            string[] lines = File.ReadAllLines(sourceFile);
            List<LogRecord> logRecords = new List<LogRecord>(lines.Length);
            _currentLineIndex = 1;

            foreach (string line in lines) {
                string cleanLine = _uncommonCharsRegex.Replace(line, "");
                LogRecord logRecord = ParseLogLine(cleanLine);
                logRecords.Add(logRecord);
                _currentLineIndex++;
            }

            return logRecords;
        }


        private static void SerializeLogRecords(List<LogRecord> logRecords, string destFile) {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(destFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
                serializer.Serialize(writer, logRecords);
        }

        private static LogRecord ParseLogLine(string line) {
            LogRecord record = new LogRecord();

            int spaceIndex1 = line.IndexOf(' ');
            int spaceIndex2 = line.IndexOf(' ', spaceIndex1 + 1);
            int lastQuotesIndex = line.LastIndexOf('"');
            int lastSpaceIndex = line.IndexOf(' ', lastQuotesIndex + 2);

            record.host = line.Substring(0, spaceIndex1);

            string datetimeStr = line.Substring(spaceIndex1 + 1, spaceIndex2 - spaceIndex1 - 1);
            record.datetime = ParseDatetime(datetimeStr);

            string requestStr = line.Substring(spaceIndex2 + 2, lastQuotesIndex - spaceIndex2 - 2);
            record.request = ParseRequest(requestStr);

            record.response_code = line.Substring(lastQuotesIndex + 2, lastSpaceIndex - lastQuotesIndex - 2);
            string docSizeStr = line.Substring(lastSpaceIndex + 1);
            if (int.TryParse(docSizeStr, out int docSize))
                record.document_size = docSize;

            return record;
        }

        private static Request ParseRequest(string requestStr) {
            requestStr = requestStr.Trim('"');
            bool isValid = requestStr.StartsWith("GET") || requestStr.StartsWith("POST") || requestStr.StartsWith("HEAD");
            if (!isValid)
                return null;

            int firstSpaceIndex = requestStr.IndexOf(' ');
            int lastSpaceIndex = requestStr.LastIndexOf(' ');
            string protocolAndVersion = requestStr.Substring(lastSpaceIndex + 1);
            bool isValidProtocol = protocolAndVersion.StartsWith("HTTP");
            string[] protocolTokens = isValidProtocol ? protocolAndVersion.Split('/') : null;

            Request req = new Request();
            req.method = requestStr.Substring(0, firstSpaceIndex);

            if (isValidProtocol)
                req.url = requestStr.Substring(firstSpaceIndex + 1, lastSpaceIndex - firstSpaceIndex - 1);
            else
                req.url = requestStr.Substring(firstSpaceIndex + 1);

            req.url = req.url.Replace("\"", "");

            if (isValidProtocol) {
                req.protocol = protocolTokens[0];
                req.protocol_version = protocolTokens[1];
            }

            return req;
        }

        private static Datetime ParseDatetime(string datetimeStr) {
            datetimeStr = datetimeStr.Trim('[', ']');
            string[] tokens = datetimeStr.Split(':');

            Datetime dt = new Datetime();
            dt.day = int.Parse(tokens[0]);
            dt.hour = int.Parse(tokens[1]);
            dt.minute = int.Parse(tokens[2]);
            dt.second = int.Parse(tokens[3]);

            return dt;
        }
    }
}
