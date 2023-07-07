using System;
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
        SocketAsyncEventArgs saea;

        public NetWorking()
        {
            
        }

        public void NetAsServer(string IP,int Port,int Num)
        {
            saea = new SocketAsyncEventArgs();
            saea.Completed += new EventHandler<SocketAsyncEventArgs>(Net_Comlpleted);
            socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(IP), Port));
            socket.Listen(Num);
            StartAccpet();
        }


        public void NetAsClient(string IP,string Port)
        {

        }

        
        void StartAccpet()
        {
            bool suspend = socket.AcceptAsync(saea);
            if (!suspend)
            {
                ProcessConnect();
            }
        }


        void ProcessConnect()
        {

        }


        void Net_Comlpleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessConnect();
            DebugLog.LogWarn("玩家IP" + (sender as Socket).RemoteEndPoint + "连上了服务器");
        }

       

    }
}
