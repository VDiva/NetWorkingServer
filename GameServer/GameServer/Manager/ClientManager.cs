using GameData;
using GameServer.GameTool;
using NetWorkingServer;
using System.Collections.Concurrent;

namespace GameServer.Manager
{
    public class ClientManager : SingletonClass<ClientManager>
    {

        public ClientManager() { }

        ConcurrentDictionary<int, Client> PlayerDataDictionary;
        

        public void Init(string ip, int prot, int maxAccpet)
        {

            PlayerDataDictionary = new ConcurrentDictionary<int, Client>();

            NetWorking<Message> net = new NetWorking<Message>();
            net.NetAsServer("127.0.0.1", 8888, 100);
        }


        public void AddClient(ref Client client)
        {
            PlayerDataDictionary.TryAdd(client.ID, client);
        }

        public void RemoveClient(ref Client client)
        {   
            if(client.IsJoinRoom) RoomManager.Instance.BlackRoom(ref client);
            PlayerDataDictionary.TryRemove(client.ID, out Client c);

        }


       
       

        public void UpData()
        {
           
        }
    }
}
