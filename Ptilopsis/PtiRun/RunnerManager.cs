using Ptilopsis.Model;
using Ptilopsis.PtiTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ptilopsis.PtiRun
{
    public class RunnerManager : IWorker
    {
        List<PtiRunner> RunnerList { get; set; }
        Object locker { get; set; } = new object();
        public RunnerManager()
        {
            this.RunnerList = new List<PtiRunner>();
        }
        public bool CreateRunner(PtiRunTask runTask)
        {
            if (string.IsNullOrWhiteSpace(runTask.PtiTasker.RunCmd))
            {
                runTask.PtiTasker.RunCmd = runTask.PtiApp.DefaultRunCmd;
            }
            PtiRunner runner = new PtiRunner(runTask.PtiTasker, runTask.PtiApp);
            this.RunnerList.Add(runner);
            runTask.Runner = runner;
            return true;
        }
        public bool CreateAndStart(PtiRunTask runTask)
        {
            var _old = this.RunnerList.Where(r => r.TaskInfo._id == runTask.PtiTasker._id).FirstOrDefault();
            if (!runTask.PtiTasker.MultiRunner && _old != null && _old?.State == ProcessState.RUNNING)
            {
                return true;
            }
            this.CreateRunner(runTask);
            runTask.LastRunDate = DateTime.Now;
            runTask.PtiTasker.LastRunDate = runTask.LastRunDate;
            runTask.PtiTasker.TaskState = TaskState.RUNNING;
            runTask.PtiApp.LastRunDate = runTask.PtiTasker.LastRunDate;
            runTask.Logger = runTask.Runner.Logger;
            runTask.Runner.Run();
            return true;
        }

        public bool CheckTaskAndKill(PtiTasker tasker)
        {
            var _old = this.RunnerList.Where(r => r.TaskInfo._id == tasker._id).FirstOrDefault();
            if (_old != null && _old?.State == ProcessState.RUNNING)
            {
                try
                {
                    _old.KillAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    this.WriteError(ex.ToString());
                    return false;
                }
            }
            return true;
        }
        public bool TaskKillById(string id)
        {
            var _old = this.RunnerList.Where(r => r.TaskInfo._id == id).FirstOrDefault();
            if (_old != null && _old?.State == ProcessState.RUNNING)
            {
                try
                {
                    _old.KillAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    this.WriteError(ex.ToString());
                    return false;
                }
            }
            return true;
        }

        public bool RemoveRunner(PtiRunner runner)
        {
            try
            {
                runner.TaskInfo.TaskState = TaskState.STOP;
                lock (locker)
                {
                    this.RunnerList.Remove(runner);
                }
                
                //TaskManager.Get().RemoveTaskById(runner.TaskInfo._id);
            }
            catch (Exception ex)
            {
                this.WriteError(ex.ToString());
                return false;
            }
            return true;
        }

        public bool TaskExist(PtiRunner runner)
        {
            lock (locker)
            {
                return this.RunnerList.Contains(runner);
            }
        }
        #region 单例模式
        private static RunnerManager _runnerManager = null;
        public static RunnerManager Get()
        {
            if (_runnerManager == null)
            {
                _runnerManager = new RunnerManager();
            }
            return _runnerManager;
        }
        #endregion
    }
}
