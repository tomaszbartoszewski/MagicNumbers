using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;

namespace MagicNumbers.Data
{
    public class ConfigRepository
    {
        private const string SettingsStoreCollectionPath = "Text Editor";
        private const string SettingsStorePropertyName = "MagicNumbersConfig";

        public Config GetConfig()
        {
            WritableSettingsStore userSettingsStore = GetSettingsStore();
            string configJson;
            if (userSettingsStore.PropertyExists(SettingsStoreCollectionPath, SettingsStorePropertyName))
            {
                configJson = userSettingsStore.GetString(SettingsStoreCollectionPath, SettingsStorePropertyName);
                var config = JsonSerializationHelper.Deserialize<Config>(configJson);
                return config;
            }

            return new Config { ConfigItems = new ConfigItem[0] };
        }

        public void Save(Config config)
        {
            WritableSettingsStore userSettingsStore = GetSettingsStore();
            var configJson = JsonSerializationHelper.ToJson(config);
            userSettingsStore.SetString(SettingsStoreCollectionPath, SettingsStorePropertyName, configJson);
        }

        private WritableSettingsStore GetSettingsStore()
        {
            SettingsManager settingsManager = new ShellSettingsManager(ConfigCommand.ServiceProviderForWindow);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            return userSettingsStore;
        }
    }
}
