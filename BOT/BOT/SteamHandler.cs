using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace BOT
{
    class SteamHandler:IProtocolSocket
    {
        public SteamClient steamClient;
        public CallbackManager manager;
        public SteamUser steamUser;
        public SteamFriends steamFriends;

        string user;
        string password;
        private Boolean keepGoing = true;

        public SteamHandler()
        {

        }
        public SteamHandler(string usr, string pass)
        {
            user = usr;
            password = pass;
        }
        public void Connect()
        {
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();
            steamFriends = steamClient.GetHandler<SteamFriends>();

            //registering callbacks
            new Callback<SteamClient.ConnectedCallback>(OnConnected, manager);
            new Callback<SteamClient.DisconnectedCallback>(OnDisconnected, manager);
            new Callback<SteamUser.LoggedOnCallback>(OnLoggedOn, manager);
            //new Callback<SteamUser.LoggedOffCallback>(OnLoggedOff, manager);
            new Callback<SteamUser.AccountInfoCallback>(OnAccountInfo, manager);
            /*new Callback<SteamFriends.ChatMsgCallback>(OnGroupChatMessage, manager);
            new Callback<SteamFriends.ChatInviteCallback>(OnChatInvite, manager);
            new Callback<SteamFriends.FriendMsgCallback>(OnFriendMsg, manager);*/
            new Callback<SteamFriends.FriendsListCallback>(OnFriendsList, manager);
            //new Callback<SteamFriends.PersonaStateCallback>(OnPersonaState, manager);
            //new Callback<SteamFriends.FriendAddedCallback>(OnFriendAdded, manager);
            //new Callback<SteamFriends.ChatEnterCallback>(OnChatEnter, manager);
            steamClient.Connect();

        }
        public void Run()
        {
            Connect();
            while(keepGoing)
            {
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));

            }
            Disconnect();
        
        }
        public void Stop()
        {
            keepGoing = false;
        }

        private void OnAccountInfo(SteamUser.AccountInfoCallback obj)
        {
            steamFriends.SetPersonaState(EPersonaState.Online);
        }
        public void Disconnect()
        {
            steamUser.LogOff();
        }
        private void OnConnected(SteamClient.ConnectedCallback obj)
        {
            if (obj.Result != EResult.OK)
            {
                Console.WriteLine("ERR: Unable to connect to Steam: {0}", obj.Result);
                //isRunning = false;
                return;
            }
            Console.WriteLine("OK: Connected to Steam! Logging in '{0}'...", user);
            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = user,
                Password = password,
            });
        }
        private void OnDisconnected(SteamClient.DisconnectedCallback obj)
        {
            Console.WriteLine("ERR: Disconnected from Steam");
            //SaveData();
        }
        void SendMessage(SteamID id, string message) //TODO: Replace SteamID
        {
            if (id.AccountType==EAccountType.Individual)
            {
                steamFriends.SendChatMessage(id, EChatEntryType.ChatMsg, message);
                return;
            }
            if (id.AccountType==EAccountType.Clan||id.AccountType==EAccountType.Chat)
            {
                steamFriends.SendChatRoomMessage(id, EChatEntryType.ChatMsg, message);
                return;
            }
            Console.Write("Couldn't send message '{0}' to {1}", message, id);
        }
        private void OnLoggedOn(SteamUser.LoggedOnCallback obj)
        {
            if (obj.Result != EResult.OK)
            {
                if (obj.Result == EResult.AccountLogonDenied)
                {
                    Console.WriteLine("ERR: Unable to logon: SteamGuard protected account.");
                    //isRunning = false;
                    return;
                }
                Console.WriteLine("ERR: Unable to logon: {0} / {1}", obj.Result, obj.ExtendedResult);
                //isRunning = false;
                return;
            }
            Console.WriteLine("OK: Successfully logged in");
        }
        private void OnFriendsList(SteamFriends.FriendsListCallback obj)
        {
            Console.WriteLine("OK: Received Friends list, accepting pending invites, current Friend List Size: {0}", steamFriends.GetFriendCount());

            foreach (var friend in obj.FriendList)
            {
                if (friend.Relationship == EFriendRelationship.RequestRecipient)
                {
                    steamFriends.AddFriend(friend.SteamID);
                    //Console.WriteLine("OK: Added {0} to friend list", friend.SteamID.Render());
                }
                if (friend.SteamID.IsClanAccount)
                {
                    steamFriends.JoinChat(friend.SteamID);
                    //gch = new GroupChatHandler(friend.SteamID);
                }
                //Console.WriteLine("OK: Friend: {0}, Status: {1}", friend.SteamID.Render(), steamFriends.GetFriendPersonaState(friend.SteamID).ToString());
            }
            // string[] lines=System.IO.File.ReadAllLines(@"C:\HevBot\SuperUsers.txt");
            //foreach (string line in lines)
            //{
            // Superusers[Superusers.Length] = steamFriends.GetFriendByIndex(line);
            //}
        }
        public void HandleCallbacks()
        {
            manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
        }

        public string Network
        {
            get
            {
                return "STEAM";
            }
            set
            {
                Console.Write("This value cannot be set, it is locked to STEAM");
                return;
            }
        }

        public string Username
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }

        public string Password
        {
            set { password = value; }
        }
    }
}
