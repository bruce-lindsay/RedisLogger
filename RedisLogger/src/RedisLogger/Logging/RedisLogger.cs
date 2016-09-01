using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Logging {
    public class RedisLogger : ILogger {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly IRedisLogWriter _writer;
        private readonly string _categoryName;
        private readonly string _appName;

        public RedisLogger(string categoryName, string appName, Func<string, LogLevel, bool> filter, IRedisLogWriter writer) {
            _filter = filter;
            _writer = writer;
            _categoryName = categoryName;
            _appName = appName;
        }
        public bool IsEnabled(LogLevel logLevel) {
            return (_filter == null || _filter(_categoryName, logLevel));
        }

        public IDisposable BeginScope<TState>(TState state) {
            return null;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter) {
            var redisLogEntry = state as RedisLogEntry;
            if (!IsEnabled(logLevel)) {
                return;
            }
            if (redisLogEntry != null) {
                redisLogEntry.logger = _categoryName;
                redisLogEntry.appname = _appName;
            }
            if (formatter == null) {
                throw new ArgumentNullException(nameof(formatter));
            }
            var message = formatter(state, exception);

            if (String.IsNullOrEmpty(message)) {
                return;
            }
            _writer.WriteLog(message);
        }

    }
}