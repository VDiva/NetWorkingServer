using NetWorkingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace GameServer
{
    public class Message : IMessage
    {
        public void OnClientAccpet(object client)
        {
            var client1=client as Client<Message>; ;
            DebugLog.LogWarn(client1.socket.RemoteEndPoint + "链接到服务器");
        }

        public void OnConnectToServer(object client)
        {
            var client1 = client as Client<Message>; ;
            DebugLog.LogWarn(client1.socket.RemoteEndPoint + "链接到服务器");
        }

        public void OnDisConnectToServer(object client)
        {
            var client1 = client as Client<Message>; ;
            DebugLog.LogWarn(client1.socket.RemoteEndPoint + "断开服务器");
        }

        public void OnMessage(byte[] data, object client)
        {
            var client1 = client as Client<Message>;
            DebugLog.Log(JsonConvert.DeserializeObject<Msg>(Encoding.UTF8.GetString(data)).Context);
            DebugLog.LogWarn(data.Length.ToString());
        }
    }
}
