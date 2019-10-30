using Ptilopsis.Model;
using Ptilopsis.PtiApplication;
using Ptilopsis.PtiTask;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Ptilopsis.PtiEvent
{
    /// <summary>
    /// 事件驱动模型事件管理器，LoopActions数组在启动之后就不再更改，所以EventManager模块应最后启动
    /// </summary>
    public class EventManager: IWorker
    {
        /// <summary>
        /// 动作队列
        /// </summary>
        ConcurrentQueue<PtiEventer> ActionQueue { get; set; }
        /// <summary>
        /// 循环线程
        /// </summary>
        Thread LoopThread { get; set; }
        /// <summary>
        /// 停止循环标记 True=停止循环
        /// </summary>
        bool StopLoopMark { get; set; }
        /// <summary>
        /// 注册全局的循环任务
        /// </summary>
        List<PtiLoopEvent> LoopActions { get; set; }

        private EventManager()
        {
            this.ActionQueue = new ConcurrentQueue<PtiEventer>();
            this.StopLoopMark = false;
            this.LoopActions = new List<PtiLoopEvent>();
        }
        public override void Start()
        {
            base.Start();
            this.StartLoop();
        }
        public void StartLoop()
        {
            if (this.LoopThread == null)
            {
                this.LoopThread = new Thread(this.MainLoop);
            }

            if (this.LoopThread.ThreadState != ThreadState.Running)
            {
                try
                {
                    this.LoopThread.Start();
                    this.WriteInfo("Loop Thread Start Success");
                }
                catch(Exception e)
                {
                    this.WriteError("Loop Thread Start Failure err=>"+e.Message);
                }
            }
        }
        public void StopLoop()
        {
            this.StopLoopMark = true;
        }
        private void MainLoop()
        {
            //启动循环，this.LoopActions数组在启动之后就不再更改，所以EventManager模块应最后启动
            var _loopActions = new List<PtiLoopEvent>();
            _loopActions.AddRange(this.LoopActions);
            DateTime now = DateTime.Now;
            while (!StopLoopMark)
            {
                #region 全局Loop模块任务
                now = DateTime.Now;
                foreach(var loopAction in _loopActions)
                {
                    if (now >= loopAction.NextRunDate)
                    {
                        ActionQueue.Enqueue(loopAction.Action);
                        loopAction.NextRunDate = now.Add(loopAction.Interval);
                    }
                }
                #endregion

                PtiEventer act;
                while(ActionQueue.TryDequeue(out act))
                {
                    act.RunAndWait(this);
                }
                Thread.Sleep(100);
            }
            StopLoopMark = false;
            this.WriteInfo("Loop Thread Stopped");
        }
        /// <summary>
        /// 注册一个LoopEvent (LoopEvent只有在EventManager启动之前注册有效)
        /// </summary>
        /// <param name="action">执行的委托，返回object</param>
        /// <param name="interval">执行间隔</param>
        /// <param name="IsImmediately">是否立即执行</param>
        public void RegLoopEvent(PtiEventer action,TimeSpan interval,bool IsImmediately=false)
        {
            PtiLoopEvent act = new PtiLoopEvent()
            {
                Action = action,
                Interval = interval,
                NextRunDate = DateTime.Now
            };
            if (!IsImmediately)
            {
                act.NextRunDate = act.NextRunDate.Add(act.Interval);
            }
            this.LoopActions.Add(act);
        }
        /// <summary>
        /// 注册一个LoopEvent (LoopEvent只有在EventManager启动之前注册有效)
        /// </summary>
        /// <param name="action">执行的委托，返回object</param>
        /// <param name="actionType">执行的类型</param>
        /// <param name="interval">执行间隔</param>
        /// <param name="IsImmediately">是否立即执行</param>
        /// <param name="callBack">回调方法，传入object</param>
        public PtiEventer RegLoopEvent(Func<PtiEventer, object> action, PtiEventType actionType, TimeSpan interval, bool IsImmediately = false, Func<PtiEventer, object> callBack =null)
        {
            PtiEventer ptiEvent = PtiEventer.Create(actionType,this, action, callBack);
            this.RegLoopEvent(ptiEvent, interval, IsImmediately);
            return ptiEvent;
        }
        /// <summary>
        /// 注册一个LoopEvent (LoopEvent只有在EventManager启动之前注册有效)
        /// </summary>
        /// <param name="action">执行的委托，返回object</param>
        /// <param name="interval">执行间隔</param>
        /// <param name="IsImmediately">是否立即执行</param>
        /// <param name="callBack">回调方法，传入object</param>
        public PtiEventer RegLoopEvent(Func<PtiEventer, object> action, TimeSpan interval, bool IsImmediately = false, Func<PtiEventer, object> callBack = null)
        {
            return this.RegLoopEvent(action, PtiEventType.Defalt, interval, IsImmediately, callBack);
        }
        /// <summary>
        /// 注册一个Event
        /// </summary>
        /// <param name="action"></param>
        public void RegEvent(PtiEventer action)
        {
            this.ActionQueue.Enqueue(action);
        }
        /// <summary>
        /// 注册一个Event
        /// </summary>
        /// <param name="action">执行的委托，返回object</param>
        /// <param name="actionType">执行类型</param>
        /// <param name="callBack">回调方法</param>
        public PtiEventer RegEvent(Func<PtiEventer, object> action, PtiEventType actionType, Func<PtiEventer, object> callBack = null)
        {
            PtiEventer ptiEvent = PtiEventer.Create(actionType,this, action, callBack);
            this.RegEvent(ptiEvent);
            return ptiEvent;
        }
        /// <summary>
        /// 注册一个Event
        /// </summary>
        /// <param name="action">执行的委托，返回object</param>
        /// <param name="callBack">回调方法</param>
        public PtiEventer RegEvent(Func<PtiEventer, object> action, Func<PtiEventer, object> callBack = null)
        {
            return this.RegEvent(action, PtiEventType.Defalt, callBack);
        }

        #region 单例模式
        private static EventManager _actionManager = new EventManager();
        public static EventManager Get()
        {
            if (_actionManager == null)
            {
                _actionManager = new EventManager();
            }
            return _actionManager;
        }
        #endregion
    }
}
