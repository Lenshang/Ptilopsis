using Ptilopsis.Model;
using Ptilopsis.PtiApplication;
using Ptilopsis.PtiEvent;
using Ptilopsis.PtiTask;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ptilopsis.PtiDB
{
    public class DBManager:IWorker
    {
        public IDataBase Db { get; set; }
        public DBManager()
        {
            this.Db = this.GetNewDb();
        }
        public IDataBase GetNewDb()
        {
            //return new TestDataBase();
            return new LiteDbDataBase();
        }
        public IDataBase GetDB()
        {
            return Db;
        }

        public List<PtiTasker> GetAllTasks()
        {
            List<PtiTasker> results = new List<PtiTasker>();
            int skip = 0;
            int take = 10;
            while (true)
            {
                var items = this.Db.GetTasks(skip, take);
                if (items != null && items.Length == 0)
                {
                    break;
                }
                results.AddRange(items);
                skip += take;
            }
            return results;
        }
        public List<PtiTasker> GetAllEnableTasks()
        {
            List<PtiTasker> results = new List<PtiTasker>();
            int skip = 0;
            int take = 10;
            while (true)
            {
                var items = this.Db.GetTasks(skip, take);
                if (items != null && items.Length == 0)
                {
                    break;
                }
                foreach(var item in items)
                {
                    if (item.Enable == true)
                    {
                        results.Add(item);
                    }
                }
                skip += take;
            }
            return results;
        }
        public bool SaveAllTasks(List<PtiTasker> tasks)
        {
            try
            {
                foreach (var task in tasks)
                {
                    this.Db.AddOrUpdateTask(task);
                }
                return true;
            }
            catch(Exception e)
            {
                this.WriteError(e.ToString());
                return false;
            }
        }
        public bool SaveAllTasksAndApps(List<PtiRunTask> tasks)
        {
            try
            {
                foreach (var task in tasks)
                {
                    this.Db.AddOrUpdateTask(task.PtiTasker);
                    this.Db.AddOrUpdateApp(task.PtiApp);
                }
                return true;
            }
            catch (Exception e)
            {
                this.WriteError(e.ToString());
                return false;
            }
        }
        public bool SaveTask(PtiTasker task)
        {
            try
            {
                this.Db.AddOrUpdateTask(task);
                return true;
            }
            catch (Exception e)
            {
                this.WriteError(e.ToString());
                return false;
            }
        }
        /// <summary>
        /// 获得所有的Apps
        /// </summary>
        /// <returns></returns>
        public List<PtiApp> GetAllApps(IDataBase db=null)
        {
            IDataBase _db = null;
            if (db == null)
            {
                _db = this.Db;
            }
            else
            {
                _db = db;
            }
            List<PtiApp> results = new List<PtiApp>();
            int skip = 0;
            int take = 10;
            while (true)
            {
                var items = _db.GetApps(skip, take);
                if (items != null && items.Length == 0)
                {
                    break;
                }
                results.AddRange(items);
                skip += take;
            }
            return results;
        }
        /// <summary>
        /// 根据ID获得一个APP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public PtiApp GetAppById(string id,IDataBase db = null)
        {
            IDataBase _db = null;
            if (db == null)
            {
                _db = this.Db;
            }
            else
            {
                _db = db;
            }

            return _db.GetAppById(id);
        }
        public override bool Start()
        {
            return base.Start();
        }

        #region 单例模式
        private static DBManager _dbManager = null;
        public static DBManager Get()
        {
            if (_dbManager == null)
            {
                _dbManager = new DBManager();
            }
            return _dbManager;
        }
        #endregion
    }
}
