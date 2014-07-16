using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBot
{
    interface IRegisteredUser:IRegisterable
    {
        int GlobalPerms;
    }
}
