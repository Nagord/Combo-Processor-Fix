using PulsarModLoader.CustomGUI;
using static UnityEngine.GUILayout;

namespace ComboProcessorFix
{
    class GUI : ModSettingsMenu
    {
        public override void Draw()
        {
            if (Button($"{MyPluginInfo.USERS_PLUGIN_NAME} {(Mod.Enabled ? "Enabled" : "Disabled")}"))
            {
                Mod.Enabled = !Mod.Enabled;
                SyncModMessage.SendAllEnabledState();
            }
            if (!PhotonNetwork.isMasterClient)
            {
                Label($"Local mod: {(Mod.Enabled ? "Enabled" : "Disabled")}, Host mod: {(Mod.HostEnabled ? "Enabled" : "Disabled")}, Mod is effectively {((Mod.HostEnabled && Mod.Enabled) ? "Enabled" : "Disabled")}");
            }
        }

        public override string Name()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                return $"{MyPluginInfo.USERS_PLUGIN_NAME}: {((Mod.HostEnabled && Mod.Enabled) ? "Enabled" : "Disabled")}";
            }
            return $"{MyPluginInfo.USERS_PLUGIN_NAME}: {(Mod.Enabled ? "Enabled" : "Disabled")}";
        }
    }
}
