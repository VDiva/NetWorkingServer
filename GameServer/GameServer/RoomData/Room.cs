using GameData;
using GameServer.GameTool;
using GameServer.Manager;
using NetWorkingServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.RoomData
{
    public class Room
    {
        public int MaxPeople;
        List<Client> clients;
        private ConcurrentQueue<Msg> MessageQueue;
        int RoomId;
        public Room(int RoomId) {
            MessageQueue = new ConcurrentQueue<Msg>();
            this.RoomId = RoomId;
            clients = new List<Client>();
        }

        public void AddMessage(ref Msg msg)
        {
            DebugLog.LogWarn(msg.client.RoomID.ToString());
            MessageQueue.Enqueue(msg);
        }

        public void Init(int MaxPeople)
        {
            this.MaxPeople = MaxPeople;
        }

        public bool JoinRoom(ref Msg msg)
        {
            if(clients.Count>MaxPeople) return false;
            msg.client.IsJoinRoom = true;
            msg.client.RoomID = RoomId;
            clients.Add(msg.client);
            DebugLog.LogWarn("房间成功");
            return true;
        }

        public void BlackRoom(ref Msg msg)
        {
            try
            {

                clients.Remove(msg.client);
                msg.client.IsJoinRoom= false;
            }catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
            }
        }

        public void CliearRoom()
        {
            clients.Clear();
        }


        public void UpData()
        {
            while (MessageQueue.Count > 0)
            {
                if (MessageQueue.TryDequeue(out Msg msg))
                {
                    MessageHandle(ref msg);
                }
            }
        }

        private void MessageHandle(ref Msg msg)
        {

            switch (msg.data.MsgType)
            {
                
                case MsgType.AnimMsg:
                    UpdataAnim(ref msg);
                    break;
                case MsgType.TransformMsg:
                    UpdataTransform(ref msg);
                    break;
               
            }
        }

        private void UpdataAnim(ref Msg msg)
        {

        }

        private void UpdataTransform(ref Msg msg)
        {

        }



    }
}
