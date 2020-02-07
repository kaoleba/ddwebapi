using Dapper.Contrib.Extensions;

namespace DDWebApi
{
    public class ResponseResult
    {
        public ResponseResult()
        {
            this.errorMsg = "";
        }
        public object content { get; set; }
        public string errorMsg { get; set; }
    }
}
