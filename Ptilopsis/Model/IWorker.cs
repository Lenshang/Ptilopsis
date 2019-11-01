using Ptilopsis.PtiLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ptilopsis.Model
{
    public enum WorkerState
    {
        INIT,
        RUNNING,
        STOP,
        ERROR
    }
    public abstract class IWorker
    {
        public WorkerState State { get; set; }
        public PtiLogger Logs { get; set; }
        public String WorkerName { get; set; }
        public Action StartAction { get; set; }
        public Action StopAction { get; set; }
        public IWorker()
        {
            this.State  = WorkerState.INIT;
            this.WorkerName = this.GetType().Name;
            this.Logs = new PtiLogger(this.WorkerName,Path.Combine("pti_sys_log", this.WorkerName));
        }
        public virtual bool Start()
        {
            if (this.State != WorkerState.RUNNING)
            {
                this.State = WorkerState.RUNNING;
                this.StartAction?.Invoke();
                Logs.Info(this.WorkerName + " Create!");
                return true;
            }
            return false;
        }
        public void Stop()
        {
            if (this.State == WorkerState.RUNNING)
            {
                this.State = WorkerState.STOP;
                this.StopAction?.Invoke();
                Logs.Info(this.WorkerName + " Stop!");
            }
        }

        public void WriteInfo(string msg)
        {
            Logs.Info($"#{this.WorkerName}# {msg}");
        }
        public void WriteInfo(string msg,string tag)
        {
            Logs.Info($"#{tag}# {msg}");
        }
        public void WriteError(string msg)
        {
            Logs.Error($"#{this.WorkerName}# {msg}");
        }
        public void WriteError(string msg,string tag)
        {
            Logs.Error($"#{tag}# {msg}");
        }
        public void WriteWarning(string msg)
        {
            Logs.Warning($"#{this.WorkerName}# {msg}");
        }
        public void WriteWarning(string msg,string tag)
        {
            Logs.Warning($"#{tag}# {msg}");
        }
    }
}
