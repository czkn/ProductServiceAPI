using CsvHelper.Configuration.Attributes;


namespace ProductServiceAPI.DTOs
{
    public class CsvPricesRecord
    {
        public string ID { get; set; }
        public string SKU { get; set; }
        public decimal NettPrice { get; set; }
        public decimal NettPriceAfterDiscount { get; set; }
        public decimal? VATRate { get; set; }
        public decimal? NettPriceAfterDiscountForLogisticUnit { get; set; }
    }
}
