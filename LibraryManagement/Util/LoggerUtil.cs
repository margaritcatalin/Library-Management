// <copyright file="LoggerUtil.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Logger utility class.</summary>

namespace LibraryManagement.Util
{
    using System.Reflection;

    /// <summary>
    /// Logging helper.
    /// </summary>
    public class LoggerUtil
    {
        /// <summary>
        /// Defines the Log.
        /// </summary>
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Log a new message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="method">The method<see cref="MethodBase"/>.</param>
        public static void LogInfo(string message, MethodBase method)
        {
            string methodName = method.DeclaringType.Name + "." + method.Name;
            if (Log.IsInfoEnabled)
            {
                Log.Info("[" + methodName + "]:" + message);
            }
        }

        /// <summary>
        /// Log a new error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="method">The method<see cref="MethodBase"/>.</param>
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
        /// <param name="method">The method<see cref="MethodBase"/>.</param>
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