using GameData;
using GameServer.GameTool;
using NetWorkingServer;
using System.Collections.Concurrent;

namespace GameServer.Manager
{
    public class LobbyManager: SingletonClass<LobbyManager>
    {   

        private List<Client> clients;
        private ConcurrentQueue<Msg> MessageQueue;

        public LobbyManager()
        {
            clients = new List<Client>();
            MessageQueue=new ConcurrentQueue<Msg>();
        }

        public void AddMessage(ref Msg msg)
        {
            //DebugLog.LogWarn(msg.playerData.RoomID.ToString());
            //msg.playerData.RoomID += 1;
            MessageQueue.Enqueue(msg);
        }

        public void JoinLobby(Client client)
        {
            
            clients.Add(client);
        }

        public void BackLobby(Client client)
        {
            try
            {
                clients.Remove(client);
               
            }
            catch (Exception ex)
            {
                DebugLog.LogError("大厅中没有该玩家"+"-"+ex.Message);
            }
        }

        public int GetLobbyCount()
        {
            return clients.Count;
        }

        public bool GetIsJoinLobby(Client client)
        {
            return clients.Contains(client);
        }


        public void UpData()
        {
            while (MessageQueue.Count>0)
            {
                if(MessageQueue.TryDequeue(out Msg msg))
                {
                    MessageHandle(ref msg);
                }
            }
        }


        private void MessageHandle(ref Msg msg)
        {
            
            
        }
    }
}
