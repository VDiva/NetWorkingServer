using GameData;
using GameServer.GameTool;
using GameServer.Manager;
using NetWorkingServer;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.RoomData
{
    public class Room
    {
        public string RoomName="房间";
        public int MaxPeople;
        List<Client> clients;
        private ConcurrentQueue<Msg> MessageQueue;
        public int RoomId;
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

        public void JoinRoom(ref Msg msg)
        {
            
            msg.client.IsJoinRoom = true;
            msg.client.RoomID = RoomId;
            clients.Add(msg.client);

            Data data = new Data {
                MsgType = MsgType.RoomMsg,
                RoomMsgType = RoomMsgType.JoinRoomCallBack,
                RoomData = new GameData.RoomData {
                    RoomID = RoomId,
                },
               
            };

            for(int i = 0;i<clients.Count;i++)
            {
                data.PlayerDatas.Add(new PlayerData { ID = clients[i].ID });
            }
            msg.client.SendMessage(MsgTool.Serialization(data));
            data.PlayerDatas.Clear();

            data.PlayerDatas.Add(new PlayerData { ID = msg.client.ID });
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].SendMessage(MsgTool.Serialization(data));
            }

            

            DebugLog.LogWarn("加入房间成功");
           
        }



        public void BlackRoom(ref Client client)
        {
            Data data = new Data();
            try
            {

                clients.Remove(client);
                client.IsJoinRoom= false;
                data.MsgType = MsgType.RoomMsg;
                data.RoomMsgType = RoomMsgType.BlackRoomSucceed;
                client.SendMessage(MsgTool.Serialization(data));
            }
            catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
                
            }
            



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
            ThreadPool.QueueUserWorkItem((t) =>
            {
                while (MessageQueue.Count > 0)
                {
                    if (MessageQueue.TryDequeue(out Msg msg))
                    {
                        MessageHandle(ref msg);
                    }
                }
            });
        }



        private void MessageHandle(ref Msg msg)
        {

            switch (msg.data.RoomMsgType)
            {
                
                case RoomMsgType.RoomAnimMsg:
                    UpdataAnim(ref msg);
                    break;
                case RoomMsgType.RoomTransformMsg:
                    UpdataTransform(ref msg);
                    break;
               
            }
        }

        private void UpdataAnim(ref Msg msg)
        {
            foreach (var item in clients)
            {
                if (item.ID != msg.data.PlayerData.ID)
                {
                    item.SendMessage(MsgTool.Serialization(msg.data));
                }   
            }
        }

        private void UpdataTransform(ref Msg msg)
        {
            for (int i=0;i<clients.Count;i++)
            {
                if (clients[i].ID != msg.data.PlayerData.ID)
                {
                    try
                    {
                        clients[i].SendMessageAsyn(MsgTool.Serialization(msg.data));
                    }
                    catch (Exception ex)
                    {
                        DebugLog.LogError(RoomName + ": " + ex.Message);
                    }
                }
            }
        }


        


        



    }
}
