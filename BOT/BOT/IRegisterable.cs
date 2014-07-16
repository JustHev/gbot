using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBot
{
    interface IRegisterable
    {
        string network;
        string name;
        Object Adress;
        Dictionary<string, LuaCommand> commands;

        public void SendMessage(string message);
    }
}
