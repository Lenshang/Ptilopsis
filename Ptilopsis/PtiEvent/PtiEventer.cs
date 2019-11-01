using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ptilopsis.PtiEvent
{
    public enum PtiEventType
    {
        CheckAllTasks,
        SyncDatabase,
        Internal,
        AddTask,
        UpdateTask,
        DisableTask,
        GetAllTasks,
        GetAllApps,
        Defalt
    }
    public class PtiEventer
    {
        Func<PtiEventer, object> MainAction { get; set; }
        Func<PtiEventer, object> CallBack { get; set; }
        public bool IsLoopEvent { get; set; } = false;
        public bool IsExcuted { get;protected set; }
        public object EventArgs { get; protected set; }
        public PtiEventType ActionType { get; protected set; }
        public DateTime CreateDate { get; protected set; }
        public DateTime ExcuteDate { get; protected set; }
        public Object Creator { get; protected set; }
        
        private PtiEventer(PtiEventType actionType,Object creator)
        {
            this.ActionType = actionType;
            this.CreateDate = DateTime.Now;
            this.IsExcuted = false;
            this.Creator = creator;
        }
        public void RunAndWait()
        {
            this.EventArgs = MainAction?.Invoke(this);
            this.ExcuteDate = DateTime.Now;
            this.IsExcuted = true;
        }
        public void RunAndWait(EventManager Em)
        {
            if (!this.IsExcuted||this.IsLoopEvent)
            {
                this.RunAndWait();
                if (this.CallBack != null)
                {
                    Em.RegEvent(this);
                }
            }
            else
            {
                this.EventArgs = CallBack?.Invoke(this);
            }
        }
        public void RunAndContinueWith(EventManager Em,Func<PtiEventer,object> continueAction)
        {
            this.RunAndWait(Em);
            Em.RegEvent(new PtiEventer(PtiEventType.Internal,this) {
                MainAction= continueAction
            });
        }
        public static PtiEventer Create(PtiEventType actionType, Object creator, Func<PtiEventer, object> action, Func<PtiEventer, object> callBack = null)
        {
            PtiEventer act = new PtiEventer(PtiEventType.CheckAllTasks,creator);
            act.MainAction = action;
            act.CallBack = callBack;
            return act;
        }
        public static PtiEventer Create(Object creator,Func<PtiEventer, object> action, Func<PtiEventer, object> callBack = null)
        {
            PtiEventer act = new PtiEventer(PtiEventType.Defalt,creator);
            act.MainAction = action;
            act.CallBack = callBack;
            return act;
        }
    }
}
