using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkingServer
{
    public class DebugLogSetting
    {
        public DebugLogSetting() {

            DebugLog.LogAction += Log;
            DebugLog.LogWarnAction += LogWarn;
            DebugLog.LogErrorAction += LogError;
        }

        protected virtual void Log(string msg)
        {

            Console.WriteLine(msg);
        }

        protected virtual void LogError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }


        protected virtual void LogWarn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
