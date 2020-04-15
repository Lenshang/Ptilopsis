using Ptilopsis.PtiApplication;
using Ptilopsis.Model;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiEvent;
using Ptilopsis.PtiRun;
using Ptilopsis.PtiTask;
using System;
using System.Threading;
using Ptilopsis.Utils;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ptilopsis 0.1");
            Console.WriteLine("2019-10-21");
            var dbManager = DBManager.Get();

            var runnerManager = RunnerManager.Get();

            var taskManager = TaskManager.Get();

            var appManager = AppManager.Get();

            var eventManager = EventManager.Get();

            dbManager.Start();
            runnerManager.Start();
            taskManager.Start();
            appManager.Start();
            eventManager.Start();

            #region 任务计划计算测试
            //DateSchedule sch = new DateSchedule();
            //Console.WriteLine(sch.CalculateDateScheduleFromNow("*,*,*,1,0"));
            //Console.WriteLine(sch.CalculateDateScheduleFromNow("*,*,5,1,0"));
            #endregion

            #region App解压测试
            PtiApp app = new PtiApp()
            {
                Name = "Hello Ptilopsis",
                ZipFile = "main.zip",
                DefaultRunCmd = "python"
            };
            app.Id = MD5Helper.getMd5Hash(app.Name);
            appManager.AddApp(app);

            PtiApp app2 = new PtiApp()
            {
                Name = "EchoTest",
                DefaultRunCmd = "echo"
            };
            app2.Id = MD5Helper.getMd5Hash(app.Name);
            appManager.AddApp(app2);
            foreach (var item in dbManager.GetAllApps())
            {
                Console.WriteLine(item.Name);
            }
            #endregion

            #region TASK测试
            //PtiTasker task = new PtiTasker()
            //{
            //    ApplicationId=app.Id,
            //    RunArgs="main.py",
            //    TaskName= "Hello Ptilopsis",
            //    _id=Guid.NewGuid().ToString("N")
            //};
            PtiTasker task = new PtiTasker()
            {
                ApplicationId = app2.Id,
                RunArgs = "echoTest",
                TaskName = "echotest",
                _id = Guid.NewGuid().ToString("N")
            };
            var _t =taskManager.AddTask(task);
            while (!_t.IsExcuted)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine(_t.EventArgs);
            #endregion
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
