using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vttToSrt
{
    class ConsoleOutputHandler
    {
        private static ConsoleOutputHandler _instance;
        private static object _consoleLock = new object();

        public enum OutputMode { normal = 1, quiet = 0, verbose = 2 };
        public enum OutputLevel { error = 0, status = 1, info = 2 };

        public static ConsoleOutputHandler Instance
        {
            get
            {
                _instance = _instance ?? new ConsoleOutputHandler();
                return _instance;
            }
        }
        public OutputMode Mode { get; set; }

        private ConsoleOutputHandler() { }

        private string GetPrefix(OutputLevel level)
        {
            switch (level)
            {
                case OutputLevel.error : return "!";
                case OutputLevel.status : return ":";
                case OutputLevel.info : return "#";
            }
            return string.Empty;
        }

        public void Print(string text, OutputLevel level)
        {
            lock (_consoleLock)
            {
                if ((int)level <= (int)Mode)
                {
                    Console.Write(String.Format("{0} {1}", GetPrefix(level), text));
                }
            }
        }

        public void Println(string text, OutputLevel level)
        {
            Print(String.Format("{0}{1}", text, Environment.NewLine), level);
        }
    }
}
