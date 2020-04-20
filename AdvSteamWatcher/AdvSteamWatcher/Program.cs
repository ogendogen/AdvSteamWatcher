using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvSteamWatcher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string configPath = "config.json";
            if (args.Length > 0)
            {
                configPath = args[0];
            }

            BotManager botManager = new BotManager(configPath);
            botManager.Launch();
        }
    }
}
