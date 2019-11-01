using Ptilopsis.Model;
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
        private string FullPath { get; set; }
        private object Locker { get; set; }
        public PtiLogger(string logName="",string savePath="")
        {
            this.Locker = new object();
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
        public PtiLogger(MessageObjectBox messageBox, string logName = "", string savePath = "")
        {
            this.MessageBox = messageBox;
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

            this.FullPath = Path.Combine(this.SavePath, DateTime.Now.ToString("yyyyMMddHHmmss") + "_ptilog.txt");
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
                LogModel model = new LogModel()
                {
                    Message = log,
                    Date = DateTime.Now,
                    Level = level
                };
                this.MessageBox.Add(model);
                lock (Locker)
                {
                    var FStream = File.Open(this.FullPath, FileMode.OpenOrCreate);
                    var FWriter = new StreamWriter(FStream);
                    FWriter.WriteLine(model.Date.ToString() + $"[{model.Level}]:" + model.Message);
                    FWriter.Close();
                    FStream.Close();
                }
            }
            catch
            {

            }
        }

        public void Dispose()
        {
            try
            {

            }
            catch
            {

            }
        }
    }
}
