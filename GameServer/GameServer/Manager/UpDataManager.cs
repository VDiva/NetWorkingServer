using NetWorkingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Manager
{
    public class UpDataManager: SingletonClass<UpDataManager>
    {

        public UpDataManager() { }

        public void Init()
        {

            new Task(() =>
            {
                while (true)
                {
                    Thread.Sleep(30);
                    DebugLog.LogWarn("循环管理器");
                    MessageManager.Instance.UpData();
                    ClientManager.Instance.UpData();
                    LobbyManager.Instance.UpData();
                    RoomManager.Instance.UpData();
                }
            }).Start();
        }
    }
}
