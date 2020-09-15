using Ptilopsis.Model;
using Ptilopsis.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ptilopsis.PtiLog
{
    public class PtiLogger:IDisposable
    {
        public MessageObjectBox MessageBox { get; set; }
        public string LogName { get; set; }
        public string SavePath { get; set; }
        public bool FormatOutput { get; set; } = true;
        public string FileName { get; set; }
        private string FullPath { get; set; }
        private string FileFormat { get; set; }
        private object Locker { get; set; }

        private DateTime LastFlushTime { get; set; }
        private FileStream FStream { get; set; }
        private StreamWriter FWriter { get; set; }
        public PtiLogger(string logName="",string savePath="", string fileFormat = "yyyyMMdd")
        {
            this.Locker = new object();
            this.FileFormat = fileFormat;
            if (this.MessageBox == null)
            {
                this.MessageBox = new MessageObjectBox(20);
            }
            if (!string.IsNullOrEmpty(savePath))
            {
                this.SavePath = savePath;
            }
            LogName = logName;
            this.Init();
        }
        public PtiLogger(MessageObjectBox messageBox, string logName = "", string savePath = "",string fileFormat= "yyyyMMdd")
        {
            this.MessageBox = messageBox;
            this.FileFormat = fileFormat;
            LogName = logName;
            this.Init();
        }
        private void Init()
        {
            if (string.IsNullOrWhiteSpace(this.LogName))
            {
                this.LogName = "UnnameLogs";
            }
            if (string.IsNullOrWhiteSpace(this.SavePath))
            {
                this.SavePath = Path.Combine(Config.Get().AppLogPath,this.LogName);
            }
            if (!Directory.Exists(this.SavePath))
            {
                Directory.CreateDirectory(this.SavePath);
            }
            this.FileName = DateTime.Now.ToString(this.FileFormat) + ".log";
            this.FullPath = Path.Combine(this.SavePath, this.FileName);
            this.LastFlushTime = DateTime.Now;
            this.FStream = File.Open(this.FullPath, System.IO.FileMode.OpenOrCreate,FileAccess.Write,FileShare.Read);
            this.FWriter = FWriter = new StreamWriter(FStream);
        }
        public void RegLogReceive(Action<LogModel> action)
        {
            this.MessageBox.OnMessageRecieve = action;
        }
        public void Info(string message)
        {
            WriteLog(message, Model.LogLevel.INFO);
        }
        public void Error(string message)
        {
            WriteLog(message, Model.LogLevel.ERROR);
        }
        public void Warning(string message)
        {
            WriteLog(message, Model.LogLevel.WARNING);
        }
        public void WriteLog(string log, LogLevel level = LogLevel.INFO)
        {
            try
            {
                if (string.IsNullOrEmpty(log))
                {
                    return;
                }

                LogModel model = new LogModel()
                {
                    Message = log,
                    Date = DateTime.Now,
                    Level = level
                };
                model._id = Guid.NewGuid().ToString("N");
                this.MessageBox.Add(model);
                lock (this.Locker)
                {
                    string msg = "";
                    if (this.FormatOutput)
                    {
                        msg = model.Date.ToString() + $"[{model.Level}]:" + model.Message;
                    }
                    else
                    {
                        msg = model.Message;
                    }
                    this.FWriter.WriteLine(msg);
                    var _now = DateTime.Now;
                    if ((_now - this.LastFlushTime) > TimeSpan.FromSeconds(5) || level>=LogLevel.WARNING)//5秒手动FLUSH文件一次 TODO改成可配置  
                    {
                        this.FWriter.Flush();
                        this.LastFlushTime = _now;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string GetFullPath()
        {
            return this.FullPath;
        }
        public void Dispose()
        {
            try
            {
                this.FWriter.Close();
                this.FStream.Close();
            }
            catch
            {

            }
        }
    }
}
