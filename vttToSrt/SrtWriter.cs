using System;
using System.Collections.Generic;
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

        public bool WriteSrt()
        {
            // TODO: implement
            return false;
        }
    }
}
