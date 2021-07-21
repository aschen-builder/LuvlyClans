using StackExchange.Redis;
using System;
using Log = Jotunn.Logger;

namespace LuvlyClans.Server.Redis
{
    public class RedisManager
    {
        private static RedisManager redisman;
        public static ConnectionMultiplexer redisdata;
        public static ConnectionMultiplexer redissub;

        private static string instanceGUID = Guid.NewGuid().ToString();
        private static string dataClientName = $"luvlyclans-data-{instanceGUID}";
        private static string subClientName = $"luvlyclans-sub-{instanceGUID}";

        private static string host;
        private static int port;
        private static string pass;
        private static int db;

        private RedisManager()
        {
            Log.LogInfo("Initializing Redis connection");

            Connect(dataClientName);
            Connect(subClientName);

            Log.LogInfo("Subscribing to Redis sync channel");

            SyncToSub();
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
            return redisdata.GetDatabase();
        }

        private static ConfigurationOptions Config(string clientName)
        {
            host = LuvlyClans.m_redis_host.Value == "localhost" ? "127.0.0.1" : LuvlyClans.m_redis_host.Value;
            port = LuvlyClans.m_redis_port.Value;
            pass = LuvlyClans.m_redis_pass.Value;
            db = LuvlyClans.m_redis_db.Value;

            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { host, port }
                },
                Password = pass,
                ClientName = clientName,
                AbortOnConnectFail = false,
                ConnectTimeout = 10000,
                SyncTimeout = 10000
            };

            if (db != -1 && clientName != subClientName)
            {
                config.DefaultDatabase = db;
            }

            return config;
        }

        private static void Connect(string clientName)
        {
            try
            {
                redisdata = ConnectionMultiplexer.Connect(Config(clientName));

                if (redisdata.IsConnecting)
                {
                    Log.LogInfo($"Connecting [{clientName}] to Redis DB");
                }

                if (redisdata.IsConnected)
                {
                    Log.LogInfo($"Connected [{clientName}] to Redis DB");
                }
            } catch (Exception e)
            {
                Log.LogWarning($"Unable to connect to Redis client [{clientName}]: {e}");
            }
        }

        private static void SyncToSub()
        {
            if (redissub != null)
            {
                redissub.GetSubscriber().Subscribe("sync", (channel, message) => SubHandler(channel, message));
            }
        }

        private static void SubHandler(RedisChannel channel, RedisValue message)
        {
            if (channel == "sync")
            {
                if (!message.IsNullOrEmpty)
                {
                    Log.LogInfo($"Received Redis sync message:: {message}");
                }
            }

            return;
        }
    }
}
