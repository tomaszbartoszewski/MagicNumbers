using MagicNumbers.Data;
using MagicNumbers.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MagicNumbers
{
    public partial class ConfigToolWindowControl : UserControl
    {
        private ConfigRepository configRepository;

        ObservableCollection<ConfigItemViewModel> Items = new ObservableCollection<ConfigItemViewModel>();

        public ConfigToolWindowControl()
        {
            configRepository = new ConfigRepository();
            this.InitializeComponent();
            InitializeConfigItems();
        }

        private void InitializeConfigItems()
        {
            var config = configRepository.GetConfig();
            var configItems = config.ConfigItems.Select(i => new ConfigItemViewModel(i.FilePattern, i.IsRegex, Map(i.TooltipDefinitions)));
            Items = new ObservableCollection<ConfigItemViewModel>(configItems);
            AllConfigs.ItemsSource = Items;
            if (Items.Any())
            {
                AllConfigs.SelectedItem = Items[0];
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Items.Add(new ConfigItemViewModel());
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
            var configItems = AllConfigs.Items.Cast<ConfigItemViewModel>().ToArray();
            Config config = new Config
                {
                    ConfigItems = configItems.Select(i => new ConfigItem(i.FilePattern, i.IsRegex, Map(i.TooltipDefinitions))).ToArray()
                };
            configRepository.Save(config);
            CloseParentWindow();
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

        private TooltipDefinition[] Map(ObservableCollection<TooltipDefinitionViewModel> tooltipDefinitions)
        {
            return tooltipDefinitions.Select(t => new TooltipDefinition(t.Input, t.Description)).ToArray();
        }

        private List<TooltipDefinitionViewModel> Map(TooltipDefinition[] tooltipDefinitions)
        {
            return tooltipDefinitions.Select(t => new TooltipDefinitionViewModel(t.Input, t.Description)).ToList();
        }
    }
}