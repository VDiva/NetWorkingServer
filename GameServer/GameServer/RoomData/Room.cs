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
        public string RoomName;
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

            Data data = new Data { MsgType=MsgType.JoinRoomCallBack };
            foreach(Client client in clients)
            {
                if (client.ID != msg.client.ID)
                {
                    data.PlayerData = new GameData.PlayerData { 
                        ID = client.ID,
                    };
                    msg.client.SendMessage(MsgTool.Serialization(data));

                    data.PlayerData = new GameData.PlayerData
                    {
                        ID = msg.client.ID,
                    };

                   client.SendMessage(MsgTool.Serialization(data));
                }
            }


            DebugLog.LogWarn("加入房间成功");
            return true;
        }



        public void BlackRoom(ref Msg msg)
        {
            Data data = new Data();
            try
            {

                clients.Remove(msg.client);
                msg.client.IsJoinRoom= false;
                data.MsgType = MsgType.BlackRoomSucceed;
            }catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
                data.MsgType = MsgType.BlackRoomError;
            }
            msg.client.SendMessage(MsgTool.Serialization(data));



        }

        public bool IsFullof()
        {
            return clients.Count > MaxPeople;
        }

        public void ClearRoom()
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
            foreach (var item in clients)
            {
                if (item.ID != msg.client.ID)
                {
                    item.SendMessage(MsgTool.Serialization(msg.data));
                }   
            }
        }

        private void UpdataTransform(ref Msg msg)
        {
            foreach (var item in clients)
            {
                if (item.ID != msg.client.ID)
                {
                    item.SendMessage(MsgTool.Serialization(msg.data));
                }
            }
        }



    }
}
