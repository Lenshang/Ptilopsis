using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ptilopsis.PtiTask;
using PtilopsisServer.ApiModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtilopsisServer.Controller
{
    public class ApiFile
    {
        public string name { get; set; }
        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool isDir { get; set; } = false;
        /// <summary>
        /// 文件大小
        /// </summary>
        public long size { get; set; } = 0;
    }
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogController : ControllerBase
    {
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            if (!Directory.Exists(Ptilopsis.Config.Get().AppLogPath))
            {
                return ApiResult.OK(new object[] { });
            }
            List<ApiFile> r = new List<ApiFile>();

            var taskLogDir = Directory.GetDirectories(Ptilopsis.Config.Get().AppLogPath);
            foreach(var dir in taskLogDir)
            {
                var dirInfo = new DirectoryInfo(dir);
                r.Add(new ApiFile()
                {
                    name=dirInfo.Name,
                    isDir=true
                });
            }
            return ApiResult.OK(r);
        }
        [HttpGet("getlogs")]
        public IActionResult GetAllLogs([FromQuery]string taskid)
        {
            if (string.IsNullOrWhiteSpace(taskid))
            {
                return ApiResult.Failure();
            }
            string dirPath = Path.Combine(Ptilopsis.Config.Get().AppLogPath, taskid);
            if (!Directory.Exists(dirPath))
            {
                return ApiResult.OK(new object[] { });
            }

            var taskLogFile = Directory.GetFiles(dirPath);

            List<ApiFile> r = new List<ApiFile>();
            foreach(var file in taskLogFile)
            {
                var fileInfo = new FileInfo(file);
                r.Add(new ApiFile()
                {
                    name=fileInfo.Name,
                    size=fileInfo.Length,
                    isDir=false
                });
            }

            return ApiResult.OK(r);
        }
        [HttpGet("getdetail")]
        public IActionResult GetLogDetail([FromQuery]string taskid, [FromQuery]string filename)
        {
            string filePath = Path.Combine(Ptilopsis.Config.Get().AppLogPath, taskid,filename);
            if (!System.IO.File.Exists(filePath))
            {
                return ApiResult.OK("");
            }
            var logContent = "";
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read,FileShare.ReadWrite))
                {
                    byte[] content = new byte[fs.Length];
                    fs.Read(content);
                    logContent = Encoding.UTF8.GetString(content);
                }
                return ApiResult.OK(logContent);
            }
            catch(Exception e)
            {
                return ApiResult.Failure(e.ToString());
            }
        }
    }
}
