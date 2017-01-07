using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vttToSrt
{
    public class Converter
    {
        private const string SRTEXTENTION = ".srt";

        public enum OverwriteMode { leave, force };

        public OverwriteMode Mode { get; set; }

        public Converter(OverwriteMode mode)
        {
            Mode = mode;
        }

        public void Convert(FileInfo inputFile, FileInfo outputFile)
        {
            List<SubtitleItem> items;
            if (inputFile == null)
            {
                ConsoleOutputHandler.Instance.Println("No input file?!", ConsoleOutputHandler.OutputLevel.error);
                return;
            }
            ConsoleOutputHandler.Instance.Println("--------------------------------------------", ConsoleOutputHandler.OutputLevel.info);
            ConsoleOutputHandler.Instance.Println(String.Format("Starting conversion of \"{0}\".", inputFile.FullName), ConsoleOutputHandler.OutputLevel.info);
            outputFile = outputFile ?? new FileInfo(String.Format("{0}//{1}{2}", inputFile.Directory.FullName, Path.GetFileNameWithoutExtension(inputFile.Name), SRTEXTENTION));
            ConsoleOutputHandler.Instance.Println(String.Format("Output to \"{0}\".", outputFile.Name), ConsoleOutputHandler.OutputLevel.info);
            if(outputFile.Exists)
            {
                if (Mode == OverwriteMode.leave)
                {
                    ConsoleOutputHandler.Instance.Println("Output file allready exists (use -f to overwrite).", ConsoleOutputHandler.OutputLevel.error);
                    return;
                }
                else
                {
                    ConsoleOutputHandler.Instance.Println("Output file allready exists, overwriting.", ConsoleOutputHandler.OutputLevel.info);
                }
            }
            try
            {
                items = VttParser.Instance.ParseStream(inputFile.OpenRead(), Encoding.UTF8);
            }
            catch(Exception ex)
            {
                ConsoleOutputHandler.Instance.Println(String.Format("Exception while parsing input: {0}", ex), ConsoleOutputHandler.OutputLevel.error);
                return;
            }
            ConsoleOutputHandler.Instance.Println(String.Format("Read {0} items from the input file.", items?.Count), ConsoleOutputHandler.OutputLevel.info);
            if(SrtWriter.Instance.WriteSrt(items, outputFile))
            {
                ConsoleOutputHandler.Instance.Println(String.Format("Succesfully converted {0} to {1}", inputFile.Name, outputFile.Name), ConsoleOutputHandler.OutputLevel.status);
            }
            else
            {
                ConsoleOutputHandler.Instance.Println(String.Format("Failed to output srt to {0}", outputFile.Name), ConsoleOutputHandler.OutputLevel.error);
            }
        }
    }
}
