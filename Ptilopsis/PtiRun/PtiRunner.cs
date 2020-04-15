using Ptilopsis.PtiApplication;
using Ptilopsis.Model;
using Ptilopsis.PtiLog;
using Ptilopsis.PtiTask;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Ptilopsis.PtiEvent;

namespace Ptilopsis.PtiRun
{
    public enum ProcessState
    {
        INIT,
        RUNNING,
        STOP,
        KILLING,
        KILLED
    }
    public class PtiRunner:IDisposable
    {
        public string Id { get; set; }
        /// <summary>
        /// 任务信息对象
        /// </summary>
        public PtiTasker TaskInfo { get; set; }
        /// <summary>
        /// 应用信息对象
        /// </summary>
        public PtiApp AppInfo { get; set; }
        /// <summary>
        /// 实时消息对象
        /// </summary>
        public PtiLogger Logger { get; set; }
        /// <summary>
        /// Runner的状态
        /// </summary>
        public ProcessState State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 系统Process对象
        /// </summary>
        private Process Process { get; set; }
        /// <summary>
        /// 系统ProcessInfo 对象
        /// </summary>
        private ProcessStartInfo ProcessInfo { get; set; }
        private List<Action<string>> MessagePipelines { get; set; }
        private List<Action<string>> ErrMessagePipelines { get; set; }
        public PtiRunner(PtiTasker task, PtiApp app)
        {
            this.Id = Guid.NewGuid().ToString();
            this.TaskInfo = task;
            this.AppInfo = app;
            this.Logger = new PtiLogger(task._id,fileFormat:"yyyyMMdd_mmHHss");
            this.MessagePipelines = new List<Action<string>>();
            this.ErrMessagePipelines = new List<Action<string>>();
            this.CreateDate = DateTime.Now;
            this.CheckRunner();

            this.ProcessInfo = new ProcessStartInfo(task.RunCmd, task.RunArgs);
            ProcessInfo.CreateNoWindow = true;   //不创建窗口
            ProcessInfo.UseShellExecute = false;//不使用系统外壳程序启动,重定向输出的话必须设为false
            ProcessInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台上
            ProcessInfo.RedirectStandardError = true;
            ProcessInfo.WorkingDirectory = Path.GetFullPath(task.RunPath);
            this.State = ProcessState.INIT;
        }
        /// <summary>
        /// 启动程序
        /// </summary>
        public void Run()
        {
            if (this.State != ProcessState.RUNNING)
            {
                try
                {
                    this.Logger.Info("{Ptilopsis_Runner}Runner Start");
                    this.State = ProcessState.RUNNING;
                    this.Process = Process.Start(this.ProcessInfo);
                    this.Process.OutputDataReceived += Process_OutputDataReceived;
                    this.Process.ErrorDataReceived += Process_ErrorDataReceived;
                    this.Process.Exited += Process_Exited;
                    this.Process.EnableRaisingEvents = true;
                    this.Process.BeginOutputReadLine();
                    this.Process.BeginErrorReadLine();
                }
                catch(Exception e)
                {
                    this.Logger.Error(e.ToString());
                    this.Logger.Info("{Ptilopsis_Runner}Runner Error End");
                    this.State = ProcessState.STOP;
                    EventManager.Get().RegEvent(ptiEvent => {
                        RunnerManager.Get().RemoveRunner(this);
                        this.Dispose();
                        return null;
                    });
                }
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            this.Logger.Info("{Ptilopsis_Runner}Runner End");
            if (this.State == ProcessState.KILLING)
            {
                this.State = ProcessState.KILLED;
            }
            else
            {
                this.State = ProcessState.STOP;
            }
            EventManager.Get().RegEvent(ptiEvent => {
                RunnerManager.Get().RemoveRunner(this);
                this.Dispose();
                return null;
            });
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Logger.Error(e.Data);
        }
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Logger.Info(e.Data);
        }

        /// <summary>
        /// 检查合法性
        /// </summary>
        private void CheckRunner()
        {
            if (string.IsNullOrWhiteSpace(this.TaskInfo.RunCmd))
            {
                throw new Exception("运行指令为空");
            }

            else if (string.IsNullOrWhiteSpace(this.TaskInfo.RunPath) && this.AppInfo == null)
            {
                throw new Exception("RunPath和Application不能同时为空");
            }
        }
        /// <summary>
        /// 杀死当前任务
        /// </summary>
        public void KillAsync()
        {
            if (this.State == ProcessState.RUNNING)
            {
                this.Process.Kill();
                this.State = ProcessState.KILLING;
            }
        }
        /// <summary>
        /// 等待任务退出
        /// </summary>
        public void Wait()
        {
            this.Process.WaitForExit();
        }

        public void Dispose()
        {
            try
            {
                if (this.State == ProcessState.RUNNING)
                {
                    this.KillAsync();
                }
                this.Logger.Dispose();
                this.Process.Dispose();
            }
            catch
            {

            }
        }
    }
}
