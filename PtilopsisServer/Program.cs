using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ptilopsis.PtiApplication;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiEvent;
using Ptilopsis.PtiRun;
using Ptilopsis.PtiTask;

namespace PtilopsisServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("PtilopsisCliServer 0.1");
                Console.WriteLine("2019-11-08");
                DBManager.Get().Start();

                RunnerManager.Get().Start();

                TaskManager.Get().Start();

                AppManager.Get().Start();

                EventManager.Get().Start();

                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception e)
            {
                Console.WriteLine("Start Error£¡Message:"+e.ToString());
                Thread.Sleep(3000);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(Config.Get().BindAddress);
                    webBuilder.UseStartup<Startup>();
                });
    }
}
