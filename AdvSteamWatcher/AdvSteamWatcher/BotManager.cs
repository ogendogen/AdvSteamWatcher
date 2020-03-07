using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AdvWatcher;
using Steam;
using SteamKit2;

namespace AdvSteamWatcher
{
    public class BotManager
    {
        public SteamBot SteamBot { get; set; }
        public List<Friend> AdvReceivers { get; set; }
        public Config Config { get; set; }

        public BotManager(string configPath)
        {
            try
            {
                LoadConfigFile(configPath);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public void Launch()
        {
            StartSteamBot();
            StartSiteWatcher();
        }

        private void HandleException(Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
            Console.WriteLine("More details in log file");

            File.WriteAllText($"BotManagerError{ DateTime.Now.ToString("yyyyMMddd") }.log", $"{ DateTime.Now.ToString("yyyyMMddd") }  {e.Message}\r\n{e.StackTrace}");
        }

        private void LoadConfigFile(string configPath)
        {
            Config = ConfigParser.ParseConfig(configPath);

            AdvReceivers = new List<Friend>();
            foreach (var rawAdvReceiver in Config.AdvMessageReceivers)
            {
                Friend friend = new Friend()
                {
                    SteamID = new SteamID(rawAdvReceiver, EUniverse.Public),
                    IsReceivingAdvInfo = true
                };
                AdvReceivers.Add(friend);
            }
            Console.WriteLine("Config loaded correctly");
        }

        private void StartSiteWatcher()
        {
            SiteWatcher siteWatcher = new SiteWatcher(Config.WatchedSite, Config.Interval, Config.WantedText);
            siteWatcher.OnAdvAvaiable += SiteWatcher_OnAdvAvaiable;
            siteWatcher.StartWatcher();
            Console.WriteLine("Site watcher working");
        }

        private void StartSteamBot()
        {
            SteamBot = new SteamBot(Config.Login, Config.Password);
            SteamBot.AddBasicCommands(Config.BasicCommands);
            SteamBot.SetWelcomeMessage(Config.WelcomeMessage);
            Console.WriteLine("SteamBot working");
        }

        private void SiteWatcher_OnAdvAvaiable(object sender, EventArgs e)
        {
            foreach (var receiver in AdvReceivers)
            {
                SteamBot.SendMessage(receiver, $"Reklama dostępna! {Config.WatchedSite}");
            }
        }
    }
}
