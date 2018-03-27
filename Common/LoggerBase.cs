using System;
using log4net;
using log4net.Config;

namespace Common
{
    /// <summary>
    ///     This is the base class for loggin purposes.
    ///     Any class that require to log errors/debug/info, it must be derived from this.
    /// </summary>
    public abstract class LoggerBase
    {
        /// <summary>
        ///     Member variable to hold the <see cref="ILog" /> instance.
        /// </summary>
        private readonly ILog logger;

        /// <summary>
        ///     Abstract property which must be overridden by the derived classes.
        ///     The logger prefix is used to create the logger instance.
        /// </summary>
        protected abstract Type LogPrefix { get; }

        /// <summary>
        ///     Debug level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged</param>
        protected void LogDebug(string message)
        {
            if (logger.IsDebugEnabled)
                logger.Debug(message);
        }

        /// <summary>
        ///     Debug level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged</param>
        /// <param name="e">The exception that needs to be logged</param>
        protected void LogDebug(string message, Exception e)
        {
            if (logger.IsDebugEnabled)
                logger.Debug(message, e);
        }

        /// <summary>
        ///     Error level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged.</param>
        protected void LogError(string message)
        {
            if (logger.IsErrorEnabled)
                logger.Error(message);
        }

        /// <summary>
        ///     Error level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged.</param>
        /// <param name="e">The exception that needs to be logged.</param>
        protected void LogError(string message, Exception e)
        {
            if (logger.IsErrorEnabled)
                logger.Error(message, e);
        }

        /// <summary>
        ///     Fatal level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged</param>
        protected void LogFatal(string message)
        {
            if (logger.IsFatalEnabled)
                logger.Fatal(message);
        }

        /// <summary>
        ///     Fatal level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged</param>
        /// <param name="e">The exception that needs to be logged</param>
        protected void LogFatal(string message, Exception e)
        {
            if (logger.IsFatalEnabled)
                logger.Fatal(message, e);
        }

        /// <summary>
        ///     Information level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged.</param>
        protected void LogInfo(string message)
        {
            if (logger.IsInfoEnabled)
                logger.Info(message);
        }

        /// <summary>
        ///     Information level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged.</param>
        /// <param name="e">The exception that needs to be logged.</param>
        protected void LogInfo(string message, Exception e)
        {
            if (logger.IsInfoEnabled)
                logger.Info(message, e);
        }

        /// <summary>
        ///     Warning level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged.</param>
        protected void LogWarn(string message)
        {
            if (logger.IsWarnEnabled)
                logger.Warn(message);
        }

        /// <summary>
        ///     Warning level messages are logged to the logger.
        /// </summary>
        /// <param name="message">String that needs to be logged.</param>
        /// <param name="e">The exception that needs to be logged.</param>
        protected void LogWarn(string message, Exception e)
        {
            if (logger.IsWarnEnabled)
                logger.Warn(message, e);
        }

        private static bool isConfigured;

        /// <summary>
        ///     Constructor of the class.
        /// </summary>
        protected LoggerBase()
        {
            // initiate logging class           
            if (!isConfigured)
            {
                XmlConfigurator.Configure();
                isConfigured = true;
            }
            logger = LogManager.GetLogger(LogPrefix);
        }
    }
}