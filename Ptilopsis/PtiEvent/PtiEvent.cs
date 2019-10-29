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
        Defalt
    }
    public class PtiEvent
    {
        Func<PtiEvent, object> MainAction { get; set; }
        Func<PtiEvent, object> CallBack { get; set; }
        bool RunCallBack { get; set; }
        public object EventArgs { get; set; }
        public PtiEventType ActionType { get; set; }
        public DateTime CreateDate { get; set; }
        public Object Creator { get; set; }
        private PtiEvent(PtiEventType actionType,Object creator)
        {
            this.ActionType = actionType;
            this.CreateDate = DateTime.Now;
            this.RunCallBack = false;
            this.Creator = creator;
        }
        public void RunAndWait()
        {
            var r = MainAction?.Invoke(this);
        }
        public void RunAndWait(EventManager Em)
        {
            if (!this.RunCallBack)
            {
                this.EventArgs = MainAction?.Invoke(this);
                this.RunCallBack = true;
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
        public void RunAndContinueWith(EventManager Em,Func<PtiEvent,object> continueAction)
        {
            this.RunAndWait(Em);
            Em.RegEvent(new PtiEvent(PtiEventType.Internal,this) {
                MainAction= continueAction
            });
        }
        public static PtiEvent Create(PtiEventType actionType, Object creator, Func<PtiEvent, object> action, Func<PtiEvent, object> callBack = null)
        {
            PtiEvent act = new PtiEvent(PtiEventType.CheckAllTasks,creator);
            act.MainAction = action;
            act.CallBack = callBack;
            return act;
        }
        public static PtiEvent Create(Object creator,Func<PtiEvent, object> action, Func<PtiEvent, object> callBack = null)
        {
            PtiEvent act = new PtiEvent(PtiEventType.Defalt,creator);
            act.MainAction = action;
            act.CallBack = callBack;
            return act;
        }
    }
}
