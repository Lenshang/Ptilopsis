using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ptilopsis.Model
{
    public enum LogLevel
    {
        INFO = 1,
        WARNING = 2,
        ERROR = 3
    }
    public class LogModel
    {
        public string _id { get; set; }
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
