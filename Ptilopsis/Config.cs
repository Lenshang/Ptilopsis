using Ptilopsis.PtiLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptilopsis
{
    class Config
    {
        public string AppZipPath { get; protected set; } = "pti_app_zip";
        public string AppRunPath { get; protected set; } = "pti_app_run";
        public int CheckTasksLoopIntervalSeconds { get; protected set; } = 10;
        public int SyncDatabaseIntervalSeconds { get; protected set; } = 60;
        private static Config _config;
        private static PtiLogger ConfigLog { get; set; }
        public static Config Get()
        {
            if (Config._config == null)
            {
                ConfigLog = new PtiLogger("config");
                var _temp = new Config();
                try
                {
                    var jsonStr = File.ReadAllText("Ptilopsis-Core.json");
                    Config._config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(jsonStr);
                }
                catch(Exception ex)
                {
                    ConfigLog.Warning("Read Config Failure."+ex.ToString());
                    Config._config = new Config();
                    string value = Newtonsoft.Json.JsonConvert.SerializeObject(Config._config);
                    File.WriteAllText("Ptilopsis-Core.json", value);
                }

                #region 检测字段有效性
                if (string.IsNullOrWhiteSpace(Config._config.AppRunPath))
                {
                    Config._config.AppRunPath = _temp.AppRunPath;
                }
                if (string.IsNullOrWhiteSpace(Config._config.AppZipPath))
                {
                    Config._config.AppZipPath = _temp.AppZipPath;
                }
                if (Config._config.CheckTasksLoopIntervalSeconds <= 0)
                {
                    Config._config.CheckTasksLoopIntervalSeconds = 10;
                }
                if (Config._config.SyncDatabaseIntervalSeconds <= 0)
                {
                    Config._config.SyncDatabaseIntervalSeconds = 60;
                }
                #endregion

                #region 检测文件夹是否存在
                if (!Directory.Exists(Config._config.AppRunPath))
                {
                    Directory.CreateDirectory(Config._config.AppRunPath);
                }

                if (!Directory.Exists(Config._config.AppZipPath))
                {
                    Directory.CreateDirectory(Config._config.AppZipPath);
                }
                #endregion

            }
            return Config._config;
        }
    }
}
