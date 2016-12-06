using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace MagicNumbers
{
    /// <summary>
    /// Interaction logic for ConfigToolWindowControl.
    /// </summary>
    public partial class ConfigToolWindowControl : UserControl
    {
        ObservableCollection<ConfigItem> Items = new ObservableCollection<ConfigItem>();

        private const string SettingsStoreCollectionPath = "Text Editor";
        private const string SettingsStorePropertyName = "MagicNumbersConfig";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigToolWindowControl"/> class.
        /// </summary>
        public ConfigToolWindowControl()
        {
            this.InitializeComponent();
            InitializeConfigItems();
        }

        private void InitializeConfigItems()
        {
            Items = new ObservableCollection<ConfigItem>(GetCurrentConfigItems());
            AllConfigs.ItemsSource = Items;
            if (Items.Any())
            {
                AllConfigs.SelectedItem = Items[0];
            }
        }

        private ConfigItem[] GetCurrentConfigItems()
        {
            WritableSettingsStore userSettingsStore = GetSettingsStore();
            string configJson;
            if (userSettingsStore.PropertyExists(SettingsStoreCollectionPath, SettingsStorePropertyName))
            {
                configJson = userSettingsStore.GetString(SettingsStoreCollectionPath, SettingsStorePropertyName);
                var config = JsonSerializationHelper.Deserialize<Config>(configJson);
                return config.ConfigItems;
            }

            return new ConfigItem[0];
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Items.Add(new ConfigItem());
            AllConfigs.SelectedItem = Items.Last();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (AllConfigs.SelectedIndex != -1)
            {
                Items.RemoveAt(AllConfigs.SelectedIndex);
            }
        }

        private void SaveAll_Click(object sender, RoutedEventArgs e)
        {
            WritableSettingsStore userSettingsStore = GetSettingsStore();
            var configItems = AllConfigs.Items.Cast<ConfigItem>().ToArray();
            Config config = new Config { ConfigItems = configItems };
            var configJson = JsonSerializationHelper.ToJson(config);
            userSettingsStore.SetString(SettingsStoreCollectionPath, SettingsStorePropertyName, configJson);

            CloseParentWindow();
        }

        private WritableSettingsStore GetSettingsStore()
        {
            SettingsManager settingsManager = new ShellSettingsManager(ConfigCommand.ServiceProviderForWindow);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            return userSettingsStore;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CloseParentWindow();
        }

        private void CloseParentWindow()
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }

    [DataContract]
    public class ConfigItem
    {
        [DataMember(Name = "filePattern")]
        public string FilePattern { get; set; }

        [DataMember(Name = "isRegex")]
        public bool IsRegex { get; set; }

        [DataMember(Name = "tooltipDefinitions")]
        public string TooltipDefinitions { get; set; }
    }

    [DataContract]
    public class Config
    {
        [DataMember(Name = "configItems")]
        public ConfigItem[] ConfigItems { get; set; }
    }
}