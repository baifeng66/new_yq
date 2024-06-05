using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Core
{
    /// <summary>
    /// 日志类别
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 默认
        /// </summary>
        AppLog,
        /// <summary>
        /// windows服务日志
        /// </summary>
        ServiceLog,
        /// <summary>
        /// 插件日志
        /// </summary>
        PluginLog,
        /// <summary>
        /// 控制台
        /// </summary>
        ConsoleLog
    }

    /// <summary>
    /// log4日志文件记录
    /// </summary>
    public sealed class Log4Helper
    {

        private static readonly log4net.ILog log;

        static Log4Helper()
        {
            ILoggerRepository repository = LogManager.CreateRepository("CoreLogRepository");

            //AppDomain.CurrentDomain.BaseDirectory
            var basePath = AppDomain.CurrentDomain.BaseDirectory;// Directory.GetCurrentDirectory();
            // 初始化配置文件
            string configPath = Path.Combine(basePath, "log4net.config");
            if (File.Exists(configPath))
            {
                log4net.Config.XmlConfigurator.Configure(repository, new FileInfo(configPath));
            }
            else
            {
                log4net.Config.XmlConfigurator.Configure();
            }
            ClearOldLog();

            log = LogManager.GetLogger(repository.Name, "AppLog");



        }

        public static void ClearOldLog()
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                //删除过期文件
                string path = Path.Combine(basePath, "Log");
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (var d in dirs)
                {
                    if (d.Name != DateTime.Now.ToString("yyyyMM") &&
                        d.Name != DateTime.Now.AddMonths(-1).ToString("yyyyMM"))
                    {
                        Directory.Delete(d.FullName, true);
                    }
                }

            }
            catch
            {
            }

        }

        /// <summary>
        /// 错误日志 ERROR
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            if (log.IsErrorEnabled)
                log.Error(message);
        }

        /// <summary>
        /// 错误日志 ERROR
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string message, params object[] args)
        {
            if (log.IsErrorEnabled)
                log.ErrorFormat(message, args);
        }
        /// <summary>
        /// 错误日志 ERROR
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string message, Exception ex)
        {
            if (log.IsErrorEnabled)
                log.Error(message, ex);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            if (log.IsDebugEnabled)
                log.Debug(message);
        }
        /// <summary>
        /// 调试日志 DEBUG
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void DebugFormat(string message, params object[] args)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat(message, args);
        }


        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            if (log.IsInfoEnabled)
                log.Info(message);
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string message, params object[] args)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat(message, args);
        }

        /// <summary>
        ///  警告日志 WARN
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void WarnFormat(string message, params object[] args)
        {
            if (log.IsWarnEnabled)
                log.WarnFormat(message, args);
        }

        /// <summary>
        ///  警告日志 WARN
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            if (log.IsWarnEnabled)
                log.Warn(message);
        }
        /// <summary>
        /// 警告日志 WARN
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Warn(string message, Exception ex)
        {
            if (log.IsWarnEnabled)
                log.Warn(message, ex);
        }


        /// <summary>
        ///  系统奔溃日志 Fatal
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void FatalFormat(string message, params object[] args)
        {
            if (log.IsFatalEnabled)
                log.FatalFormat(message, args);
        }

        /// <summary>
        ///  系统奔溃日志 WARN
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {
            if (log.IsFatalEnabled)
                log.Fatal(message);
        }
        /// <summary>
        /// 系统奔溃日志 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Fatal(string message, Exception ex)
        {
            if (log.IsFatalEnabled)
                log.Fatal(message, ex);
        }

    }

}
