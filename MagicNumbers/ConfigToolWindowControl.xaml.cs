namespace MagicNumbers
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ConfigToolWindowControl.
    /// </summary>
    public partial class ConfigToolWindowControl : UserControl
    {
        ObservableCollection<ConfigItem> Items = new ObservableCollection<ConfigItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigToolWindowControl"/> class.
        /// </summary>
        public ConfigToolWindowControl()
        {
            this.InitializeComponent();
            Items.Add(new ConfigItem { FilePattern = "Test.txt", IsRegex = true });
            Items.Add(new ConfigItem { FilePattern = "Test2.txt", IsRegex = false });
            Items.Add(new ConfigItem { FilePattern = "Test3.txt", IsRegex = false });
            Items.Add(new ConfigItem { FilePattern = "Test4.txt", IsRegex = true });
            AllConfigs.ItemsSource = Items;
            AllConfigs.SelectedItem = Items[0];
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
    }

    public class ConfigItem
    {
        public string FilePattern { get; set; }

        public bool IsRegex { get; set; }

        public string TooltipDefinitions { get; set; }
    }
}