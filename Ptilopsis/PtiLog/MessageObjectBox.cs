using Ptilopsis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ptilopsis.PtiLog
{
    public class MessageObjectBox
    {
        private LogModel[] _logs { get; set; }
        private object locker { get; set; }
        private int size { get; set; }
        public Action<LogModel> OnMessageRecieve { get; set; }
        public MessageObjectBox(int _size)
        {
            this.size = _size;
            this._logs = new LogModel[size];
            this.locker = new object();
        }

        public void Add(LogModel log)
        {
            lock (locker)
            {
                for (int i = 0; i < _logs.Length; i++)
                {
                    if (i > 0)
                    {
                        _logs[i - 1] = _logs[i];
                    }
                }
                _logs[_logs.Length - 1] = log;
                this.OnMessageRecieve?.Invoke(log);
            }
        }
        public void Add(string log, LogLevel level= LogLevel.INFO)
        {
            LogModel model = new LogModel()
            {
                Message=log,
                Date=DateTime.Now,
                Level= level
            };
            this.Add(model);
        }
        public void Clear()
        {
            lock (locker)
            {
                this._logs = new LogModel[size];
            }
        }

        public List<LogModel> GetAll()
        {
            List<LogModel> result = new List<LogModel>();
            lock (locker)
            {
                foreach(var log in _logs)
                {
                    if (log != null)
                    {
                        result.Add(log);
                    }
                }
                
            }
            return result;
        }

        public LogModel GetNew()
        {
            LogModel result = null;
            lock (locker)
            {
                result = _logs.Last();
            }
            return result;
        }
    }
}
