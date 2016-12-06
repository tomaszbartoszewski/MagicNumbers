using System.Runtime.Serialization;

namespace MagicNumbers.Data
{
    [DataContract]
    public class Config
    {
        [DataMember(Name = "configItems")]
        public ConfigItem[] ConfigItems { get; set; }
    }

    [DataContract]
    public class ConfigItem
    {
        public ConfigItem()
        {
        }

        public ConfigItem(string filePattern, bool isRegex, TooltipDefinition[] tooltipDefinitions)
        {
            FilePattern = filePattern;
            IsRegex = isRegex;
            TooltipDefinitions = tooltipDefinitions;
        }

        [DataMember(Name = "filePattern")]
        public string FilePattern { get; set; }

        [DataMember(Name = "isRegex")]
        public bool IsRegex { get; set; }

        [DataMember(Name = "tooltipDefinitions")]
        public TooltipDefinition[] TooltipDefinitions { get; set; }
    }

    [DataContract]
    public class TooltipDefinition
    {
        public TooltipDefinition()
        {
        }

        public TooltipDefinition(string input, string description)
        {
            Input = input;
            Description = description;
        }

        [DataMember(Name = "i")]
        public string Input { get; set; }

        [DataMember(Name = "d")]
        public string Description { get; set; }
    }
}
