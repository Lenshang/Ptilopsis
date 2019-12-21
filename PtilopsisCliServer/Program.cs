using Ptilopsis.PtiApplication;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiEvent;
using Ptilopsis.PtiRun;
using Ptilopsis.PtiTask;
using System;

namespace PtilopsisCliServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PtilopsisCliServer 0.1");
            Console.WriteLine("2019-11-08");
            DBManager.Get().Start();

            RunnerManager.Get().Start();

            TaskManager.Get().Start();

            AppManager.Get().Start();

            EventManager.Get().Start();


        }
    }
}
