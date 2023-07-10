using GameServer.GameTool;

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
                   
                    MessageManager.Instance.UpData();
                    ClientManager.Instance.UpData();
                    LobbyManager.Instance.UpData();
                    RoomManager.Instance.UpData();
                }
            }).Start();
        }
    }
}
