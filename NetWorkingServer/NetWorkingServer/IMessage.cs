using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkingServer
{
    public interface IMessage
    {
        void OnClientAccpet(object client);
        void OnMessage( byte[] data, int ID);
        void OnConnectToServer(int ID);
        void OnDisConnectToServer(int ID);
    }
}
