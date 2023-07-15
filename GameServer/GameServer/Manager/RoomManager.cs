
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

        private int DefMaxPeople = 4;
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
                data.MsgType = MsgType.RoomMsg;
                data.RoomMsgType = RoomMsgType.CreateRoomMsgSucceed;
            }
            else
            {
                data.MsgType = MsgType.RoomMsg;
                data.RoomMsgType = RoomMsgType.CreateRoomMsgSucceed;
            }

            msg.client.SendMessage(MsgTool.Serialization(data));


        }

        public void JoinRandomRoom(ref Msg msg)
        {
            
           
            if (rooms.Count > 0)
            {
                
                foreach (var room in rooms.Values)
                {
                    if (!room.IsFullof())
                    {
                        room.JoinRoom(ref msg);
                        
                        //data.RoomData.RoomName = room.RoomName;

                        return;
                    }
                }
            }
            
                if (roomQueue.TryDequeue(out Room room1))
                {
                    room1.Init(DefMaxPeople);
                    room1.JoinRoom(ref msg);
                   
                }
                else
                {
                    Room r = new Room(RoomID);
                    r.JoinRoom(ref msg);
                    r.Init(DefMaxPeople);
                    rooms.TryAdd(RoomID, r);
                    RoomID += 1;
                }
            
            
            

        }

        public void JoinRoom(ref Msg msg)
        {
            if(rooms.TryGetValue(msg.data.RoomData.RoomID,out Room room))
            {
                room.JoinRoom(ref msg);
                Data data = new Data { 
                    MsgType = MsgType.RoomMsg,
                    RoomMsgType=RoomMsgType.JoinRandomRoomMsgSucceed,
                    RoomData=msg.data.RoomData 
                };
                msg.client.SendMessage(MsgTool.Serialization(data));
            }
            else
            {
                Data data = new Data
                {
                    MsgType = MsgType.RoomMsg,
                    RoomMsgType = RoomMsgType.JoinRoomMsgError,
                    RoomData = msg.data.RoomData
                };
                msg.client.SendMessage(MsgTool.Serialization(data));
            }
        }

        public void UpData()
        {
            try
            {
                while(MessageQueue.Count>0)
                {
                    if (MessageQueue.TryDequeue(out Msg msg))
                    {
                        
                        MessageHandle(ref msg);
                    }
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
            switch (msg.data.RoomMsgType)
            {
                case RoomMsgType.CreateRoomMsg:
                    CreateRoom(msg.data.RoomData.MaxPeople,ref msg);
                    break;
                case RoomMsgType.JoinRandomRoomMsg:
                    JoinRandomRoom(ref msg);
                    break;
                case RoomMsgType.JoinRoomMsg:
                    JoinRoom(ref msg);
                    break;
                case RoomMsgType.BlackRoom:
                    BlackRoom(ref msg.client);
                    break;
                case RoomMsgType.RoomTransformMsg:
                    room?.AddMessage(ref msg);  
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


        public void BlackRoom(ref Client client)
        {
            if(rooms.TryGetValue(client.RoomID,out Room room))
            {

                room.BlackRoom(ref client);
                
            }
        }
    }
}
