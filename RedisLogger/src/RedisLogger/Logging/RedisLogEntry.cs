using System;

namespace Common.Logging {
    public class RedisLogEntry {
        public string appname { get; set; }
        public DateTime date { get; set; }
        public string ipaddress { get; set; }
        public string level { get; set; }
        public string logger { get; set; }
        public string method { get; set; }
        public string uri { get; set; }
        public Exception exception { get; set; }
        public string host { get; set; }
        public string eventid { get; set; }
        public string message { get; set; }
    }
}
