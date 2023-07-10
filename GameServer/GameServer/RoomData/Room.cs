using GameServer.GameTool;
using NetWorkingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.RoomData
{
    public class Room
    {
        public int MaxPeople;
        List<PlayerData> playerDatas;
        int RoomId;
        public Room(int RoomId) {
            this.RoomId = RoomId;
            playerDatas = new List<PlayerData>();
        }

        public void Init(int MaxPeople)
        {
            this.MaxPeople = MaxPeople;
        }

        public bool JoinRoom(ref Msg msg)
        {
            if(playerDatas.Count>MaxPeople) return false;
            msg.playerData.IsJoinRoom = true;
            msg.playerData.RoomID = RoomId;
            playerDatas.Add(msg.playerData);
            DebugLog.LogWarn("房间成功");
            return true;
        }

        public void BlackRoom(ref Msg msg)
        {
            try
            {

                playerDatas.Remove(msg.playerData);
                msg.playerData.IsJoinRoom= false;
            }catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
            }
        }

        public void CliearRoom()
        {
            playerDatas.Clear();
        }




    }
}
