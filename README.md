#RedisLogger#
##Append .NET Core log entries to a Redis List in LogStash format.##

#Configuration#
Startup.cs - Configure Method:
```cs
var logWriter = new RedisLogWriter(new RedisOptions() {
   Hostname = "localhost",
   Port = "6379",
   ListName = "logstash"
});
loggerFactory.AddRedis(logWriter);
```

The `AddRedis` extension method automatically sets up a filter for you based on the current assembly name, so the logger is only enabled when the requesting type (type used in ILogger<T>) begins with your projects root namespace.

Log entries will now be appended to the list defined in the RedisOptions object that you passed to the logger factory extension method (provided you use the convenience extension methods, or call the logger.Log method with RedisLogEntry as the type argument).

#Usage#

To use, simply inject an `ILogger<T>` (where `T` is the class that is requesting the logger) call it with one of the extension methods defined in `RedisLoggerExtensions`.

```cs
_logger.LogStashNone(eventId, httpContext, exception, "Some message - {arg0}", args);
_logger.LogStashInfo(eventId, httpContext, exception, "Some message - {arg0}", args...);
_logger.LogStashDebug(eventId, httpContext, exception, "Some message - {arg0}", args...);
_logger.LogStashWarning(eventId, httpContext, exception, "Some message - {arg0}", args...);
_logger.LogStashError(eventId, httpContext, exception, "Some message - {arg0}", args...);
_logger.LogStashTrace(eventId, httpContext, exception, "Some message - {arg0}", args...);
```
The exception, and event id objects are optional and overloads exist for cases in which you might want to exclude one or both of them. By default, if the message argument is `null` it is replaced by the exception message (one of these must be non null). 

You also have the option of calling the logger using the ILogger interface's `Log` method, but this is more verbose.

```cs
logger.Log<RedisLogEntry>(level, id, redisLogEntry, exception, DefaultMessageFormatter);
```
