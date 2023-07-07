using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkingServer
{
    public class ServerMain
    {
        public static void Main(string[] args)
        {       
            DebugLogSetting debug=new DebugLogSetting();
            NetWorking working = new NetWorking();
            Console.ReadKey();
        }
    }
}
