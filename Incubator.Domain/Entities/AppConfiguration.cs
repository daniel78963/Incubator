using Incubator.Domain.Enums;

namespace Incubator.Domain.Entities
{
    public class AppConfiguration
    {
        public StorageMethod StorageType { get; set; } = StorageMethod.FlatFile;
        public string FlatFileFormat { get; set; } = "yyyyMMdd";
    }
}
