using System;

namespace Common.Logging {
    public interface IRedisLogWriter : IDisposable {
        void WriteLog(string value);
    }
}