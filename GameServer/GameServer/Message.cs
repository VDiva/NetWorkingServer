using NetWorkingServer;
using GameData;
using Newtonsoft.Json;
using System.Text;

namespace GameServer
{
    public class Message : IMessage
    {
        public void OnClientAccpet(object client)
        {
            var client1=client as Client<Message>; ;
            DebugLog.LogWarn(client1.socket.RemoteEndPoint + "链接到服务器");
            ClientManager.Instance.AddClient(client1);
            client1.SetSendMessageBuffer(GameTool.Serialization(new Data { MsgType = MsgType.AllocationIdmsg, ID = client1.ID }));
            client1.SendMessage();

            ClientManager.Instance.JoinSelfMsg(client1);
            ClientManager.Instance.JoinOtherMsg(client1);
        }

       

        public void OnDisConnectToServer(object client)
        {
            var client1 = client as Client<Message>; ;
            DebugLog.LogWarn(client1.socket.RemoteEndPoint + "断开服务器");
            client1.socket.Close();
            ClientManager.Instance.RemoveClient(client1.ID);

        }

        public void OnMessage(byte[] data, object client)
        {
            var client1 = client as Client<Message>;
            

            try
            {
                Data value = GameTool.DeSerialization<Data>(data);
                if (value != null)
                {
                    MessageHandle.Handle(value);
                }
            }
            catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
            }


        }



        public void OnConnectToServer(object client)
        {

        }
    }
}
