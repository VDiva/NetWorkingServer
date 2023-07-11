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
            ClientManager.Instance.AddClient(client);
            client.SendMessageAsyn(MsgTool.Serialization(new Data { MsgType = MsgType.AllocationIdmsg, ID = client.ID }));
        }

        public void OnDisConnectToServer(ref Client client)
        {
            
        }

       

        public void OnMessage(byte[] data, ref Client client)
        {
            DebugLog.LogError(client.RoomID.ToString());
            try
            {
                Data value = MsgTool.DeSerialization<Data>(data);
                if (value != null)
                {
                    client.RoomID += 1;
                    //PlayerData playerData = ClientManager.Instance.GetPlayerData(ID);
                    //Msg msg = new Msg(value, ref playerData);
                    //MessageManager.Instance.AddMessage(ref msg);
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
