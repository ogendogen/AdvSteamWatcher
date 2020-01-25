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
        public bool IsRunning { get; set; } = false;

        private string _login;
        private string _password;

        public SteamBot()
        {
            var configuration = SteamConfiguration.Create(b => b.WithProtocolTypes(ProtocolTypes.Tcp));
            SteamClient = new SteamClient(configuration);
            Manager = new CallbackManager(SteamClient);

            SteamUser = SteamClient.GetHandler<SteamUser>();
            SteamFriends = SteamClient.GetHandler<SteamFriends>();

            Manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            Manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            Manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            Manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            Manager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);
            Manager.Subscribe<SteamFriends.FriendsListCallback>(OnFriendsList);
            Manager.Subscribe<SteamFriends.PersonaStateCallback>(OnPersonaState);

            Manager.Subscribe<SteamFriends.FriendAddedCallback>(OnFriendAdded);
            Manager.Subscribe<SteamFriends.FriendMsgCallback>(OnFriendMsg);
        }

        public void Login(string login, string password)
        {
            _login = login;
            _password = password;

            SteamClient.Connect();
        }

        private void OnConnected(SteamClient.ConnectedCallback callback)
        {
            Console.WriteLine($"Connected to Steam! Logging in {_login}...");

            SteamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = _login,
                Password = _password
            });
        }

        private void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine("Disconnected from Steam");
            IsRunning = false;
        }

        private void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback.Result != EResult.OK)
            {
                if (callback.Result == EResult.AccountLogonDenied)
                {
                    Console.WriteLine("Unable to logon to Steam: This account is SteamGuard protected.");

                    IsRunning = false;
                    return;
                }

                Console.WriteLine($"Unable to logon to Steam: {callback.Result} / {callback.ExtendedResult}");

                IsRunning = false;
                return;
            }

            Console.WriteLine("Successfully logged on!");
            IsRunning = true;
        }

        private void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            Console.WriteLine($"Logged off of Steam: {callback.Result}");
            IsRunning = false;
        }

        private void OnAccountInfo(SteamUser.AccountInfoCallback callback)
        {
            SteamFriends.SetPersonaState(EPersonaState.Online);
        }

        private void OnFriendsList(SteamFriends.FriendsListCallback callback)
        {
            Console.WriteLine($"We have {SteamFriends.GetFriendCount()} friends");

            foreach (var friend in callback.FriendList)
            {
                SteamID steamIdFriend = friend.SteamID;
                if (friend.Relationship == EFriendRelationship.RequestRecipient)
                {
                    SteamFriends.AddFriend(steamIdFriend);
                }
                Console.WriteLine($"Friend: {steamIdFriend.Render()}");
            }
        }

        private void OnPersonaState(SteamFriends.PersonaStateCallback callback)
        {
            Console.WriteLine($"State change: {callback.Name}");
        }

        private void OnFriendAdded(SteamFriends.FriendAddedCallback callback)
        {
            Console.WriteLine($"{callback.PersonaName} is now a friend");
            var sender = callback.SteamID;
            SteamFriends.SendChatMessage(sender, EChatEntryType.ChatMsg, "Witaj, jesteś teraz moim znajomym :)");
        }

        private void OnFriendMsg(SteamFriends.FriendMsgCallback callback)
        {
            if (callback.EntryType == EChatEntryType.ChatMsg)
            {
                var sender = callback.Sender;
                SteamFriends.SendChatMessage(sender, EChatEntryType.ChatMsg, "Hello");
            }
        }
    }
}
