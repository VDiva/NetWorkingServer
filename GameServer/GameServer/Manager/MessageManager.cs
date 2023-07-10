using GameData;
using NetWorkingServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Manager
{

    public class MessageManager: SingletonClass<MessageManager>
    {
        private ConcurrentQueue<Data> DataQueue;
        public MessageManager() { 
            DataQueue = new ConcurrentQueue<Data>();
        }

        public void UpData()
        {
            try
            {
                if(DataQueue.TryDequeue(out Data data))
                {
                    _ = ThreadPool.QueueUserWorkItem(MessageHandle,data);
                }
            }catch(Exception ex)
            {
                DebugLog.LogError(ex.Message);
            }
        }

        private void MessageHandle(object? state)
        {
            if (state == null) return;
            Data data= (Data)state;
            switch (data.MsgType)
            {
                case MsgType.AllocationIdmsg:
                    break;
                case MsgType.JoinMsg:
                    break;
                case MsgType.StringMsg:
                    break;
                case MsgType.AnimMsg:
                    break;
                case MsgType.TransformMsg:
                    break;
                
            }
        }

        public void Enqueue(Data data)
        {
            DataQueue.Enqueue(data);
        }

    }
}
