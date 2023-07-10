using GameData;
using GameServer.GameTool;
using NetWorkingServer;


namespace GameServer.Manager
{
    public class LobbyManager: SingletonClass<LobbyManager>
    {
        private List<Client<Message>> clients;

        public LobbyManager()
        {
            clients = new List<Client<Message>>();
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

        public void OnLobbyMessage(Data data)
        {

        }

        public void UpData()
        {

        }
    }
}
