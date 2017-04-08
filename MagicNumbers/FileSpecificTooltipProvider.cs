using MagicNumbers.Data;
using System.Linq;

namespace MagicNumbers
{
    public interface IFileSpecificTooltipProvider
    {
        TooltipDefinition[] GetFileSpecificTooltipDefinitions(string fileFullPath);
    }

    public class FileSpecificTooltipProvider : IFileSpecificTooltipProvider
    {
        private ConfigRepository configRepository;

        public FileSpecificTooltipProvider()
        {
            configRepository = new ConfigRepository();
        }

        public TooltipDefinition[] GetFileSpecificTooltipDefinitions(string fileFullPath)
        {
            //return new TooltipDefinition[1] {
            //    new TooltipDefinition { Description="asd", Input="asd" }
            //    };
            var config = configRepository.GetConfig();
            return config.ConfigItems.Where(ci => ci.IsFileMatchingPattern(fileFullPath)).SelectMany(ci => ci.TooltipDefinitions).ToArray();
        }
    }
}
