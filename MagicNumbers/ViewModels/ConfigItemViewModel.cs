using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MagicNumbers.ViewModels
{
    public class ConfigItemViewModel : INotifyPropertyChanged
    {
        public ConfigItemViewModel()
        {
            TooltipDefinitions = new ObservableCollection<TooltipDefinitionViewModel>();
            TooltipDefinitions.CollectionChanged += new NotifyCollectionChangedEventHandler(Refresh_Json);
        }

        private void Refresh_Json(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("TooltipDefinitionsJson");
        }

        public ConfigItemViewModel(string filePattern, bool isRegex, List<TooltipDefinitionViewModel> tooltipDefinitions)
        {
            FilePattern = filePattern;
            IsRegex = isRegex;
            TooltipDefinitions = new ObservableCollection<TooltipDefinitionViewModel>(tooltipDefinitions);
            TooltipDefinitions.CollectionChanged += new NotifyCollectionChangedEventHandler(Refresh_Json);
        }

        public string FilePattern { get; set; }

        public bool IsRegex { get; set; }

        private ObservableCollection<TooltipDefinitionViewModel> tooltipDefinitions;
        private string tooltipDefinitionsJson;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<TooltipDefinitionViewModel> TooltipDefinitions
        {
            get
            {
                return tooltipDefinitions;
            }
            set
            {
                tooltipDefinitions = value;
                tooltipDefinitionsJson = JsonSerializationHelper.ToJson(tooltipDefinitions);
                OnPropertyChanged();
                OnPropertyChanged("TooltipDefinitionsJson");
            }
        }

        public string TooltipDefinitionsJson
        {
            get
            {
                return tooltipDefinitionsJson;
            }
            set
            {
                tooltipDefinitionsJson = value;
                tooltipDefinitions = JsonSerializationHelper.Deserialize<ObservableCollection<TooltipDefinitionViewModel>>(tooltipDefinitionsJson);
                OnPropertyChanged();
                OnPropertyChanged("TooltipDefinitions");
            }
        }
    }

    [DataContract]
    public class TooltipDefinitionViewModel
    {
        public TooltipDefinitionViewModel()
        {
        }

        public TooltipDefinitionViewModel(string input, string description)
        {
            Input = input;
            Description = description;
        }

        [DataMember(Name = "input")]
        public string Input { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
