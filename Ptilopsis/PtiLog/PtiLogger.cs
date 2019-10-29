using Ptilopsis.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ptilopsis.PtiLog
{
    public class PtiLogger
    {
        public MessageObjectBox MessageBox { get; set; }
        public string LogName { get; set; }
        public string SavePath { get; set; }
        private string FullPath { get; set; }

        public PtiLogger(string logName="",string savePath="")
        {
            if (this.MessageBox == null)
            {
                this.MessageBox = new MessageObjectBox(20);
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
                this.LogName = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            if (string.IsNullOrWhiteSpace(this.SavePath))
            {
                this.SavePath = "";
            }
            this.FullPath = Path.Combine(this.SavePath, this.LogName + "_ptilog.txt");
        }
        public void RegLogReceive(Action<LogModel> action)
        {
            this.MessageBox.OnMessageRecieve = action;
        }
        public void Info(string message)
        {
            this.MessageBox.Add(message, Model.LogLevel.INFO);
        }
        public void Error(string message)
        {
            this.MessageBox.Add(message, Model.LogLevel.ERROR);
        }
        public void Warning(string message)
        {
            this.MessageBox.Add(message, Model.LogLevel.WARNING);
        }
    }
}
