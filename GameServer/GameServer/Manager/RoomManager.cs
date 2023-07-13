
using NetWorkingServer;

using System.Collections.Concurrent;
using GameServer.RoomData;
using GameData;

using GameServer.GameTool;

namespace GameServer.Manager
{
    public class RoomManager:SingletonClass<RoomManager>
    {
        /// <summary>
        /// 当前存在的房间
        /// </summary>
        private ConcurrentDictionary<int, Room> rooms;
        /// <summary>
        /// 没有人的房间待分配
        /// </summary>
        private ConcurrentQueue<Room> roomQueue;


        private ConcurrentQueue<Msg> MessageQueue;

        private int RoomID = 1;

        private int DefMaxPeople = 2;
        public RoomManager() {
            roomQueue = new ConcurrentQueue<Room>();
            rooms = new ConcurrentDictionary<int, Room>();
            MessageQueue = new ConcurrentQueue<Msg>(); 
        }

        public void AddMessage(ref Msg msg)
        {
            DebugLog.LogWarn(msg.client.RoomID.ToString());
            MessageQueue.Enqueue(msg);
        }

        public void CreateRoom(int MaxPeople,ref Msg msg)
        {
            
            if(roomQueue.TryDequeue(out Room room))
            {
                room.Init(MaxPeople);
                room.JoinRoom(ref msg);
            }
            else
            {
                RoomData.Room r =new RoomData.Room(RoomID);
                r.JoinRoom(ref msg);
                r.Init(MaxPeople);
                rooms.TryAdd(0, r);
                RoomID += 1;
            }
        }

        public void JoinRandomRoom(ref Msg msg)
        {

            if (rooms.Count > 0)
            {
                Room room = rooms[Random.Shared.Next(0, rooms.Count)];
                room.JoinRoom(ref msg);
            }
            else
            {
                if (roomQueue.TryDequeue(out Room room))
                {
                    room.Init(DefMaxPeople);
                    room.JoinRoom(ref msg);
                }
                else
                {
                    Room r = new Room(RoomID);
                    r.JoinRoom(ref msg);
                    r.Init(DefMaxPeople);
                    rooms.TryAdd(0, r);
                    RoomID += 1;
                }
            }
        }

        public bool JoinRoom(int roomId, ref Msg msg)
        {
            if(rooms.TryGetValue(roomId,out Room room))
            {
                room.JoinRoom(ref msg);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpData()
        {
            try
            {
                if (MessageQueue.TryDequeue(out Msg msg))
                {
                    MessageHandle(ref msg);
                }
            }
            catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
            }
            UpDataRoom();
        }

        private void MessageHandle(ref Msg msg)
        {
            bool isHas = rooms.TryGetValue(msg.client.RoomID, out Room room);
            switch (msg.data.MsgType)
            {
                
                case MsgType.AnimMsg:
                    if (isHas)
                    {
                        room?.AddMessage(ref msg);
                    }
                    break;
                case MsgType.TransformMsg:
                    if (isHas)
                    {
                        room?.AddMessage(ref msg);
                    }
                    break;
                
            }
        }

        private void UpDataRoom()
        {
            foreach (Room room in rooms.Values)
            {
                room.UpData();
            }
        }
    }
}
