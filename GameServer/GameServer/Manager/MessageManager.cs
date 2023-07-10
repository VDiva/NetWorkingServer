using GameData;
using GameServer.GameTool;

using NetWorkingServer;

using System.Collections.Concurrent;


namespace GameServer.Manager
{

    public class MessageManager: SingletonClass<MessageManager>
    {
        private ConcurrentQueue<Msg> MessageQueue;
        public MessageManager() {
            MessageQueue = new ConcurrentQueue<Msg>();
        }

        public void UpData()
        {
            while (MessageQueue.Count>0)
            {
                if (MessageQueue.TryDequeue(out Msg msg))
                {
                    MessageHandle(ref msg);
                }
            }
        }


        public void AddMessage(ref Msg msg)
        {
            DebugLog.LogWarn(msg.playerData.RoomID.ToString());
            //msg.playerData.RoomID += 1;
            MessageQueue.Enqueue(msg);
        }


        private void MessageHandle(ref Msg msg)
        {
            //DebugLog.Log(msg.playerData.RoomID.ToString());
            msg.playerData.RoomID += 1;
            
            //if (state == null) return;
            //Msg msg= (Msg)state;
            switch (msg.data.MsgType)
            {
                case MsgType.AllocationIdmsg:
                    break;
                case MsgType.StringMsg:
                    break;
                case MsgType.AnimMsg:
                    break;
                case MsgType.TransformMsg:
                    //RoomManager.Instance.AddMessage(msg);
                    break;
                case MsgType.JoinRoomMsg:
                    
                    break;
                case MsgType.JoinRandomRoomMsg:
                    //RoomManager.Instance.AddMessage(ref msg);
                    break;
                case MsgType.CreateRoomMsg:
                    break;
                
                
                
            }
        }

        

    }
}
