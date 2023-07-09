using GameData;
using NetWorkingServer;
using System.Collections.Concurrent;

namespace GameServer
{
    public class ClientManager : SingletonClass<ClientManager>
    {

        public ClientManager() {}

        ConcurrentDictionary<int, Client<Message>> IDFindClient;

       
        public void Init(string ip, int prot, int maxAccpet)
        {

            IDFindClient = new ConcurrentDictionary<int, Client<Message>>();

            NetWorking<Message> net = new NetWorking<Message>();
            net.NetAsServer("127.0.0.1", 8888, 100);

        }


        public  void AddClient(Client<Message> client)
        {
            IDFindClient.TryAdd(client.ID, client);
        }

        public  void RemoveClient(int ID)
        {
            IDFindClient.TryRemove(ID,out Client<Message> client);
        }

        public void JoinSelfMsg(Client<Message> client)
        {
            Data data = new Data
            {
                MsgType=MsgType.JoinMsg,
                JoinData=new JoinData
                {
                    ID = client.ID,
                }
            };
            foreach (var msg in IDFindClient.Values)
            {
               if(msg.ID != client.ID)
                {
                    msg.SetSendMessageBuffer(GameTool.Serialization(data));
                    msg.SendMessage();
                }
            }
        }

        public void JoinOtherMsg(Client<Message> client)
        {
            Data data = new Data
            {
                MsgType = MsgType.JoinMsg,
                JoinData = new JoinData
                {
                    ID = client.ID,
                }
            };

            foreach(var msg in IDFindClient.Values)
            {
                if (msg.ID != client.ID)
                {
                    data.ID = msg.ID;
                    client.SetSendMessageBuffer(GameTool.Serialization(data));
                    client.SendMessage();
                }
            }

        }

        
    }
}
