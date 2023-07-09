
using NetWorkingServer;
using GameData;

namespace GameServer
{
    public static class MessageHandle
    {
        public static void Handle(Data data)
        {
            switch(data.MsgType)
            {
                case MsgType.AnimMsg:
                    break;
                case MsgType.StringMsg:
                    DebugLog.LogWarn(data.Msg);
                    break;
                case MsgType.TransformMsg: 
                    break;
                case MsgType.JoinMsg: 

                    break;
            }
        }
    }
}
