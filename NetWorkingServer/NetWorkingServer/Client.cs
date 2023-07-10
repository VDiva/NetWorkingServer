
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using System;

namespace NetWorkingServer
{
    public class Client<T> where T : IMessage, new()
    {
        public Socket socket;
        public int ID;
        private SocketAsyncEventArgs saeaReceive;
        private SocketAsyncEventArgs saeaSend;
        T IMsg;

        public Client(Socket s,int id)
        {
            
            IMsg = new T();
            socket=s;
            ID = id;
            saeaReceive = new SocketAsyncEventArgs();
            saeaSend= new SocketAsyncEventArgs();
            saeaReceive.SetBuffer(new byte[4096],0,4096);
            saeaReceive.Completed += Receive_Comlpleted;
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
            if (saeaReceive.SocketError == SocketError.Success && saeaReceive.BytesTransferred > 0)
            {
                byte[] bytes = new byte[saeaReceive.BytesTransferred];
                Buffer.BlockCopy(saeaReceive.Buffer, 0, bytes, 0, saeaReceive.BytesTransferred);

                IMsg.OnMessage(bytes, ID);

                StartReceive();

            }
            else if (saeaReceive.BytesTransferred == 0)
            {
                IMsg.OnDisConnectToServer(ID);
            }
           
        }


        void Receive_Comlpleted(object sender, SocketAsyncEventArgs e)
        {

            
            ReceiveProcessConnect();

        }

        
        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="buffer"></param>
        public void SendMessageAsyn(byte[] buffer)
        {
            saeaSend.SetBuffer(buffer, 0, buffer.Length);
            socket.SendAsync(saeaSend);
        }


        /// <summary>
        /// 同步执行
        /// </summary>
        /// <param name="buffer"></param>
        public void SendMessage(byte[] buffer)
        {
            
            socket.Send(buffer);
        }

    }
}
