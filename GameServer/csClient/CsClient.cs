using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer;
using NetWorkingServer;
namespace csClient
{
    public class CsClient
    {
        static void Main(string[] args)
        {
            DebugLogSetting debugLogSetting = new DebugLogSetting();
            NetWorking<Message> net=new NetWorking<Message>();
            var client =net.NetAsClient("127.0.0.1", 8888);
            //Console.ReadKey();


            while (true)
            {
                Thread.Sleep(1000);
                client.Send(new Msg { Context="你好dddddddddddddddddddddddddddddddddddddddddddddddddddddddddd"});
                Console.WriteLine("发送消息");
            }

        }
    }
}
