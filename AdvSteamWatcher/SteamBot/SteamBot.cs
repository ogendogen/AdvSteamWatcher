using System;
using SteamKit2;

namespace Steam
{
    public class SteamBot
    {
        public SteamClient SteamClient { get; set; }
        public CallbackManager Manager { get; set; }
        public SteamUser SteamUser { get; set; }
        public SteamFriends SteamFriends { get; set; }
    }
}
