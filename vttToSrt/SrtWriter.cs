using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vttToSrt
{
    public class SrtWriter
    {
        private static SrtWriter _instance;

        public static SrtWriter Instance
        {
            get
            {
                _instance = _instance ?? new SrtWriter();
                return _instance;
            }
        }

        private SrtWriter() { }

        public bool WriteSrt(List<SubtitleItem> items, FileInfo outputFile)
        {
            if(items == null || items.Count<1)
            {
                ConsoleOutputHandler.Instance.Println("No items to write (are you shure that was a WebVTT file?)", ConsoleOutputHandler.OutputLevel.status);
                return false;
            }
            try
            {
                if(outputFile.Exists)
                {
                    ConsoleOutputHandler.Instance.Println("Deleting old file", ConsoleOutputHandler.OutputLevel.info);
                    outputFile.Delete();
                }
                using (StreamWriter wrt = new StreamWriter(outputFile.OpenWrite(), Encoding.UTF8))
                {
                    
                    for (int i = 0; i < items.Count; i++)
                    {
                        wrt.Write(String.Format("{0}{1}", i + 1, "\r\n"));
                        wrt.Write(String.Format("{0:hh\\:mm\\:ss\\,fff} --> {1:hh\\:mm\\:ss\\,fff}{2}", new TimeSpan(0, 0, 0, 0, items[i].StartTime), new TimeSpan(0, 0, 0, 0, items[i].EndTime), "\r\n"));
                        foreach(string y in items[i]?.Lines)
                        {
                            wrt.Write(String.Format("{0}{1}", y, "\r\n"));
                        }
                        wrt.Write("\r\n");
                    }
                }
            }
            catch(Exception ex)
            {
                ConsoleOutputHandler.Instance.Println(String.Format("Exception while writing output: {0}", ex), ConsoleOutputHandler.OutputLevel.error);
                return false;
            }
            return true;
        }
    }
}
