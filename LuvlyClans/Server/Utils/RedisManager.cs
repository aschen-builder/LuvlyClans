using StackExchange.Redis;
using LuvlyClans.Server.Types;
using System;

using Log = Jotunn.Logger;

namespace LuvlyClans.Server.Utils
{
    public class RedisManager
    {
        public string InstanceGUID;

        public const string key = "clans";

        private static RedisManager instance;

        public ConnectionMultiplexer data;
        public ConnectionMultiplexer sub;

        private const int connectionTimeout = 10000;
        private const int syncTimeout = 10000;

        public string clansString;

        private static string subChannel = $"__keyspace@{LuvlyClans.redisDB.Value}__:{key}";

        private RedisManager()
        {
            InstanceGUID = Guid.NewGuid().ToString();

            Connect();

            SubToKeyspace();
        }

        public static RedisManager GetInstancec()
        {
            if (instance == null)
            {
                instance = new RedisManager();
            }

            return instance;
        }

        private ConfigurationOptions ConfigBuilder(string client)
        {
            return new ConfigurationOptions
            {
                EndPoints =
                {
                    {
                        LuvlyClans.redisHost.Value == "localhost" ? "127.0.0.1" : LuvlyClans.redisHost.Value,
                        LuvlyClans.redisPort.Value
                    }
                },
                Password = LuvlyClans.redisPass.Value,
                ClientName = client,
                AbortOnConnectFail = false,
                ConnectTimeout = connectionTimeout,
                SyncTimeout = syncTimeout
            };
        }

        public void Connect()
        {
            Log.LogInfo("Connecting to Redis Server");

            try
            {
                data = ConnectionMultiplexer.Connect(ConfigBuilder($"data-{InstanceGUID}"));
                sub = ConnectionMultiplexer.Connect(ConfigBuilder($"sub-{InstanceGUID}"));
            }
            catch (Exception e)
            {
                Log.LogWarning($"Unable to connect clients to Redis server: {e}");
            }
        }

        public string GetClansString()
        {
            if (string.IsNullOrEmpty(clansString))
            {
                GetClans();
            }

            return clansString;
        }

        public void SetClansString(string clans)
        {
            if (clansString != clans)
            {
                clansString = clans;
            }
        }

        public void GetClans()
        {
            clansString = data.GetDatabase().StringGet(key);
        }

        public void SetClans()
        {
            data.GetDatabase().StringSetAsync(key, GetClansString());
        }

        private void SubToKeyspace()
        {
            sub.GetSubscriber().Subscribe(subChannel, (channel, message) => SubHandler(channel, message));
        }

        private void SubHandler(RedisChannel channel, RedisValue message)
        {
            if (channel == subChannel)
            {
                if (message == "set")
                {
                    Log.LogInfo("Recevied sync request from Redis server");

                    GetClans();

                    /** sync all game clients with clans manager */
                }
            }
        }
    }
}
