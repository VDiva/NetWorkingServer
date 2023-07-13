
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
            
            MessageQueue.Enqueue(msg);
        }

        public void CreateRoom(int MaxPeople,ref Msg msg)
        {
            bool isJoin = false;
            if(roomQueue.TryDequeue(out Room room))
            {
                room.Init(MaxPeople);
                room.JoinRoom(ref msg);
                isJoin = true;
            }
            else
            {
                RoomData.Room r =new RoomData.Room(RoomID);
                r.JoinRoom(ref msg);
                r.Init(MaxPeople);
                rooms.TryAdd(RoomID, r);
                RoomID += 1;
                isJoin = true;
            }

            Data data = new Data { RoomData = msg.data.RoomData };
            if (isJoin)
            {
                data.MsgType = MsgType.CreateRoomMsgSucceed;
            }
            else
            {
                data.MsgType = MsgType.CreateRoomMsgError;
            }

            msg.client.SendMessage(MsgTool.Serialization(data));


        }

        public void JoinRandomRoom(ref Msg msg)
        {
            bool isJoin=false;
            if (rooms.Count > 0)
            {
                bool isHas = false;
                foreach (var room in rooms.Values)
                {
                    if (!room.IsFullof())
                    {
                        room.JoinRoom(ref msg);
                        isHas = true;
                        isJoin=true;
                        break;
                    }
                }

                if (!isHas)
                {
                    if (roomQueue.TryDequeue(out Room room))
                    {
                        room.Init(DefMaxPeople);
                        room.JoinRoom(ref msg);
                        isJoin = true;
                    }
                    else
                    {
                        Room r = new Room(RoomID);
                        r.JoinRoom(ref msg);
                        r.Init(DefMaxPeople);
                        rooms.TryAdd(0, r);
                        isJoin = true;
                        RoomID += 1;
                    }
                }

            }
            else
            {
                if (roomQueue.TryDequeue(out Room room))
                {
                    room.Init(DefMaxPeople);
                    room.JoinRoom(ref msg);
                    isJoin = true;
                }
                else
                {
                    Room r = new Room(RoomID);
                    r.JoinRoom(ref msg);
                    r.Init(DefMaxPeople);
                    rooms.TryAdd(0, r);
                    isJoin = true;
                    RoomID += 1;
                }
            }
            
            
            
            Data data = new Data { RoomData=msg.data.RoomData };
            if(isJoin)
            {
                data.MsgType = MsgType.JoinRandomRoomMsgSucceed;
            }
            else
            {
                data.MsgType = MsgType.JoinRandomRoomMsgError;
            }

            msg.client.SendMessage(MsgTool.Serialization(data));

        }

        public void JoinRoom(ref Msg msg)
        {
            if(rooms.TryGetValue(msg.data.RoomData.RoomID,out Room room))
            {
                room.JoinRoom(ref msg);
                Data data = new Data { MsgType = MsgType.JoinRoomMsgSucceed,RoomData=msg.data.RoomData };
                msg.client.SendMessage(MsgTool.Serialization(data));
            }
            else
            {
                Data data = new Data { MsgType = MsgType.JoinRoomMsgError };
                msg.client.SendMessage(MsgTool.Serialization(data));
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
                case MsgType.JoinRandomRoomMsg:
                    JoinRandomRoom(ref msg);
                    break;
                case MsgType.JoinRoomMsg:
                    JoinRoom(ref msg);
                    break;
                case MsgType.CreateRoomMsg: 
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


        public void RemoveClient(ref Client client)
        {

        }
    }
}
