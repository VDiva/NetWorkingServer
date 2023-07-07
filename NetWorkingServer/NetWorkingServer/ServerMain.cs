using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkingServer
{
    public class ServerMain
    {
        public static void Main(string[] args)
        {       
            DebugLogSetting debug=new DebugLogSetting();
            
            NetWorking<IMessage> net=new NetWorking<IMessage>();
            net.NetAsServer("127.0.0.1", 8888, 1000);
            Console.ReadKey();
        }
    }
}
