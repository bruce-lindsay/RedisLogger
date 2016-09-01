using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Logging {
    public class RedisLoggerProvider : ILoggerProvider {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly IRedisLogWriter _redisLogWriter;
        private readonly string _appName;

        public RedisLoggerProvider(string appName, Func<string, LogLevel, bool> filter, IRedisLogWriter writer) {
            _redisLogWriter = writer;
            _filter = filter;
            _appName = appName;
        }
        public void Dispose() {
            _redisLogWriter.Dispose();
        }

        public ILogger CreateLogger(string categoryName) {
            return new RedisLogger(categoryName, _appName, _filter, _redisLogWriter);
        }
    }

}