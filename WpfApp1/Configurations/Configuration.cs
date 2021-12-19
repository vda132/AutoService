using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Configurations
{
    class Configuration
    {
        public IConfiguration Config
        {
            get;
            private set;
        }

        private static Configuration instance = null;

        private Configuration()
        {
            Config = new ConfigurationBuilder().AddJsonFile("config.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }
        public static Configuration GetConfiguration()
        {
            if (instance == null)
            {
                instance = new Configuration();
            }
            return instance;
        }
    }
}
