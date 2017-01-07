using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace vttToSrt
{
    class Program
    {
        private const string DEFAULTSEARCHEXTENTION = ".vtt";
        private static List<ConsoleParameter> _parameterConfiguration = new List<ConsoleParameter>()
        {
            new ConsoleParameter("force", false, "-f", "--force", "--plz"),
            new ConsoleParameter("search", true, "-s", "--search"),
            new ConsoleParameter("extention", true, "-e", "--extention"),
            new ConsoleParameter("recurse", false, "-r", "--recurse"),
            new ConsoleParameter("verbose", false, "-v", "--verbose"),
            new ConsoleParameter("quiet", false, "-q", "--quiet"),
            new ConsoleParameter("help", false, "-h", "--help", "--haaalp"),
        };

        private static Dictionary<string, string> _consoleParameters = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            if (ParseCmdParams(args))
            {
                new Program();
            }
            ConsoleOutputHandler.Instance.Println("----- DONE -----", ConsoleOutputHandler.OutputLevel.status);
            #if DEBUG
            Console.ReadLine();
            #endif
        }

        private Program()
        {
            Converter converter = new Converter(_consoleParameters.ContainsKey("force") ? Converter.OverwriteMode.force : Converter.OverwriteMode.leave);
            bool recurse = _consoleParameters.ContainsKey("recurse");
            if (_consoleParameters.ContainsKey("search"))
            {
                string searchpattern = String.Format("*.{0}", (_consoleParameters.ContainsKey("extention") ? _consoleParameters["extention"] : DEFAULTSEARCHEXTENTION).Replace("*", string.Empty).TrimStart('.'));
                DirectoryInfo searchdir = new DirectoryInfo(_consoleParameters["search"]);
                if(!searchdir.Exists)
                {
                    ConsoleOutputHandler.Instance.Println(String.Format("Search directory does not exist: \"{0}\".", searchdir.FullName), ConsoleOutputHandler.OutputLevel.error);
                    return;
                }
                ConsoleOutputHandler.Instance.Println(String.Format("Searching {0}in \"{1}\" for files matching \"{2}\".", recurse ? "recurively " : String.Empty, searchdir.FullName, searchpattern), ConsoleOutputHandler.OutputLevel.info);
                List<FileInfo> files = searchdir.GetFiles(searchpattern, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();                
                ConsoleOutputHandler.Instance.Println(String.Format("Found: {0} matching files.", files?.Count), ConsoleOutputHandler.OutputLevel.info);
                if(files.Count<1)
                {
                    return;
                }
                for (int i = 0; i < files.Count; i++)
                {                    
                    converter.Convert(files[i], null);
                    ConsoleOutputHandler.Instance.Println(String.Format("Status:{0}/{1}", i + 1, files.Count, files?.Count), ConsoleOutputHandler.OutputLevel.info);
                }
            }
            else
            {
                if(_consoleParameters.ContainsKey("inPath"))
                {
                    converter.Convert(new FileInfo(_consoleParameters["inPath"]), _consoleParameters.ContainsKey("outPath") ? new FileInfo(_consoleParameters["outPath"]) : null);
                }
                else
                {
                    ConsoleOutputHandler.Instance.Println("No source Path (-h for help).", ConsoleOutputHandler.OutputLevel.error);
                }
            }
        }

        private static bool ParseCmdParams(string[] args)
        {            
            for (int i = 0; i < args.Length; i++)
            {
                string current = args[i].ToLower().Trim();
                ConsoleParameter match = _parameterConfiguration.FirstOrDefault(x => x.Aliases.Any(y => y == current));
                if(match != null)
                {
                    if(match.HasValue)
                    {
                        if(args.Length> i + 1)
                        {
                            i++;
                            _consoleParameters.Add(match.Name, args[i]);
                        }
                        else
                        {
                            Console.WriteLine(String.Format("! Missing value for parameter {0}", args[i]));
                            return false;
                        }
                    }
                    else
                    {
                        _consoleParameters[match.Name] = null;
                    }
                }
                else
                {
                    if(!_consoleParameters.ContainsKey("inPath"))
                    {
                        _consoleParameters.Add("inPath", args[i]);
                    }
                    else
                    {
                        _consoleParameters["outPath"] = args[i];
                    }
                }
            }
            if(_consoleParameters.Count <1 || _consoleParameters.ContainsKey("help"))
            {
                PrintHelp();
                return false; // No error but not ok to continue
            }
            ConsoleOutputHandler.Instance.Mode = _consoleParameters.ContainsKey("quiet") ? ConsoleOutputHandler.OutputMode.quiet : _consoleParameters.ContainsKey("verbose") ? ConsoleOutputHandler.OutputMode.verbose : ConsoleOutputHandler.OutputMode.normal;
            return true;
        }

        private static void PrintHelp()
        {
            try
            {
                Console.Write(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("vttToSrt.halp.txt")).ReadToEnd());
            }
            catch
            {
                ConsoleOutputHandler.Instance.Println("Help is broken.", ConsoleOutputHandler.OutputLevel.error);
            }
        }
    }
}
