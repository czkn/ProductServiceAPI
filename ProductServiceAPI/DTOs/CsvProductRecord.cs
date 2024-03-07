using CsvHelper.Configuration.Attributes;

namespace ProductServiceAPI.DTOs
{
    public class CsvProductRecord
    {
        public int ID { get; set; }
        public string SKU { get; set; }
        [Name("name")]
        public string Name { get; set; }

        public string EAN { get; set; }

        [Name("producer_name")]
        public string ProducerName { get; set; }

        [Name("category")]
        public string Category { get; set; }

        [Name("is_wire")]
        public bool IsWire { get; set; }

        [Name("available")]
        public bool Available { get; set; }

        [Name("is_vendor")]
        public bool IsVendor { get; set; }

        [Name("default_image")]
        public string DefaultImage { get; set; }
    }
}
