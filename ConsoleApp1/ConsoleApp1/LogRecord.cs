using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class LogRecord
    {
        public string host { get; set; }
        public Datetime datetime { get; set; }
        public Request request { get; set; }
        public string response_code { get; set; }
        public int document_size { get; set; }
    }

    class Datetime
    {
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
    }

    class Request
    {
        public string method { get; set; }
        public string url { get; set; }
        public string protocol { get; set; }
        public string protocol_version { get; set; }
    }
}
