using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSPortal.Util
{    public static class Logger
    {
        public static void LogError(Type source, Exception ex)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(source);
            logger.Error(ex.ToString());
        }

        public static void LogInfo(Type source, string message)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(source);
            logger.Info(message);
        }

        public static void LogDebug(Type source, string message)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(source);
            logger.Debug(message);
        }
    }
}