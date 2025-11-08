using PulsarModLoader;

namespace ComboProcessorFix
{
    public class Mod : PulsarMod
    {
        public override string Version => MyPluginInfo.PLUGIN_VERSION;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string Name => MyPluginInfo.USERS_PLUGIN_NAME;


        public static bool Enabled = true;
        public static bool HostEnabled = false;

        public override void Disable()
        {
            Enabled = false;
            SyncModMessage.SendAllEnabledState();
        }

        public override void Enable()
        {
            Enabled = true;
            SyncModMessage.SendAllEnabledState();
        }

        public override bool IsEnabled()
        {
            return Enabled;
        }

        public override string HarmonyIdentifier()
        {
            return MyPluginInfo.PLUGIN_GUID;
        }
    }
}
