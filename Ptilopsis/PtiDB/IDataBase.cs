using Ptilopsis.Model;
using Ptilopsis.PtiApplication;
using Ptilopsis.PtiTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ptilopsis.PtiDB
{
    public abstract class IDataBase
    {
        public abstract PtiApp GetAppById(string id);
        public abstract PtiApp[] SearchAppByName(string name);
        public abstract PtiApp[] GetApps(int skip,int take);
        public abstract bool AddOrUpdateApp(PtiApp app);
        public abstract bool DeleteApp(string id);

        public abstract PtiTasker GetTask(string id);
        public abstract PtiTasker[] SearchTaskByName(string name);
        public abstract PtiTasker[] GetTasks(int skip, int take);
        public abstract Int32 GetTaskCount();
        public abstract bool AddOrUpdateTask(PtiTasker task);
        public abstract bool DeleteTask(string id);


    }
}
