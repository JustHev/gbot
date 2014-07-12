using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOT
{
    interface IProtocolSocket
    {
        string Network { get; set; }
        string Username { get; set; }
        string Password { set; }

        void Run();
        void Stop();
        /*void Connect();
        void Disconnect();
        //void SendMessage();


        void HandleCallbacks();*/
    }
}
