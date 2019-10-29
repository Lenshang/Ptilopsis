using Ptilopsis.PtiApplication;
using Ptilopsis.Model;
using Ptilopsis.PtiEvent;
using Ptilopsis.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiRun;

namespace Ptilopsis.PtiTask
{
    public class TaskManager: IWorker
    {
        public TimeSpan CheckTasksLoopInterval { get; set; }
        public TimeSpan SyncDatabaseInterval { get; set; }
        List<PtiRunTask> TaskPool { get; set; }
        DateSchedule Schedule { get; set; }
        public TaskManager()
        {
            CheckTasksLoopInterval = TimeSpan.FromSeconds(30);
            SyncDatabaseInterval = TimeSpan.FromMinutes(5);
            TaskPool = new List<PtiRunTask>();
            Schedule = new DateSchedule();
            //TODO 通过事件驱动模型方式执行，不考虑异步操作
        }
        public override void Start()
        {
            base.Start();
            EventManager.Get().RegLoopEvent(this.CheckAllTasksEvent, PtiEventType.CheckAllTasks, this.CheckTasksLoopInterval);
            EventManager.Get().RegLoopEvent(this.SyncDatabaseEvent, PtiEventType.SyncDatabase, this.SyncDatabaseInterval);
            //EventManager.Get().RegLoopEvent(()=> WriteInfo(Secret.Ptilopsis(),"Secret"),TimeSpan.FromSeconds(10));
        }
        public object CheckAllTasksEvent(object value)
        {
            DateTime now = DateTime.Now;
            foreach(var runtask in this.TaskPool)
            {
                try
                {
                    if (now >= runtask.NextRunDate)
                    {
                        if (!RunnerManager.Get().CreateAndStart(runtask))
                        {
                            WriteWarning($"Task {runtask.PtiTasker.TaskName} Start Failure!");
                        }
                        runtask.NextRunDate = Schedule.CalculateDateScheduleFromNow(runtask.PtiTasker.Schedule);
                    }
                }
                catch(Exception e)
                {
                    WriteWarning(e.ToString());
                }
            }
            WriteInfo("Check All Tasks Success");
            return true;
        }
        public object SyncDatabaseEvent(object value)
        {
            DBManager.Get().SaveAllTasks(this.TaskPool);
            WriteInfo("Database Synchronized!");
            return true;
        }
        #region 单例模式
        private static TaskManager _taskManager = null;
        public static TaskManager Get()
        {
            if (_taskManager == null)
            {
                _taskManager = new TaskManager();
            }
            return _taskManager;
        }
        #endregion
    }
}
