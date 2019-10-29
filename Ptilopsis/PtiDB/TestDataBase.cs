using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ptilopsis.Model;
using Ptilopsis.PtiApplication;
using Ptilopsis.PtiTask;

namespace Ptilopsis.PtiDB
{
    public class TestDataBase : IDataBase
    {
        public override bool AddOrUpdateApp(PtiApp app)
        {
            throw new NotImplementedException();
        }

        public override bool AddOrUpdateTask(PtiTasker task)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteApp(string id)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteTask(string id)
        {
            throw new NotImplementedException();
        }

        public override PtiApp GetAppById(string id)
        {
            throw new NotImplementedException();
        }

        public override PtiApp[] GetApps(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public override PtiTasker GetTask(string id)
        {
            throw new NotImplementedException();
        }

        public override PtiTasker[] GetTasks(int skip, int take)
        {
            return null;
        }

        public override PtiApp[] SearchAppByName(string name)
        {
            throw new NotImplementedException();
        }

        public override PtiTasker[] SearchTaskByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
