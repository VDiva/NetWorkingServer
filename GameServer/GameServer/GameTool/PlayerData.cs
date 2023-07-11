using NetWorkingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameTool
{
    public class PlayerData
    {
        public bool IsJoinRoom;
        public bool IsJoinLobby;
        public int ID;
        public int RoomID;
        public Client client;
    }
}
