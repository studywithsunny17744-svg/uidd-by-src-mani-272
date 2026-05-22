using DiscordRPC;
using System;

namespace proxy_uid.Core
{
    internal class RPC
    {
        public static DiscordRpcClient client;
        public static Timestamps rpctimestamp { get; set; }
        private static RichPresence presence;

        public static void InitializeRPC()
        {
            try
            {
                client = new DiscordRpcClient("1493261397927395460");

                client.OnReady += (sender, e) =>
                {
                    try
                    {
                        if (presence != null && client != null)
                            client.SetPresence(presence);
                    }
                    catch { }
                };

                client.Initialize();

                presence = new RichPresence
                {
                    Assets = new Assets
                    {
                        LargeImageKey = "logo",
                        LargeImageText = AppBranding.AppTitle,
                        SmallImageText = AppBranding.AppSubtitle
                    }
                };

                UpdateDiscordPresence();
            }
            catch
            {
                client = null;
                presence = null;
            }
        }

        public static void SetState(string state, bool watching = false)
        {
            if (client == null || presence == null) return;

            try
            {
                if (watching)
                    state = "Looking at " + state;

                presence.State = state;
                client.SetPresence(presence);
            }
            catch { }
        }

        public static void UpdateDiscordPresence()
        {
            if (client == null || presence == null) return;

            try
            {
                presence.Timestamps = Timestamps.Now;
                presence.Details = AppBranding.AppTitle;
                presence.State = AppBranding.AppSubtitle;
                client.SetPresence(presence);
            }
            catch { }
        }

        public static void Shutdown()
        {
            try { client?.Dispose(); } catch { }
            client = null;
            presence = null;
        }
    }
}
