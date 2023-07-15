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
            
            MessageQueue.Enqueue(msg);
        }


        private void MessageHandle(ref Msg msg)
        {
            
            switch (msg.data.MsgType)
            {
                case MsgType.AllocationIdmsg:
                    break;
                case MsgType.StringMsg:
                    break;
                case MsgType.RoomMsg: 
                    RoomManager.Instance.AddMessage(ref msg);
                    break;
                
                
                
            }
        }

        

    }
}
