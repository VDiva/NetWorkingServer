using NetWorkingServer;
using GameData;
using GameServer.GameTool;
using GameServer.Manager;


namespace GameServer
{
    public class Message : IMessage
    {
        public void OnClientAccpet(object client)
        {
            var client1=client as Client<Message>; ;
            DebugLog.LogWarn(client1.socket.RemoteEndPoint + "链接到服务器");
            ClientManager.Instance.AddClient(client1);
            client1.SendMessageAsyn(MsgTool.Serialization(new Data { MsgType = MsgType.AllocationIdmsg, ID = client1.ID }));
        }

        public void OnDisConnectToServer(object client)
        {
            
        }

       

        public void OnMessage(byte[] data, int ID)
        {
           
            try
            {
                Data value = MsgTool.DeSerialization<Data>(data);
                if (value != null)
                {
                    PlayerData playerData = ClientManager.Instance.GetPlayerData(ID);
                    Msg msg = new Msg(value, ref playerData);
                    MessageManager.Instance.AddMessage(ref msg);
                }
            }
            catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
            }
        }

        public void OnConnectToServer(int ID)
        {
           
        }

        public void OnDisConnectToServer(int ID)
        {
            ClientManager.Instance.RemoveClient(ID);

        }
    }
}
