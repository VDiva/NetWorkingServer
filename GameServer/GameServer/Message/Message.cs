using NetWorkingServer;
using GameData;
using GameServer.GameTool;
using GameServer.Manager;


namespace GameServer
{
    public class Message : IMessage
    {
        public void OnClientAccpet(ref Client client)
        {
            
            DebugLog.LogWarn(client.socket.RemoteEndPoint + "链接到服务器");
            ClientManager.Instance.AddClient(ref client);
            client.SendMessageAsyn(MsgTool.Serialization(new Data { MsgType = MsgType.AllocationIdmsg, ID = client.ID }));
        }

        public void OnDisConnectToServer(ref Client client)
        {
            DebugLog.LogWarn(client.ID + ":断开了链接");
            ClientManager.Instance.RemoveClient(ref client);
        }

       

        public void OnMessage(byte[] data, ref Client client)
        {
            
            try
            {
                Data value = MsgTool.DeSerialization<Data>(data);
                if (value != null)
                {
                    
                    //PlayerData playerData = ClientManager.Instance.GetPlayerData(ID);
                    Msg msg = new Msg(ref value, ref client);
                    MessageManager.Instance.AddMessage(ref msg);
                }
            }
            catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
            }
        }

        public void OnConnectToServer(ref Client client)
        {
           
        }


    }
}
