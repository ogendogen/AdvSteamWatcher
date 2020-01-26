using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Steam
{
    public static class ConfigParser
    {
        public static Config ParseConfig(string path)
        {
            try
            {
                string configContent = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<Config>(configContent);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
