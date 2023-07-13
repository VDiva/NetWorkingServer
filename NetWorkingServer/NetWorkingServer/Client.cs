
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using System;

namespace NetWorkingServer
{
    public class Client
    {
        public Socket socket;
        public int ID;
        public bool IsJoinRoom;
        public bool IsJoinLobby;
        public int RoomID;


        private SocketAsyncEventArgs saeaReceive;
        private SocketAsyncEventArgs saeaSend;

        public Action<byte[],Client> ReceiveMsgAction;
        public Action<Client> ReceiveDisConnectAction;


        public Client(Socket s,int id)
        {
            IsJoinLobby = false;
            IsJoinRoom = false;
            socket=s;
            ID = id;
            saeaReceive = new SocketAsyncEventArgs();
            saeaSend= new SocketAsyncEventArgs();
            saeaReceive.SetBuffer(new byte[4096],0,4096);
            saeaReceive.AcceptSocket = socket;
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
                DebugLog.LogWarn("接收到消息");
                ReceiveMsgAction?.Invoke(bytes, this);

                StartReceive();

            }
            else if (saeaReceive.BytesTransferred == 0)
            {
                ReceiveDisConnectAction?.Invoke(this);
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
