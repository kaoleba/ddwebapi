using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DDWebApi.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class DDController : ControllerBase
    {
         /// <summary>
         /// 钉钉获取鉴权
         /// </summary>
         /// <param name="url"></param>
         /// <returns></returns>
        [HttpGet]
        [Route("GetConfig")]
        [EnableCors("any")]
        public string GetConfig(string url)
        {
            return JsonConvert.SerializeObject(DDHelper.GetConfig(url)); 
        }

        /// <summary>
        /// 根据Code换取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserInfo")]
        [EnableCors("any")]
        public string GetUserInfo(string code)
        {
            return JsonConvert.SerializeObject(DDHelper.GetUserInfo(code)); 
        }


    }
}