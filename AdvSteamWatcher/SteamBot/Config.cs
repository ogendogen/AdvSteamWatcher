using System;
using System.Collections.Generic;
using System.Text;

namespace Steam
{
    public class Config
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public List<string> AdvMessageReceivers { get; set; }
        public string WatchedSite { get; set; }
        public string WantedText { get; set; }
        public double Interval { get; set; }
        public Dictionary<string, string> BasicCommands { get; set; }
        public string WelcomeMessage { get; set; }

        public Config()
        {
            AdvMessageReceivers = new List<string>();
            BasicCommands = new Dictionary<string, string>();
        }
    }
}
