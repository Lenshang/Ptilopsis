using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ptilopsis.PtiDB;
using Ptilopsis.PtiEvent;
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
    [Authorize]
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
            var result = (bool)_t.EventResult;
            if (result)
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure();
            }
        }
        /// <summary>
        /// 获得所有正在在内存中的任务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getrunning")]
        public IActionResult GetAllRunning([FromQuery]ApiTaskQuery query)
        {
            var r = TaskManager.Get().GetAllTasksSync();
            return ApiResult.OK(r);
        }
        
        [HttpGet("kill")]
        public IActionResult KillTask(string id)
        {
            var r=TaskManager.Get().KillTaskById(id);
            if (r!=null && r.Value==true)
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure();
            }
        }
        /// <summary>
        /// 启动一个Task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("enable")]
        public IActionResult Enable([FromQuery]string id)
        {
            var r = TaskManager.Get().EnableTask(id);
            if (r)
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure();
            }
        }
        /// <summary>
        /// 禁用一个Task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("disable")]
        public IActionResult Disable([FromQuery]string id)
        {
            var r = TaskManager.Get().DisableTask(id);
            if (r)
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure();
            }
        }

        [HttpGet("delete")]
        public IActionResult Remove([FromQuery] string id)
        {
            var pe = TaskManager.Get().RemoveTaskById(id);
            var r = EventManager.Get().WaitEvent<string>(pe);
            if (r==null)
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure(r);
            }
        }
    }
}
