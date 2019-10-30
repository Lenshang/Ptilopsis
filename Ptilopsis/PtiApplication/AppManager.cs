using Ptilopsis.Model;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiEvent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ptilopsis.PtiApplication
{
    public class AppManager: IWorker
    {
        private AppManager()
        {

        }

        public void GetAllApps(Action<PtiApp[]> callback)
        {
            EventManager.Get().RegEvent(ptievent => {
                var result = DBManager.Get().GetAllApps().ToArray();
                callback?.Invoke(result);
                return null;
            }, PtiEventType.GetAllApps);
        }

        #region 单例模式
        private static AppManager _appManager = null;
        public static AppManager Get()
        {
            if (_appManager == null)
            {
                _appManager = new AppManager();
            }
            return _appManager;
        }
        #endregion
    }
}
