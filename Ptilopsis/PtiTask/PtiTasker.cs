using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ptilopsis.PtiTask
{
    public class PtiTasker
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 启动命令
        /// </summary>
        public string RunCmd { get; set; }
        /// <summary>
        /// 启动参数
        /// </summary>
        public string RunArgs { get; set; }
        /// <summary>
        /// AppId
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// 运行目录
        /// </summary>
        public string RunPath { get; set; }
        /// <summary>
        /// 任务计划
        /// </summary>
        public string Schedule { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 上次运行时间
        /// </summary>
        public DateTime LastRunDate { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
