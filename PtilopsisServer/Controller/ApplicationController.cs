using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ptilopsis.PtiApplication;
using Ptilopsis.Utils;
using PtilopsisServer.ApiModel;

namespace PtilopsisServer.Controller
{
    public class ApiAppData
    {
        public IFormFile file { get; set; }
        public string name { get; set; }
        public string runCmd { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController: ControllerBase
    {
        [HttpGet("getall")]
        public IActionResult getAll()
        {
            return ApiResult.OK(AppManager.Get().GetAllApps());
        }

        [HttpPost("add")]
        public async Task<IActionResult> add([FromForm]ApiAppData data)
        {
            var fileExName = data.file.FileName.Substring(data.file.FileName.LastIndexOf("."));
            var fileName = Guid.NewGuid().ToString("N") + fileExName;
            var saveFile = Path.Combine(Ptilopsis.Config.Get().AppZipPath,fileName);
            using (FileStream fs = System.IO.File.Create(saveFile))
            {
                await data.file.CopyToAsync(fs);
                fs.Flush();
            }

            //储存对象
            PtiApp app = new PtiApp()
            {
                Name = data.name,
                ZipFile = fileName,
                DefaultRunCmd = data.runCmd,
                CreateDate=DateTime.Now
            };
            app.Id = MD5Helper.getMd5Hash(app.Name);

            if (AppManager.Get().AddApp(app))
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure("程序添加失败");
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> update([FromForm]ApiAppData data)
        {
            var fileExName = data.file.FileName.Substring(data.file.FileName.LastIndexOf("."));
            var fileName = Guid.NewGuid().ToString("N") + fileExName;
            var saveFile = Path.Combine(Ptilopsis.Config.Get().AppZipPath, fileName);
            using (FileStream fs = System.IO.File.Create(saveFile))
            {
                await data.file.CopyToAsync(fs);
                fs.Flush();
            }

            //储存对象
            PtiApp app = new PtiApp()
            {
                Name = data.name,
                ZipFile = fileName,
                DefaultRunCmd = data.runCmd
            };
            app.Id = MD5Helper.getMd5Hash(app.Name);

            if (AppManager.Get().AddApp(app))
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure("程序添加失败");
            }
        }

        [HttpDelete("delete")]
        public IActionResult delete([FromQuery]string id)
        {
            if (AppManager.Get().DeleteApp(id))
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure("删除失败");
            }
        }
    }
}