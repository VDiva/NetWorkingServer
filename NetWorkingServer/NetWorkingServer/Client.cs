using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkingServer
{
    public class Client<T> where T : IMessage
    {
        private Socket socket;
        private int ID;

        private SocketAsyncEventArgs saeaReceive;
        private SocketAsyncEventArgs saeaSend;
        T IMsg;

        public Client(Socket s,int id)
        {
            
            socket=s;
            ID = id;
            saeaReceive = new SocketAsyncEventArgs();
            saeaSend= new SocketAsyncEventArgs();
            saeaReceive.SetBuffer(new byte[4096]);
            StartReceive();
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
                IMsg.OnMessage(bytes);
            }
            else
            {
                IMsg.OnDisConnectToServer(this);
                DebugLog.LogError(saeaReceive.SocketError.ToString());
            }

            ReceiveProcessConnect();
        }
    }
}
