using Ptilopsis.PtiRun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TestClient
{
    public class PtiTester
    {
        public void TestRunner(PtiRunner runner)
        {
            Print(
                "==+PtiRunner Test+==",
                "==========可用指令=========",
                "getstate:查看当前任务状态",
                "run:启动任务",
                "kill:杀死任务",
                "logs:获得最新10条日志",
                "help:获得帮助",
                "===========================");
            while (true)
            {
                var input = Input(">");
                switch (input)
                {
                    case "getstate":
                        Print(runner.State);
                        break;
                    case "run":
                        runner.Run();
                        break;
                    case "kill":
                        runner.KillAsync();
                        break;
                    case "logs":
                        Print(runner.Logger.MessageBox.GetAll().Select(i => i.Date.ToString() + $"[{i.Level}]:" + i.Message));
                        break;
                    case "help":
                        Print(
                            "==========可用指令=========",
                            "getstate:查看当前任务状态",
                            "run:启动任务",
                            "kill:杀死任务",
                            "help:获得帮助",
                            "===========================");
                        break;
                }
            }
        }
        private string Input(Object msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }
        private void Print(Object msg)
        {
            Console.WriteLine(msg);
        }
        private void Print(params Object[] msgs)
        {
            foreach (var msg in msgs)
            {
                Console.WriteLine(msg);
            }
        }
        private void Print(IEnumerable<object> msgs)
        {
            foreach (var msg in msgs)
            {
                Console.WriteLine(msg);
            }
        }
    }
}
