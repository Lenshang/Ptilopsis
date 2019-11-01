using Ptilopsis.PtiLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PtilopsisServer
{
    public class Config
    {
        public string BindAddress { get; set; } = "http://0.0.0.0:6505";
        private static Config _config;
        private static PtiLogger ConfigLog { get; set; }
        public static Config Get()
        {
            if (Config._config == null)
            {
                string FileName = "Ptilopsis-Server.json";
                ConfigLog = new PtiLogger("config", "pti_server_log");
                var _temp = new Config();
                try
                {
                    var jsonStr = File.ReadAllText(FileName);
                    Config._config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(jsonStr);
                }
                catch (Exception ex)
                {
                    ConfigLog.Warning("Read Config Failure." + ex.ToString());
                    Config._config = new Config();
                    string value = Newtonsoft.Json.JsonConvert.SerializeObject(Config._config);
                    File.WriteAllText(FileName, value);
                }

                #region 检测字段有效性
                #endregion

                #region 检测文件夹是否存在

                #endregion

            }
            return Config._config;
        }
    }
}
