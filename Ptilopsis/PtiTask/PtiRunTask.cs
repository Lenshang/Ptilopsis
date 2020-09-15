using Ptilopsis.PtiApplication;
using Ptilopsis.PtiLog;
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
        public PtiLogger Logger { get; set; }
        public DateTime LastRunDate { get; set; }
        private DateTime _NextRunDate { get; set; }
        public DateTime NextRunDate
        {
            get
            {
                return _NextRunDate;
            }
            set
            {
                this._NextRunDate = value;
                this.PtiTasker.NextRunDate = value;
            }
        }
    }
}
