using System;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PainKiller.SerilogExtensions.Managers
{
    public static class GetLoggerManager
    {
        public static Microsoft.Extensions.Logging.ILogger GetFileLogger(string fileName, string rollingIntervall, string logLevel)
        {
            var eLogLevel = Enum.Parse<LogLevel>(logLevel, ignoreCase: true);
            var log = new LoggerConfiguration()
                .WriteTo.File(fileName, rollingInterval: Enum.Parse<RollingInterval>(rollingIntervall) , restrictedToMinimumLevel:eLogLevel.ToLogLevel())
                .CreateLogger();
            return log.ToMicrosoftILoggerImplementation();
        }
    }
}