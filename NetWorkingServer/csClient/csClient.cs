using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetWorkingServer;
namespace csClient
{
    public class csClient
    {
        static void Main(string[] args)
        {
            NetWorking<Message> net=new NetWorking<Message>();
            DebugLogSetting setting = new DebugLogSetting();
            net.NetAsClient("127.0.0.1", 8888);
            Console.ReadKey();
        }
    }
}
