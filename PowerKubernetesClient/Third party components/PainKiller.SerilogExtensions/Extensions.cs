using System;
using Microsoft.Extensions.Logging;
using PainKiller.SerilogExtensions.DomainObjects;
using Serilog.Events;

namespace PainKiller.SerilogExtensions
{
    public static class Extensions
    {
        public static LogEventLevel ToLogLevel(this LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return LogEventLevel.Debug;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Critical:
                    return LogEventLevel.Fatal;
                case LogLevel.None:
                    return LogEventLevel.Verbose;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }
        public static LoggerBase ToMicrosoftILoggerImplementation(this Serilog.ILogger logger)
        {
            var retVal = new LoggerBase(logger);
            return retVal;
        }
    }
}
