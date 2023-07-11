using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

using Newtonsoft.Json;
namespace NetWorkingServer
{
    public class NetWorking<T> where T:IMessage , new()
    {
        Socket socket;
        SocketAsyncEventArgs saeaAccpet;
        SocketAsyncEventArgs saeaReceive;
        SocketAsyncEventArgs saeaSend;
        SocketAsyncEventArgs saeaConnect;

        ConcurrentDictionary<int, Client> clients;
        T IMsg;
        int Index = 0;
        
        public NetWorking()
        {
            IMsg = new T();
            saeaAccpet = new SocketAsyncEventArgs();
            saeaReceive = new SocketAsyncEventArgs();
            saeaSend = new SocketAsyncEventArgs();
            clients = new ConcurrentDictionary<int, Client>();
        }

        private Client GetClient(int ID)
        {
            if(clients.TryGetValue(ID,out Client client))
            {
                return client;
            }
            return null;
        }

        public void NetAsServer(string IP,int Port,int Num)
        {
            
            saeaAccpet = new SocketAsyncEventArgs();
            saeaAccpet.Completed += new EventHandler<SocketAsyncEventArgs>(Accpet_Comlpleted);
            socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(IP), Port));
            socket.Listen(Num);
            saeaReceive.SetBuffer(new byte[4096]);
            DebugLog.LogWarn("服务器已开启 等待链接中.....");
            StartAccpet();
        }


        public Client NetAsClient(string IP,int Port)
        {
            
            saeaConnect = new SocketAsyncEventArgs();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse(IP),Port));
            Client client = new Client(socket, Index);
            client.ReceiveDisConnectAction += DisConnect;
            client.ReceiveMsgAction+=ReceiveMsg;
            IMsg.OnConnectToServer(ref client);
            return client;
        }


        void StartAccpet()
        {
            bool suspend = socket.AcceptAsync(saeaAccpet);
            if (!suspend)
            {

                AccpetProcessConnect();
            }
        }


        void AccpetProcessConnect()
        {
            Client client = new Client(saeaAccpet.AcceptSocket, Index);
            client.ReceiveDisConnectAction += DisConnect;
            client.ReceiveMsgAction += ReceiveMsg;
            IMsg.OnClientAccpet(ref client);
            saeaAccpet.AcceptSocket = null;
            StartAccpet();
            Index += 1;
            
        }


        void Accpet_Comlpleted(object sender, SocketAsyncEventArgs e)
        {
            AccpetProcessConnect();
        }



      

        void ReceiveMsg(byte[] bytes,Client client)
        {
            IMsg.OnMessage(bytes,ref client);
        }

        void DisConnect(Client client)
        {
            IMsg.OnDisConnectToServer(ref client);
        }


        

    }
}
