using Microsoft.AspNetCore.Mvc;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiTask;
using Ptilopsis.Utils;
using PtilopsisServer.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PtilopsisServer.Controller
{
    public class ApiTaskData
    {
        public string appId { get; set; }
        public string runArgs { get; set; }
        public string name { get; set; }
        public string schedule { get; set; }
    }
    public class ApiTaskQuery
    {
        public int skip { get; set; }
        public int take { get; set; } = 10;
    }
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet("getall")]
        public IActionResult GetAll([FromQuery]ApiTaskQuery query)
        {
            return ApiResult.OK(new {
                Array= DBManager.Get().GetAllTasks(query.skip,query.take),
                Total = DBManager.Get().GetTaskCount()
            });
        }

        [HttpGet("get")]
        public IActionResult Get([FromQuery]string id)
        {
            var r = DBManager.Get().GetTaskById(id);
            if (r == null)
            {
                return ApiResult.Failure();
            }
            else
            {
                return ApiResult.OK(r);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]ApiTaskData data)
        {
            PtiTasker task = new PtiTasker()
            {
                ApplicationId = data.appId,
                RunArgs = data.runArgs,
                Schedule=data.schedule,
                TaskName = data.name,
                _id = MD5Helper.getMd5Hash(data.name)
            };

            var _t = TaskManager.Get().AddTask(task);
            while (!_t.IsExcuted)
            {
                await Task.Delay(100);
            }
            var result = (bool)_t.EventArgs;
            if (result)
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure();
            }
        }
    }
}
