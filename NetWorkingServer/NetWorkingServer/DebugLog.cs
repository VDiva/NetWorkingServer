using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkingServer
{
    public static class DebugLog
    {
        public static Action<string> LogAction;
        public static Action<string> LogErrorAction;
        public static Action<string> LogWarnAction;
        public static void Log(string msg)
        {
            LogAction?.Invoke(msg);
        }

        public static void LogError(string msg)
        {
            LogErrorAction?.Invoke(msg);
        }

        public static void LogWarn(string msg)
        {
            LogWarnAction?.Invoke(msg);
        }
    }
}
