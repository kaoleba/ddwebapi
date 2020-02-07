using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace DDWebApi
{
    public class ConfigurationHelper
    {
        public readonly static IConfiguration Configuration;
        static ConfigurationHelper()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        public static string GetConStr(string Name)
        {
             return Configuration.GetConnectionString(Name);
        }
    }
}
