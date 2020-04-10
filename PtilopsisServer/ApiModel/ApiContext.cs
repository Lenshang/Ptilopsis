using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PtilopsisServer.ApiModel
{
    public class ApiContext
    {
        public bool success { get; set; }
        public object data { get; set; }
        public DateTime dateTime { get; set; }
        public string message { get; set; }
    }
}
