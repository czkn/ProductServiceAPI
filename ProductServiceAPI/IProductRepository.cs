using ProductServiceAPI.DTOs;

namespace ProductServiceAPI
{
    public interface IProductRepository
    {
        ProductDto? GetProductDtoBySKU(string sku);
        public void SavePrices();
        public void SaveProducts();
        public void SaveInventory();
    }
}
