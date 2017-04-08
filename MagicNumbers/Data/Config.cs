using System.Runtime.Serialization;
using System.Text.RegularExpressions;

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

        public bool IsFileMatchingPattern(string fileFullPath)
        {
            if (IsRegex)
            {
                Regex rgx = new Regex(FilePattern);
                return rgx.IsMatch(fileFullPath);
            }
            else
            {
                return fileFullPath.Contains(FilePattern);
            }
        }
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
