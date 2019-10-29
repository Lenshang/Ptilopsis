using Ptilopsis.PtiApplication;
using Ptilopsis.PtiRun;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ptilopsis.PtiTask
{
    public class PtiRunTask
    {
        public PtiTasker PtiTasker { get; set; }
        public PtiApp PtiApp { get; set; }
        public PtiRunner Runner { get; set; }
        public DateTime NextRunDate { get; set; }
    }
}
