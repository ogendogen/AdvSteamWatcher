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

        public SteamBot()
        {
            var configuration = SteamConfiguration.Create(b => b.WithProtocolTypes(ProtocolTypes.Tcp));
            SteamClient = new SteamClient(configuration);
            Manager = new CallbackManager(SteamClient);

            SteamUser = SteamClient.GetHandler<SteamUser>();
            SteamFriends = SteamClient.GetHandler<SteamFriends>();
        }
    }
}
