
using NetWorkingServer;
using Newtonsoft.Json;
using System.Text;

namespace GameServer
{
    public static class GameTool
    {
        

        public static T DeSerialization<T>(byte[] data)
        {
            T da =JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Tool.UnPacket(ref data)));
            return da;
        }

        public static byte[] Serialization(object data)
        {
            byte[] bytes =Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            return Tool.Packet(ref bytes);
        }
    }
}
