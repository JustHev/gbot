using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BOT
{
    class Program
    {
        static Dictionary<string, IProtocolSocket> sockets=new Dictionary<string,IProtocolSocket>();
        static List<Thread> threads=new List<Thread>();
        static void Main(string[] args)
        {
            //Todo: split off into separate thread :IProtocolSocket?
            while(true)
            {
                string line = Console.ReadLine();
                if (line=="dc")
                {
                    foreach (IProtocolSocket i in sockets.Values)
                    {
                        i.Stop();
                    }
                }
                if (line=="connect")
                {
                    AddBot();
                }
                //Console.WriteLine("You wrote: {0}", line);
            }
        }
        static void AddBot()
        {

            Console.WriteLine("Network type (IRC/STEAM)");
            string network = Console.ReadLine();

            Console.WriteLine("Username");
            string user = Console.ReadLine();
            Console.WriteLine("Password");
            string pass = Console.ReadLine();
            if (network == "STEAM")
            {
                try
                {
                    sockets.Add("STEAM:" + user, new SteamHandler(user, pass));
                }
                catch (System.ArgumentException)
                {
                    Console.WriteLine("This user is already logged in");
                }
            }
            else
            {
                Console.WriteLine("Network type invalid");
                return;
            }
            threads.Add(new Thread(new ThreadStart(sockets[network + ":" + user].Run)));
            threads.Last<Thread>().Start();
        }
    }
}
