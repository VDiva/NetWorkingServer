using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetWorkingServer
{
    public class Tool
    {
        public static byte[] UnPacket(ref byte[] cache)
        {
            if (cache.Length < 4)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream(cache))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    //包的长度（ReadInt32:读取一个4字节的带符号整数，并把流的位置向前移4个字节）
                    int length = br.ReadInt32();
                    //包的长度减去流的当前位置，用于判断包是否完整
                    int remainLength = (int)(ms.Length - ms.Position);
                    if (length > remainLength)
                    {
                        return null;
                    }

                    //读取数据,并且流的位置position向前移动length长度的字节
                    byte[] data = br.ReadBytes(length);

                    return data;
                }
            }
        }

        public static byte[] Packet(ref byte[] cache)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(cache.Length);
                bw.Write(cache);
                return ms.ToArray();    
            }
        }


        
    }
}
