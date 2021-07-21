using StackExchange.Redis;
using System;
using Log = Jotunn.Logger;

namespace LuvlyClans.Server.Redis
{
    public class RedisManager
    {
        private static RedisManager redisman;
        public static ConnectionMultiplexer redis;

        private static string instanceGUID = Guid.NewGuid().ToString();
        private static string clientName = $"luvlyclans-server-{instanceGUID}";

        private static string host;
        private static int port;
        private static string pass;
        private static int db;

        private RedisManager()
        {
            Connect();
        }

        public static RedisManager GetInstance()
        {
            if (redisman == null)
            {
                redisman = new RedisManager();
            }

            return redisman;
        }

        public IDatabase GetDatabase()
        {
            return redis.GetDatabase();
        }

        private static ConfigurationOptions Config()
        {
            host = LuvlyClans.m_redis_host.Value;
            port = LuvlyClans.m_redis_port.Value;
            pass = LuvlyClans.m_redis_pass.Value;
            db = LuvlyClans.m_redis_db.Value;

            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { host, port }
                },
                ClientName = clientName,
                ChannelPrefix = "luvlyclans",
                Password = pass
            };

            if (db != -1)
            {
                config.DefaultDatabase = db;
            }

            return config;
        }

        private static void Connect()
        {
            try
            {
                redis = ConnectionMultiplexer.Connect(Config());
            } catch (Exception e)
            {
                Log.LogWarning($"Unable to connect to Redis: {e}");
            }
        }
    }
}
