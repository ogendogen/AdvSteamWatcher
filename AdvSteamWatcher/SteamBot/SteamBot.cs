using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
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
        public Dictionary<string, string> BasicCommands { get; private set; }
        public string WelcomeMessage 
        { 
            get
            {
                StringBuilder finalMsg = new StringBuilder();
                finalMsg.Append($"{_welcomeMessage}");

                foreach (var command in BasicCommands)
                {
                    finalMsg.Append($"{command.Key}{Environment.NewLine}");
                }

                return finalMsg.ToString();
            }
            private set
            {
                _welcomeMessage = value;
            }
        }
        public HashSet<Friend> Chatters { get; set; }
        public string CommandNotFoundMessage { get; private set; }

        private string _login;
        private string _password;
        private string _welcomeMessage;

        public SteamBot(string login, string password)
        {
            var configuration = SteamConfiguration.Create(b => b.WithProtocolTypes(ProtocolTypes.Tcp));
            SteamClient = new SteamClient(configuration);
            Manager = new CallbackManager(SteamClient);
            Chatters = new HashSet<Friend>();

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

            _login = login;
            _password = password;

            IsRunning = true;
            Console.WriteLine("Connecting to Steam");

            SteamClient.Connect();

            KeepBotAlive();
        }

        public void SendMessage(Friend friend, string message)
        {
            SteamFriends.SendChatMessage(friend.SteamID, EChatEntryType.ChatMsg, message);
        }

        private void KeepBotAlive()
        {
            new Thread(() =>
            {
                while (IsRunning)
                {
                    Manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
                }
            }).Start();
        }

        public void SetWelcomeMessage(string welcomeMessage)
        {
            WelcomeMessage = welcomeMessage;
        }

        public void SetCommandNotFoundMessage(string commandNotFoundMessage)
        {
            CommandNotFoundMessage = commandNotFoundMessage;
        }

        public void AddBasicCommands(Dictionary<string, string> basicCommands)
        {
            BasicCommands = basicCommands;
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

                Friend currentChatter = Chatters.FirstOrDefault(chatter => chatter.SteamID.AccountID == sender.AccountID);

                if (currentChatter == null)
                {
                    currentChatter = new Friend()
                    {
                        WelcomedDate = DateTime.Now,
                        ChatState = ChatState.NoInteraction,
                        SteamID = sender,
                        IsReceivingAdvInfo = false
                    };

                    Chatters.Add(currentChatter);
                }

                TimeSpan timeSpan = DateTime.Now - currentChatter.WelcomedDate;
                if (timeSpan.Minutes >= 5)
                {
                    currentChatter.ChatState = ChatState.NoInteraction;
                }
                
                string responseMessage = String.Empty;
                if (BasicCommands.ContainsKey(callback.Message))
                {
                    responseMessage = BasicCommands[callback.Message];
                    currentChatter.ChatState = ChatState.Welcomed;
                }
                else if (currentChatter.ChatState == ChatState.NoInteraction)
                {
                    currentChatter.ChatState = ChatState.Welcomed;
                    responseMessage = WelcomeMessage;
                }
                else if (currentChatter.ChatState == ChatState.Welcomed)
                {
                    responseMessage = CommandNotFoundMessage;
                }
                
                SteamFriends.SendChatMessage(sender, EChatEntryType.ChatMsg, responseMessage);

                Console.WriteLine($"{SteamFriends.GetFriendPersonaName(sender)}: {callback.Message}");
                Console.WriteLine($"BOT: {responseMessage}");
            }
        }
    }
}
