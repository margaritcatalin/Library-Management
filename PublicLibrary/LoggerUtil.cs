using System;

namespace PublicLibrary
{

    

    public class LoggerUtil
    {
        /*static LoggerUtil()
        {
            log4net.Config.XmlConfigurator.Configure();
        }*/
    private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogInfo(string message)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info(message);
            }
        }
        public static void LogError(string message)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Error(message);
            }
        }
        public static void LogWarning(string message)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Warn(message);
            }
        }
    }
}