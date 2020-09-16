using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using PtilopsisServer.ApiModel;

namespace PtilopsisServer.Controller
{
    public class LoginUser
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginUser user)
        {
            if (user.userName == Config.Get().AdminName && user.password == Config.Get().AdminPassword)
            {
                var authClaims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub,user.userName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
                IdentityModelEventSource.ShowPII = true;
                //签名秘钥 可以放到json文件中
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Get().JwtSignKey));

                var token = new JwtSecurityToken(
                       expires: DateTime.Now.AddHours(2),
                       claims: authClaims,
                       signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                       );
                return ApiResult.OK(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return ApiResult.Failure("用户名或密码错误");
        }
    }
}
