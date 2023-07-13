using GameData;
using NetWorkingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameTool
{
    public class Msg
    {
        
        public Data data;
        public Client client;
        public Msg(ref Data data,ref Client client)
        {
            this.client = client;
            this.data = data;
        }
    }
}
