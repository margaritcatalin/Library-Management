// <copyright file="LoggerUtil.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System.Reflection;

namespace LibraryManagement
{
    /// <summary>
    /// Logging helper.
    /// </summary>
    public class LoggerUtil
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Log a new message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogInfo(string message, MethodBase method)
        {
            string methodName = method.DeclaringType.Name + "." + method.Name;
            if (Log.IsInfoEnabled)
            {
                Log.Info("[" + methodName + "]:" +message);
            }
        }

        /// <summary>
        /// Log a new error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogError(string message, MethodBase method)
        {
            string methodName = method.DeclaringType.Name + "." + method.Name;
            if (Log.IsInfoEnabled)
            {
                Log.Error("[" + methodName + "]:" + message);
            }
        }

        /// <summary>
        /// Log a new warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogWarning(string message, MethodBase method)
        {
            string methodName = method.DeclaringType.Name + "." + method.Name;
            if (Log.IsInfoEnabled)
            {
                Log.Warn("[" + methodName + "]:" + message);
            }
        }
    }
}