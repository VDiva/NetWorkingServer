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
        public PlayerData playerData;
        public Msg(Data data,ref PlayerData playerData)
        {
            this.playerData = playerData;
            this.data = data;
        }
    }
}
