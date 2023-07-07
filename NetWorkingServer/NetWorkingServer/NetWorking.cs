using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkingServer
{
    public class NetWorking
    {
        Socket socket;
        SocketAsyncEventArgs saeaAccpet;
        SocketAsyncEventArgs saeaReceive;
        SocketAsyncEventArgs saeaSend;

        ConcurrentStack<Client> clients;
        List<Socket> OnlineClients;

        public Action OnReceive;
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
            clients=new ConcurrentStack<Client>();
            OnlineClients = new List<Socket>();
            StartAccpet();
        }


        public void NetAsClient(string IP,string Port)
        {

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
            if(clients.Count>0&&clients.TryPop(out Client client))
            {
                client.socket = saeaAccpet.AcceptSocket;
                OnlineClients.Add(saeaAccpet.AcceptSocket);
            }
            else
            {
                Client newClient= new Client();
                newClient.socket = saeaAccpet.AcceptSocket;
                OnlineClients.Add(saeaAccpet.AcceptSocket);
            }

            StartAccpet();
        }


        void Accpet_Comlpleted(object sender, SocketAsyncEventArgs e)
        {
            AccpetProcessConnect();
            DebugLog.LogWarn("玩家IP" + (sender as Socket).RemoteEndPoint + "连上了服务器");
        }






        
        void StartReceive()
        {
            bool suspend = socket.ReceiveAsync(saeaReceive);
            if (!suspend)
            {
                ReceiveProcessConnect();
            }
        }


        void ReceiveProcessConnect()
        {
            StartReceive();
        }


        void Receive_Comlpleted(object sender, SocketAsyncEventArgs e)
        {

            if (saeaReceive.BytesTransferred > 0 && saeaReceive.SocketError == SocketError.Success)
            {
                byte[] bytes = new byte[saeaReceive.BytesTransferred];
                Buffer.BlockCopy(saeaReceive.Buffer, 0, bytes, 0, saeaReceive.BytesTransferred);
                
               
                Socket s=sender as Socket;
                OnlineClients.Remove(s);
                s.Close();
                clients.Push(new Client { socket = s });
                
            }
            else
            {
                
                DebugLog.LogError(saeaReceive.SocketError.ToString());
            }

            ReceiveProcessConnect();
        }



    


        void SendProcessConnect()
        {
            
        }


        void Send_Comlpleted(object sender, SocketAsyncEventArgs e)
        {

            
        }





    }
}
