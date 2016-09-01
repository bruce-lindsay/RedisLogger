using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Newtonsoft.Json;

namespace Common.Logging {
    public static class RedisLoggerExtensions {
        private static readonly Func<object, Exception, string> DefaultMessageFormatter = MessageFormatter;

        private static string MessageFormatter(object o, Exception exception) {
            return JsonConvert.SerializeObject(o);
        }

        public static ILoggerFactory AddRedis(this ILoggerFactory factory, IRedisLogWriter writer) {
            var appName = Assembly.GetEntryAssembly().GetName().Name;
            factory.AddProvider(new RedisLoggerProvider(appName, (s, l) => s.Contains(appName), writer));
            return factory;
        }

        private static RedisLogEntry CreateLogEntry(HttpContext context, EventId id, Exception e,
            string message, LogLevel level) {
            var redisEntry = new RedisLogEntry {
                date = DateTime.UtcNow,
                host = Environment.MachineName,
                level = level.ToString(),
                eventid = id.Id.ToString(),
                exception = e,
                message = message ?? e?.Message
            };
            if (context != null) {
                redisEntry.method = context.Request.Method;
                redisEntry.uri = context.Request.Scheme + "://" + context.Request.Host +
                                 context.Request.Path + context.Request.QueryString.Value;
                redisEntry.ipaddress = context.Connection.RemoteIpAddress?.ToString();
            }
            return redisEntry;
        }

        public static void LogStash(this ILogger logger, LogLevel level, EventId id, HttpContext context,
            Exception exception, string message, params object[] args) {
            if (message != null) {
                message = new FormattedLogValues(message, args).ToString();
            }
            var logEntry = CreateLogEntry(context, id, exception, message, level);
            logger.Log<RedisLogEntry>(level, id, logEntry, exception, DefaultMessageFormatter);
        }

        public static void LogStashError(this ILogger logger, EventId id, HttpContext context, Exception exception,
            string message, params object[] args) {
            LogStash(logger, LogLevel.Error, id, context, exception, message, args);
        }

        public static void LogStashError(this ILogger logger, EventId id, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.Error, id, context, null, message, args);
        }

        public static void LogStashError(this ILogger logger, HttpContext context, string message, params object[] args) {
            LogStash(logger, LogLevel.Error, 0, context, null, message, args);
        }

        public static void LogStashInfo(this ILogger logger, EventId id, HttpContext context, Exception exception,
            string message, params object[] args) {
            LogStash(logger, LogLevel.Information, id, context, exception, message, args);
        }

        public static void LogStashInfo(this ILogger logger, EventId id, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.Information, id, context, null, message, args);
        }

        public static void LogStashInfo(this ILogger logger, HttpContext context, string message, params object[] args) {
            LogStash(logger, LogLevel.Information, 0, context, null, message, args);
        }

        public static void LogStashDebug(this ILogger logger, EventId id, HttpContext context, Exception exception,
            string message, params object[] args) {
            LogStash(logger, LogLevel.Debug, id, context, exception, message, args);
        }

        public static void LogStashDebug(this ILogger logger, EventId id, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.Debug, id, context, null, message, args);
        }

        public static void LogStashDebug(this ILogger logger, HttpContext context, string message, params object[] args) {
            LogStash(logger, LogLevel.Debug, 0, context, null, message, args);
        }

        public static void LogStashCritical(this ILogger logger, EventId id, HttpContext context, Exception exception,
            string message, params object[] args) {
            LogStash(logger, LogLevel.Critical, id, context, exception, message, args);
        }

        public static void LogStashCritical(this ILogger logger, EventId id, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.Critical, id, context, null, message, args);
        }

        public static void LogStashCritical(this ILogger logger, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.Critical, 0, context, null, message, args);
        }

        public static void LogStashWarning(this ILogger logger, EventId id, HttpContext context, Exception exception,
            string message, params object[] args) {
            LogStash(logger, LogLevel.Warning, id, context, exception, message, args);
        }

        public static void LogStashWarning(this ILogger logger, EventId id, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.Warning, id, context, null, message, args);
        }

        public static void LogStashWarning(this ILogger logger, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.Warning, 0, context, null, message, args);
        }

        public static void LogStashNone(this ILogger logger, EventId id, HttpContext context, Exception exception,
            string message, params object[] args) {
            LogStash(logger, LogLevel.None, id, context, exception, message, args);
        }

        public static void LogStashNone(this ILogger logger, EventId id, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.None, id, context, null, message, args);
        }

        public static void LogStashNone(this ILogger logger, HttpContext context, string message, params object[] args) {
            LogStash(logger, LogLevel.None, 0, context, null, message, args);
        }

        public static void LogStashTrace(this ILogger logger, EventId id, HttpContext context, Exception exception,
            string message, params object[] args) {
            LogStash(logger, LogLevel.Trace, id, context, exception, message, args);
        }

        public static void LogStashTrace(this ILogger logger, EventId id, HttpContext context, string message,
            params object[] args) {
            LogStash(logger, LogLevel.Trace, id, context, null, message, args);
        }

        public static void LogStashTrace(this ILogger logger, HttpContext context, string message, params object[] args) {
            LogStash(logger, LogLevel.Trace, 0, context, null, message, args);
        }
    }
}