using CsvHelper.Configuration.Attributes;

namespace ProductServiceAPI.DTOs
{
    public class CsvInventoryRecord
    {
        [Name("product_id")]
        public int ProductID { get; set; }
        [Name("sku")]
        public string SKU { get; set; }
        [Name("unit")]
        public string Unit { get; set; }
        [Name("qty")]
        public decimal Qty { get; set; }
        [Name("manufacturer_ref_num")]
        public string Manufacturer { get; set; }
        [Name("shipping")]
        public string Shipping { get; set; }
        [Name("shipping_cost")]
        public decimal? ShippingCost { get; set; }
    }
}
