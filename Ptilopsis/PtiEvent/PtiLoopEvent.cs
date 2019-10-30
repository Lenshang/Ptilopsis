using System;
using System.Collections.Generic;
using System.Text;

namespace Ptilopsis.PtiEvent
{
    public class PtiLoopEvent
    {
        public TimeSpan Interval { get; set; }
        public PtiEventer Action { get; set; }
        public DateTime NextRunDate { get; set; }
    }
}
