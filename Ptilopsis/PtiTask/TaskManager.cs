using Ptilopsis.PtiApplication;
using Ptilopsis.Model;
using Ptilopsis.PtiEvent;
using Ptilopsis.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiRun;
using System.Linq;
using System.Threading;
using System.IO;

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
            CheckTasksLoopInterval = TimeSpan.FromSeconds(Config.Get().CheckTasksLoopIntervalSeconds);
            SyncDatabaseInterval = TimeSpan.FromSeconds(Config.Get().SyncDatabaseIntervalSeconds);
            TaskPool = new List<PtiRunTask>();
            Schedule = new DateSchedule();
        }
        public override bool Start()
        {
            if (base.Start())
            {
                EventManager.Get().RegLoopEvent(this.CheckAllTasksEvent, PtiEventType.CheckAllTasks, this.CheckTasksLoopInterval);
                EventManager.Get().RegLoopEvent(this.SyncDatabaseEvent, PtiEventType.SyncDatabase, this.SyncDatabaseInterval);
                //EventManager.Get().RegLoopEvent((eventer)=> { WriteInfo(Secret.Ptilopsis(), "Secret");return null; },TimeSpan.FromSeconds(10));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 循环遍历所有任务，检查可执行的并且执行
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object CheckAllTasksEvent(PtiEventer value)
        {
            DateTime now = DateTime.Now;
            List<PtiRunTask> removeList = new List<PtiRunTask>();
            foreach(var runtask in this.TaskPool)
            {
                try
                {
                    //判断是否已经禁用任务，从内存中删除任务
                    if (!runtask.PtiTasker.Enable)
                    {
                        removeList.Add(runtask);
                        continue;
                    }

                    //判断任务运行是否已经超时
                    if (runtask.PtiTasker.TimeOutSeconds>0)
                    {
                        if ((runtask.LastRunDate - now) >= TimeSpan.FromSeconds(runtask.PtiTasker.TimeOutSeconds))
                        {
                            if (!RunnerManager.Get().CheckTaskAndKill(runtask.PtiTasker))
                            {
                                WriteWarning($"Task {runtask.PtiTasker._id}({runtask.PtiTasker.TaskName}) Kill Failure!");
                            }
                        }
                    }

                    //判断启动时间启动任务
                    if (now >= runtask.NextRunDate)
                    {
                        if (!RunnerManager.Get().CreateAndStart(runtask))
                        {
                            WriteWarning($"Task {runtask.PtiTasker._id}({runtask.PtiTasker.TaskName}) Start Failure!");
                        }

                        //没有任务计划的为一次性任务，执行后标记为失效
                        if (string.IsNullOrWhiteSpace(runtask.PtiTasker.Schedule))
                        {
                            runtask.PtiTasker.Enable = false;
                            removeList.Add(runtask);
                        }
                        else
                        {
                            runtask.NextRunDate = Schedule.CalculateDateScheduleFromNow(runtask.PtiTasker.Schedule);
                        }
                            
                    }
                }
                catch(Exception e)
                {
                    WriteError(e.ToString());
                }
            }
            foreach(var removetask in removeList)
            {
                //立即更新任务
                DBManager.Get().SaveTask(removetask.PtiTasker);
                //在内存中移除任务
                //if (!this.TaskPool.Remove(removetask))
                //{
                //    WriteWarning($"Task {removetask.PtiTasker._id}({removetask.PtiTasker.TaskName}) remove failure!");
                //}
            }
            WriteInfo("Check All Tasks Success");
            return true;
        }
        /// <summary>
        /// 将任务同步至数据库
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object SyncDatabaseEvent(PtiEventer value)
        {
            DBManager.Get().SaveAllTasksAndApps(this.TaskPool);
            WriteInfo("Database Synchronized!");
            return true;
        }
        /// <summary>
        /// 获得当前运行的所有任务
        /// </summary>
        /// <param name="callback"></param>
        public void GetAllTasks(Action<PtiTasker[]> callback)
        {
            EventManager.Get().RegEvent(ptievent => {
                var result = this.TaskPool.Select(p => p.PtiTasker).ToArray();
                callback?.Invoke(result);
                return null;
            }, PtiEventType.GetAllTasks);
        }
        /// <summary>
        /// 获得当前运行的所有任务（同步）
        /// </summary>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public PtiTasker[] GetAllTasksSync(int timeout=10000)
        {
            var ptiEvent=EventManager.Get().RegEvent(ptievent => {
                var result = this.TaskPool.Select(p => p.PtiTasker).ToArray();
                return result;
            }, PtiEventType.GetAllTasks);

            DateTime TimeOut = DateTime.Now.AddMilliseconds(timeout);
            while (DateTime.Now< TimeOut)
            {
                if (ptiEvent.IsExcuted)
                {
                    return ptiEvent.EventResult as PtiTasker[];
                }
                Thread.Sleep(100);
            }
            return null;
        }
        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="app"></param>
        /// <param name="immediately"></param>
        public PtiEventer AddTask(PtiTasker tasker,PtiApp app=null,bool immediately=false)
        {
            return EventManager.Get().RegEvent(ptievent => {
                try
                {
                    if (app == null)
                    {
                        app = DBManager.Get().Db.GetAppById(tasker.ApplicationId);
                        if (app == null)
                        {
                            return false;
                        }
                    }
                    var r = this.TaskPool.Where(i => i.PtiTasker._id == tasker._id).FirstOrDefault();
                    if (r == null)
                    {
                        if (!CheckTask(tasker))
                        {
                            WriteWarning($"Task {tasker._id}({tasker.TaskName}) 添加失败,任务参数不正确！");
                            return false;
                        }
                        //APP改成从APP模块查询获得
                        PtiRunTask runtask = new PtiRunTask()
                        {
                            PtiTasker = tasker,
                            PtiApp = app
                        };
                        if (!immediately)
                        {
                            if (runtask.PtiTasker.Schedule != null)
                            {
                                runtask.NextRunDate = Schedule.CalculateDateScheduleFromNow(runtask.PtiTasker.Schedule);
                            }
                            else
                            {
                                runtask.NextRunDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            runtask.NextRunDate = DateTime.Now.AddSeconds(3);
                        }
                        this.TaskPool.Add(runtask);
                        return true;
                    }
                    else
                    {
                        if (!r.PtiTasker.Enable)
                        {
                            r.PtiTasker.Enable = true;
                        }
                    }
                    return true;
                }
                catch(Exception e)
                {
                    WriteError(e.ToString());
                    return false;
                }
            }, PtiEventType.AddTask);
        }

        public void UpdateTask(string id,PtiTasker tasker)
        {
            EventManager.Get().RegEvent(ptievent => {
                var task = this.TaskPool.Where(i => i.PtiTasker._id == id).FirstOrDefault();
                if (task != null)
                {
                    task.PtiTasker.Update(tasker);
                }
                else
                {

                }
                return null;
            }, PtiEventType.UpdateTask);
        }
        public void DisableTask(string id)
        {
            EventManager.Get().RegEvent(ptievent => {
                var task = this.TaskPool.Where(i => i.PtiTasker._id == id).FirstOrDefault();
                if (task != null)
                {
                    task.PtiTasker.Enable = false;
                    this.TaskPool.Remove(task);
                }
                else
                {

                }
                return null;
            }, PtiEventType.DisableTask);
        }
        public bool? KillTaskById(string id,int timeout=10000)
        {
            var ptiEvent = EventManager.Get().RegEvent(ptievent => {
                return RunnerManager.Get().TaskKillById(id);
            }, PtiEventType.Default);

            DateTime TimeOut = DateTime.Now.AddMilliseconds(timeout);
            while (DateTime.Now < TimeOut)
            {
                if (ptiEvent.IsExcuted)
                {
                    return ptiEvent.EventResult as bool?;
                }
                Thread.Sleep(100);
            }
            return null;
        }

        public bool CheckTask(PtiTasker tasker)
        {
            if (string.IsNullOrWhiteSpace(tasker._id) && string.IsNullOrWhiteSpace(tasker.TaskName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(tasker.ApplicationId))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(tasker.RunPath))
            {
                //tasker.RunPath = "./" + tasker.ApplicationId;
                tasker.RunPath = Path.Combine(Config.Get().AppRunPath, tasker.ApplicationId);
            }
            if (string.IsNullOrWhiteSpace(tasker._id))
            {
                tasker._id = MD5Helper.getMd5Hash(tasker.TaskName);
            }
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
