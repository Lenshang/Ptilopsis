using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ptilopsis.PtiApplication
{
    public class PtiApp
    {
        /// <summary>
        /// 应用ID (应用名称MD5 固定）
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 默认启动指令
        /// </summary>
        public string DefaultRunCmd { get; set; }
        /// <summary>
        /// 上传的ZIP文件文件路径
        /// </summary>
        public string ZipFile { get; set; }
        /// <summary>
        /// 最后一次启动时间
        /// </summary>
        public DateTime LastRunDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
