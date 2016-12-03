using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace MagicNumbers
{
    public interface IFileSpecificTooltipProvider
    {
        TooltipDefinition[] GetFileSpecificTooltipDefinitions(string fileFullPath);
    }

    public class FileSpecificTooltipProvider : IFileSpecificTooltipProvider
    {
        private const string TemporaryConfig = "[{ \"input\":\"123\",\"description\":\"Magic number A\"}, { \"input\":\"124\",\"description\":\"Magic number B\"}]";

        public TooltipDefinition[] GetFileSpecificTooltipDefinitions(string fileFullPath)
        {
            return GetConfigForFile(fileFullPath);
        }

        private TooltipDefinition[] GetConfigForFile(string fileFullPath)
        {
            Stream jsonSource = GenerateStreamFromString(TemporaryConfig);
            var s = new DataContractJsonSerializer(typeof(TooltipDefinition[]));
            return (TooltipDefinition[])s.ReadObject(jsonSource);
        }

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }

    [DataContract]
    public class TooltipDefinition
    {
        [DataMember(Name = "input")]
        public string Input { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
