﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDB;
using Ptilopsis.PtiApplication;
using Ptilopsis.PtiTask;

namespace Ptilopsis.PtiDB
{
    class LiteDbDataBase : IDataBase
    {
        public object locker { get; set; }
        public LiteDbDataBase()
        {
            locker = new object();
        }
        public override bool AddOrUpdateApp(PtiApp app)
        {
            lock (locker)
            {
                if (string.IsNullOrWhiteSpace(app.Id))
                {
                    return false;
                }
                else
                {
                    app._id = app.Id;
                }
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiApp>("apps");
                    col.Upsert(app._id, app);
                }
                return true;
            }
        }
        public override PtiApp GetAppById(string id)
        {
            lock (locker)
            {
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiApp>("apps");
                    return col.FindById(id);
                }
            }
        }

        public override PtiApp[] GetApps(int skip, int take)
        {
            lock (locker)
            {
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiApp>("apps");
                    return col.Find(q => true, skip, take).ToArray();
                }
            }
        }

        public override bool AddOrUpdateTask(PtiTasker task)
        {
            lock (locker)
            {
                if (string.IsNullOrWhiteSpace(task._id))
                {
                    return false;
                }
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiTasker>("tasks");
                    col.Upsert(task._id, task);
                }
                return true;
            }
        }

        public override bool DeleteApp(string id)
        {
            lock (locker)
            {
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiApp>("apps");
                    return col.Delete(id);
                }
            }
        }

        public override bool DeleteTask(string id)
        {
            lock (locker)
            {
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiTasker>("tasks");
                    return col.Delete(id);
                }
                return true;
            }
        }

        public override PtiTasker GetTask(string id)
        {
            lock (locker)
            {
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiTasker>("tasks");
                    return col.FindById(id);
                }
            }
        }

        public override PtiTasker[] GetTasks(int skip, int take)
        {
            lock (locker)
            {
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiTasker>("tasks");
                    return col.Find(q => true, skip, take).ToArray();
                }
            }
        }
        public override int GetTaskCount()
        {
            lock (locker)
            {
                using (var db = new LiteDatabase(Config.Get().LiteDBFile))
                {
                    var col = db.GetCollection<PtiTasker>("tasks");
                    return col.Count();
                }
            }
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
