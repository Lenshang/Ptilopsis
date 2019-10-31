using Ptilopsis.Model;
using Ptilopsis.PtiTask;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ptilopsis.PtiRun
{
    public class RunnerManager:IWorker
    {
        List<PtiRunner> RunnerList { get; set; }
        public RunnerManager()
        {
            this.RunnerList = new List<PtiRunner>();
        }
        public bool CreateRunner(PtiRunTask runTask)
        {
            PtiRunner runner = new PtiRunner(runTask.PtiTasker, runTask.PtiApp);
            this.RunnerList.Add(runner);
            runTask.Runner = runner;
            return true;
        }
        public bool CreateAndStart(PtiRunTask runTask)
        {
            this.CreateRunner(runTask);
            runTask.PtiTasker.LastRunDate= DateTime.Now;
            runTask.PtiApp.LastRunDate = runTask.PtiTasker.LastRunDate;
            runTask.Runner.Run();
            return true;
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
