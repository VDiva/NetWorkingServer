
using System.Net.Sockets;

using System.Text.Json;
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
                byte[] data = Tool.UnPacket(ref bytes);
                DebugLog.LogWarn("收到消息");
                IMsg.OnMessage(data, this);

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


        public void Send(object data)
        {
            DebugLog.LogWarn(JsonSerializer.SerializeToUtf8Bytes(data).Length.ToString());
            Send(JsonSerializer.SerializeToUtf8Bytes(data));

        }

        public void Send(byte[] data)
        {
            byte[] bytes = Tool.Packet(ref data);
            saeaSend.SetBuffer(bytes,0,bytes.Length);
            socket.SendAsync(saeaSend);
        }
    }
}
