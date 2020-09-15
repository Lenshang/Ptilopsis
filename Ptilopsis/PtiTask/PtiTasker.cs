using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ptilopsis.PtiTask
{
    public enum TaskState
    {
        INIT,
        RUNNING,
        STOP
    }
    public class PtiTasker
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string _id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        //public string Id { get; set; }
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
        /// 超时时间
        /// </summary>
        public long TimeOutSeconds { get; set; } = 0;
        /// <summary>
        /// 是否允许多实例运行
        /// </summary>
        public bool MultiRunner { get; set; } = false;
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool Enable { get; set; } = true;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 上次运行时间
        /// </summary>
        public DateTime LastRunDate { get; set; }
        /// <summary>
        /// 下次运行时间
        /// </summary>
        public DateTime NextRunDate { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskState TaskState { get; set; } = TaskState.INIT;
        /// <summary>
        /// 最后一次日志文件
        /// </summary>
        public string LastLogName { get; set; }
        public void Update(PtiTasker tasker)
        {
            this.RunArgs = tasker.RunArgs;
            this.RunPath = tasker.RunPath;
            this.Schedule = tasker.Schedule;
            if (!string.IsNullOrWhiteSpace(tasker.ApplicationId))
            {
                this.ApplicationId = tasker.ApplicationId;
            }
            if (!string.IsNullOrWhiteSpace(tasker.RunCmd))
            {
                this.RunCmd = tasker.RunCmd;
            }
            if (!string.IsNullOrWhiteSpace(tasker.Schedule))
            {
                
            }
            if (!string.IsNullOrWhiteSpace(tasker.TaskName))
            {
                this.TaskName = tasker.TaskName;
            }
        }

        public string GetRunCmd()
        {
            string[] runCmds = this.RunCmd.Split(" ");
            if (runCmds.Length > 0)
            {
                return runCmds[0];
            }
            return this.RunCmd;
        }

        public string GetRunArgs()
        {
            List<string> runCmds = this.RunCmd.Split(" ").ToList();

            List<string> result = new List<string>();
            if (runCmds.Count > 0)
            {
                runCmds = runCmds.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                for(int i = 1; i < runCmds.Count; i++)
                {
                    result.Add(runCmds[i]);
                }
            }

            if (!string.IsNullOrWhiteSpace(this.RunArgs))
            {
                List<string> args = this.RunArgs.Split(" ").ToList();
                args = args.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                result.AddRange(args);
            }
            
            return string.Join(" ", result);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
