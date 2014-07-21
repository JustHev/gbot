using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBot
{
    interface IRegisterable
    {
        string network {get; set;}
        string name {get; set;}
        Object Adress { get; set; }
        Dictionary<string, LuaCommand> commands { get; set; }

        void SendMessage(string message);
    }
}
