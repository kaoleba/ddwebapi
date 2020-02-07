using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDWebApi
{
    public class LogHelper
    {
        static ILog log = LogManager.GetLogger(Startup.repository.Name, "NETCorelog4net");

        public static void Info(string mes)
        {
            log.Info(mes);
        }

        public static void Warn(string mes)
        {
            log.Warn(mes);
        }

        public static void Debug(string mes)
        {
            log.Debug(mes);
        }

        public static void Error(string mes)
        {
            log.Error(mes);
        }
    }
}
