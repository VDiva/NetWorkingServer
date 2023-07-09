using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetWorkingServer;
namespace GameServer
{
    public class ServerMain
    {
        static void Main(string[] args)
        {
            DebugLogSetting logSetting = new DebugLogSetting();
            ClientManager.Instance.Init("127.0.0.1", 8888, 100);
            Console.ReadKey();

           
        }
        
    }

    public class Msg
    {
        public string Context;
    }
}
