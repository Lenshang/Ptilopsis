using Ptilopsis.PtiApplication;
using Ptilopsis.Model;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiEvent;
using Ptilopsis.PtiRun;
using Ptilopsis.PtiTask;
using System;
using System.Threading;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ptilopsis 0.1");
            Console.WriteLine("2019-10-21");
            var dbManager = DBManager.Get();
            dbManager.Logs.RegLogReceive(log => {
                Console.WriteLine(log.Date.ToString() + $"[{log.Level}]:" + log.Message);
            });

            var runnerManager = RunnerManager.Get();
            runnerManager.Logs.RegLogReceive(log => {
                Console.WriteLine(log.Date.ToString() + $"[{log.Level}]:" + log.Message);
            });

            var taskManager = TaskManager.Get();
            taskManager.Logs.RegLogReceive(log => {
                Console.WriteLine(log.Date.ToString() + $"[{log.Level}]:" + log.Message);
            });

            var appManager = AppManager.Get();
            appManager.Logs.RegLogReceive(log => {
                Console.WriteLine(log.Date.ToString() + $"[{log.Level}]:" + log.Message);
            });

            var eventManager = EventManager.Get();
            eventManager.Logs.RegLogReceive(log => {
                Console.WriteLine(log.Date.ToString() + $"[{log.Level}]:" + log.Message);
            });

            dbManager.Start();
            runnerManager.Start();
            taskManager.Start();
            appManager.Start();
            eventManager.Start();

            DateSchedule sch = new DateSchedule();

            Console.WriteLine(sch.CalculateDateScheduleFromNow("*,*,*,1,0"));
            Console.WriteLine(sch.CalculateDateScheduleFromNow("*,*,5,1,0"));
            //#if DEBUG
            //            PtiTester tester = new PtiTester();
            //            PtiApp app = new PtiApp();//APP 项目目录
            //            PtiTask task = new PtiTask();
            //            task.RunPath = "D:\\RunPath";
            //            task.RunCmd = "python";
            //            task.RunArgs = "main.py";
            //            PtiRunner runner = new PtiRunner(task, app);
            //            tester.TestRunner(runner);
            //#endif
            //            Console.WriteLine("Hello World!");
        }
    }
}
