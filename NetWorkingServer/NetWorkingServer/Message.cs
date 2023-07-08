using Douyin;
using System.Text.Json;

namespace NetWorkingServer
{
    public class Message : IMessage
    {
        public void OnClientAccpet(object client)
        {
            var client1 = client as Client<Message>;
            if (client1 != null)
            {

                DebugLog.LogWarn("玩家" + client1.socket.RemoteEndPoint + "链接到服务器");
            }
        }

        public void OnConnectToServer(object client)
        {
            
            var client1 = client as Client<Message>;
            if (client1 != null)
            {

                DebugLog.LogWarn(client1.socket.RemoteEndPoint + "链接到服务器");
            }
        }

        public void OnDisConnectToServer(object client)
        {
            var client1 = client as Client<Message>;
            if (client1 != null)
            {

                DebugLog.LogWarn("玩家" + client1.socket.RemoteEndPoint + "断开到服务器");
                client1.socket.Close();
            }
        }

        public void OnMessage(byte[] data, object client)
        {
            
                Console.WriteLine("收到信息");
                Data d = JsonSerializer.Deserialize<Data>(data);
                DebugLog.Log(d.Content);
                
        }
    }
}
