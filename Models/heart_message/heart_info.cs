using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace DDWebApi
{
    public class FlowInfo
    {
        public DeptInfo deptInfo { get; set; }
        public List<heart_flow> flowlist { get; set; }
    }

    public class DeptInfo
    {
        public string Name { get; set; }
        public string UserCode { get; set; }
        public string Dept { get; set; }
        public string DingTalk { get; set; }
        public string Phone { get; set; }
    }

    public class UserInfo
    {
        public UserInfo(string Code, string Name)
        {
            this.Code = Code;
            this.Name = Name;
        }

        public string Code { get; set; }
        public string Name { get; set; }
    }
}
