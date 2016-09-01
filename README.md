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

Log entries will now be appended to the list defined in the RedisOptions object that you passed to the logger factory extension method.
