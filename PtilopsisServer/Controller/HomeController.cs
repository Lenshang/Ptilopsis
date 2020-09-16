using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PtilopsisServer.ApiModel;

namespace PtilopsisServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpGet("get")]
        public IActionResult GetHome()
        {
            return ApiResult.OK("Ptilopsis Beta");
        }
    }
}
