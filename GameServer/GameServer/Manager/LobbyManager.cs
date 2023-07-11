using GameData;
using GameServer.GameTool;
using NetWorkingServer;
using System.Collections.Concurrent;

namespace GameServer.Manager
{
    public class LobbyManager: SingletonClass<LobbyManager>
    {   

        private List<Client<Message>> clients;
        private ConcurrentQueue<Msg> MessageQueue;

        public LobbyManager()
        {
            clients = new List<Client<Message>>();
            MessageQueue=new ConcurrentQueue<Msg>();
        }

        public void AddMessage(ref Msg msg)
        {
            //DebugLog.LogWarn(msg.playerData.RoomID.ToString());
            //msg.playerData.RoomID += 1;
            MessageQueue.Enqueue(msg);
        }

        public void JoinLobby(Client<Message> client)
        {
            
            clients.Add(client);
        }

        public void BackLobby(Client<Message> client)
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

        public bool GetIsJoinLobby(Client<Message> client)
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
            
            switch (msg.data.MsgType)
            {
                case MsgType.AllocationIdmsg:
                    break;
                case MsgType.StringMsg:
                    break;
                case MsgType.AnimMsg:
                    break;
                case MsgType.TransformMsg:
                    break;
                case MsgType.JoinRoomMsg:
                    RoomManager.Instance.AddMessage(ref msg);
                    break;
                case MsgType.JoinRandomRoomMsg:
                    RoomManager.Instance.AddMessage(ref msg);
                    break;
                case MsgType.CreateRoomMsg:
                    RoomManager.Instance.AddMessage(ref msg);
                    break;
            }
        }
    }
}
