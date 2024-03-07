namespace ProductServiceAPI.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string EAN { get; set; }
        public string ProducerName { get; set; }
        public string Category { get; set; }
        public string DefaultImage { get; set; }
        public int StockQuantity { get; set; }
        public string LogisticUnit { get; set; }
        public decimal NetPurchasePrice { get; set; }
        public decimal ShippingCost { get; set; }      
    }
}
