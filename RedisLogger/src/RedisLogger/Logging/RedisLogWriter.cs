using System;
using StackExchange.Redis;
using Newtonsoft.Json;
namespace Common.Logging {
    public class RedisLogWriter : IDisposable, IRedisLogWriter {
        private readonly IDatabase _db;
        private readonly ConnectionMultiplexer _con;
        private readonly string _listName;

        public RedisLogWriter(RedisOptions options) {
            _con = ConnectionMultiplexer.Connect(string.Format("{0}:{1}", options.Hostname, options.Port));
            _db = _con.GetDatabase();
            _listName = options.ListName;
        }

        public void WriteLog(string value) {
            _db.ListRightPush(_listName, value);
        }

        public void Dispose() {
            if (_con == null) {
                return;
            }
            _con.Close(false);
            _con.Dispose();
        }
    }
}