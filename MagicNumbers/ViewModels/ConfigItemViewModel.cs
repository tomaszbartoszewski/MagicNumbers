using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MagicNumbers.ViewModels
{
    public class ConfigItemViewModel
    {
        public ConfigItemViewModel()
        {
            TooltipDefinitions = new List<TooltipDefinitionViewModel>();
        }

        public ConfigItemViewModel(string filePattern, bool isRegex, List<TooltipDefinitionViewModel> tooltipDefinitions)
        {
            FilePattern = filePattern;
            IsRegex = isRegex;
            TooltipDefinitions = tooltipDefinitions;
        }

        public string FilePattern { get; set; }

        public bool IsRegex { get; set; }

        private List<TooltipDefinitionViewModel> tooltipDefinitions;
        private string tooltipDefinitionsJson;

        public List<TooltipDefinitionViewModel> TooltipDefinitions
        {
            get
            {
                return tooltipDefinitions;
            }
            set
            {
                tooltipDefinitions = value;
                tooltipDefinitionsJson = JsonSerializationHelper.ToJson(tooltipDefinitions);
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
                tooltipDefinitions = JsonSerializationHelper.Deserialize<List<TooltipDefinitionViewModel>>(tooltipDefinitionsJson);
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
