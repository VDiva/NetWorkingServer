using System.Net;
using System.Net.Sockets;

using Newtonsoft.Json;
namespace NetWorkingServer
{
    public class NetWorking<T> where T:IMessage 
    {
        Socket socket;
        SocketAsyncEventArgs saeaAccpet;
        SocketAsyncEventArgs saeaReceive;
        SocketAsyncEventArgs saeaSend;
        SocketAsyncEventArgs saeaConnect;
        T IMsg;
        int Index = 0;
        
        public NetWorking()
        {
            
            saeaAccpet = new SocketAsyncEventArgs();
            saeaReceive = new SocketAsyncEventArgs();
            saeaSend = new SocketAsyncEventArgs();
        }
        public void NetAsServer(string IP,int Port,int Num)
        {
            saeaAccpet = new SocketAsyncEventArgs();
            saeaAccpet.Completed += new EventHandler<SocketAsyncEventArgs>(Accpet_Comlpleted);
            socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(IP), Port));
            socket.Listen(Num);
            saeaReceive.SetBuffer(new byte[4096]);
           
            
            StartAccpet();
        }


        public void NetAsClient(string IP,int Port)
        {
            saeaConnect = new SocketAsyncEventArgs();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse(IP),Port));
            Client<T> client = new Client<T>(socket, Index);     

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
            Client<T> client = new Client<T>(saeaAccpet.AcceptSocket, 1);
            IMsg.OnClientAccpet(client);
            saeaAccpet.AcceptSocket = null;
            StartAccpet();
            Index += 1;
        }


        void Accpet_Comlpleted(object sender, SocketAsyncEventArgs e)
        {
            AccpetProcessConnect();
            DebugLog.LogWarn("玩家IP" + (sender as Socket).RemoteEndPoint + "连上了服务器");
        }

    }
}
