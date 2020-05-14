using System;
using System.Runtime.Serialization;

namespace Necromancy.Server.Setting
{
    [DataContract]
    public class NecSecret
    {
        [DataMember(Order = 1)] public string DatabasePassword { get; set; }
        [DataMember(Order = 10)] public string DiscordBotToken { get; set; }

        public NecSecret()
        {
            string envDbPass = Environment.GetEnvironmentVariable("DB_PASS");
            if (!string.IsNullOrEmpty(envDbPass))
            {
                DatabasePassword = envDbPass;
            }

            string envDiscordBotToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
            if (!string.IsNullOrEmpty(envDiscordBotToken))
            {
                DiscordBotToken = envDiscordBotToken;
            }
        }
    }
}
