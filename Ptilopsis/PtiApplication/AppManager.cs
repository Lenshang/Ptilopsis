using Ptilopsis.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ptilopsis.PtiApplication
{
    public class AppManager: IWorker
    {

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
