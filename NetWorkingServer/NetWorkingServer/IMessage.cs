using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkingServer
{
    public interface IMessage
    {
        void OnClientAccpet(ref Client client);
        void OnMessage( byte[] data, ref Client client);
        void OnConnectToServer(ref Client client);
        void OnDisConnectToServer(ref Client client);
    }
}
