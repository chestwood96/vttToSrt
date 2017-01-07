using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vttToSrt
{
    public class ConsoleParameter
    {
        public string Name { get; set; }
        public List<string> Aliases { get; set; }
        public bool HasValue { get; set; }

        public ConsoleParameter(string name, bool hasValue, params string[] aliases)
        {
            Name = name;
            HasValue = hasValue;
            Aliases = aliases.ToList();
        }
    }
}
