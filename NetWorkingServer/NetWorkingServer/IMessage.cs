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
        void OnMessage( byte[] data, object client);
        void OnConnectToServer(object client);
        void OnDisConnectToServer(object client);
    }
}
