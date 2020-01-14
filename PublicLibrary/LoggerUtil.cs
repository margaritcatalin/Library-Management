// <copyright file="LoggerUtil.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace PublicLibrary
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
        public static void LogInfo(string message)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info(message);
            }
        }

        /// <summary>
        /// Log a new error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogError(string message)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Error(message);
            }
        }

        /// <summary>
        /// Log a new warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogWarning(string message)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Warn(message);
            }
        }
    }
}