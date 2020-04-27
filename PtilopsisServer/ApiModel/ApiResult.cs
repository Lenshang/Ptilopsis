using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PtilopsisServer.ApiModel
{
    public class ApiResult : IActionResult
    {
        public ApiContext result { get; set; }
        public int codeStatus { get; set; }
        public Dictionary<string,string> headers { get; set; }
        public ApiResult(object data, bool success = true, string message = "",int codeStatus=200)
        {
            this.result = new ApiContext()
            {
                data = data,
                success = success,
                message = message,
                dateTime=DateTime.Now
            };
            this.codeStatus = codeStatus;

            this.headers = new Dictionary<string, string>();
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = this.codeStatus;
            foreach(var key in this.headers.Keys)
            {
                response.Headers.Add(key, this.headers[key]);
            }
            int[] codes = { 302, 500, 403, 404, 502, 401 };
            if(codes.Contains(this.codeStatus))
            {

            }
            else
            {
                await response.WriteAsync(JsonConvert.SerializeObject(this.result));
            }
        }
        /// <summary>
        /// 返回一个OK的结果
        /// </summary>
        /// <param name="data">包含的数据</param>
        /// <returns></returns>
        public static ApiResult OK(object data = null)
        {
            return new ApiResult(data);
        }
        /// <summary>
        /// 返回一个失败的结果
        /// </summary>
        /// <param name="message">失败信息</param>
        /// <returns></returns>
        public static ApiResult Failure(string message="")
        {
            return new ApiResult(null,false,message);
        }
        /// <summary>
        /// 返回一个重定向结果
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static ApiResult Redirect(string location)
        {
            var r= new ApiResult(null, false, "",302);
            r.headers.Add("location", location);
            return r;
        }
        public static ApiResult ServerError()
        {
            return new ApiResult(null, false, "", 500);
        }
        public static ApiResult BadGateway()
        {
            return new ApiResult(null, false, "", 502);
        }
        public static ApiResult NotFound()
        {
            return new ApiResult(null, false, "", 404);
        }
        public static ApiResult Forbidden()
        {
            return new ApiResult(null, false, "", 403);
        }
    }
}
