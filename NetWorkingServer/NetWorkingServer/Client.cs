
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;

namespace NetWorkingServer
{
    public class Client<T> where T : IMessage, new()
    {
        public Socket socket;
        public int ID;
        public bool IsJoinLobby;
        public bool IsJoinRoom;
        public int RoomID;

        private SocketAsyncEventArgs saeaReceive;
        private SocketAsyncEventArgs saeaSend;
        T IMsg;

        public Client(Socket s,int id)
        {
            IsJoinLobby = false;
            IsJoinRoom = false;
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
                

                IMsg.OnMessage(bytes, this);

                StartReceive();

            }
            else if (saeaReceive.BytesTransferred == 0)
            {
                IMsg.OnDisConnectToServer(this);


            }
           
        }


        void Receive_Comlpleted(object sender, SocketAsyncEventArgs e)
        {

            
            ReceiveProcessConnect();

        }

        public void SetSendMessageBuffer(byte[] buffer)
        {
            saeaSend.SetBuffer(buffer,0,buffer.Length);
        }

        public void SendMessage()
        {
            socket.SendAsync(saeaSend);
        }
        
    }
}
