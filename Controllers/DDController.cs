using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DDWebApi.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class DDController : ControllerBase
    {
        

        [HttpGet]
        [Route("GetConfig")]
        public string GetConfig(string url)
        {
            return JsonConvert.SerializeObject(DDHelper.GetConfig(url)); ;
        }
    }
}