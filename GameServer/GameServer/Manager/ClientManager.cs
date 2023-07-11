using GameData;
using GameServer.GameTool;
using NetWorkingServer;
using System.Collections.Concurrent;

namespace GameServer.Manager
{
    public class ClientManager : SingletonClass<ClientManager>
    {

        public ClientManager() { }

        ConcurrentDictionary<int, PlayerData> PlayerDataDictionary;
        

        public void Init(string ip, int prot, int maxAccpet)
        {

            PlayerDataDictionary = new ConcurrentDictionary<int, PlayerData>();

            NetWorking<Message> net = new NetWorking<Message>();
            net.NetAsServer("127.0.0.1", 8888, 100);
        }


        public void AddClient(Client client)
        {
            PlayerDataDictionary.TryAdd(client.ID, new PlayerData { client=client,IsJoinLobby=false,IsJoinRoom=false,ID=client.ID});
        }

        public void RemoveClient(int ID)
        {
            GetPlayerData(ID).client.socket.Close();
            PlayerDataDictionary.TryRemove(ID, out PlayerData data);
        }


        public PlayerData GetPlayerData(int ID)
        {
            if(PlayerDataDictionary.TryGetValue(ID, out PlayerData data))
            {
                return data;
            }
            return null;
        }
       

        public void UpData()
        {
           
        }
    }
}
