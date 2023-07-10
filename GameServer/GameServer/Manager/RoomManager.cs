
using NetWorkingServer;

using System.Collections.Concurrent;
using GameServer.RoomData;

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

        public RoomManager() {
            roomQueue = new ConcurrentQueue<Room>();
            rooms = new ConcurrentDictionary<int, Room>();
        }

        public void CreateRoom(int MaxPeople,Client<Message> client)
        {
            if(roomQueue.TryDequeue(out Room room))
            {
                room.Init(MaxPeople);
                room.JoinRoom(client);
            }
            else
            {
                Room r=new Room();
                r.JoinRoom(client);
                r.Init(MaxPeople);
                rooms.TryAdd(0, r);
            }
        }

        public void JoinRandomRoom()
        {

        }

        public void JoinRoom(int roomId)
        {

        }

        public void UpData()
        {

        }
    }
}
