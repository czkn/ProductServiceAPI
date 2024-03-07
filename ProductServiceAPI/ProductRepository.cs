using System.Data.SqlClient;
using System.Data;
using ProductServiceAPI.DTOs;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Formats.Asn1;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Net;
using CsvHelper.TypeConversion;
using ProductServiceAPI.CustomConverters;

namespace ProductServiceAPI
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void SavePrices()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var pricesFileUrl = _configuration["Urls:PricesFileUrl"];

            using (var webClient = new WebClient())
            using (var stream = webClient.OpenRead(pricesFileUrl))
            using (var reader = new StreamReader(stream))
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                var csv = new CsvReader(reader,
                    new CsvConfiguration(CultureInfo.InvariantCulture) { BadDataFound = null, HasHeaderRecord = false });

                csv.Context.TypeConverterCache.AddConverter<decimal?>(new CustomVatRateConverter());

                var records = csv.GetRecords<CsvPricesRecord>();

                foreach (var record in records)
                {
                    var price = new
                    {
                        ID = record.ID,
                        SKU = record.SKU,
                        NettPrice = record.NettPrice,
                        NettPriceAfterDiscount = record.NettPriceAfterDiscount,
                        VATRate = record.VATRate,
                        NettPriceAfterDiscountForLogisticUnit = record.NettPriceAfterDiscountForLogisticUnit
                    };

                    dbConnection.Execute("INSERT INTO Prices (ID, SKU, NettPrice, NettPriceAfterDiscount, VATRate, NettPriceAfterDiscountForLogisticUnit) " +
                                        "VALUES (@ID, @SKU, @NettPrice, @NettPriceAfterDiscount, @VATRate, @NettPriceAfterDiscountForLogisticUnit)", price);
                }
            }
        }

        public void SaveProducts()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var productsFileUrl = _configuration["Urls:ProductsFileUrl"];

            using (var webClient = new WebClient())
            using (var stream = webClient.OpenRead(productsFileUrl))
            using (var reader = new StreamReader(stream))
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                var csv = new CsvReader(reader,
                    new CsvConfiguration(CultureInfo.InvariantCulture) { BadDataFound = null, Delimiter = ";",
                        ShouldSkipRecord = context => context.Row[0] == "__empty_line__"
                    });

                var records = csv.GetRecords<CsvProductRecord>();

                foreach (var record in records)
                {
                    var product = new
                    {
                        ID = record.ID,
                        SKU = record.SKU,
                        Name = record.Name,
                        EAN = record.EAN,
                        ProducerName = record.ProducerName,
                        Category = record.Category,
                        IsWire = record.IsWire,
                        Available = record.Available,
                        IsVendor = record.IsVendor,
                        DefaultImage = record.DefaultImage
                    };

                    dbConnection.Execute("INSERT INTO Products (ID, SKU, Name, EAN, ProducerName, Category, IsWire, Available, IsVendor, DefaultImage) " +
                                        "VALUES (@ID, @SKU, @Name, @EAN, @ProducerName, @Category, @IsWire, @Available, @IsVendor, @DefaultImage)", product);
                }
            }
        }

        public void SaveInventory()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var inventoryFileUrl = _configuration["Urls:InventoryFileUrl"];

            using (var webClient = new WebClient())
            using (var stream = webClient.OpenRead(inventoryFileUrl))
            using (var reader = new StreamReader(stream))
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                });

                var records = csv.GetRecords<CsvInventoryRecord>();

                foreach (var record in records)
                {
                    var inventoryItem = new
                    {
                        ProductID = record.ProductID,
                        SKU = record.SKU,
                        Unit = record.Unit,
                        Qty = record.Qty,
                        Manufacturer = record.Manufacturer,
                        Shipping = record.Shipping,
                        ShippingCost = record.ShippingCost,
                    };

                    dbConnection.Execute("INSERT INTO Inventory (ProductID, SKU, Unit, Qty, Manufacturer, Shipping, ShippingCost) " +
                                        "VALUES (@ProductID, @SKU, @Unit, @Qty, @Manufacturer, @Shipping, @ShippingCost)", inventoryItem);
                }
            }
        }

        public ProductDto? GetProductDtoBySKU(string sku)
        {

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                string query = @"
            SELECT 
                p.Name, 
                p.EAN, 
                p.ProducerName, 
                p.Category, 
                p.DefaultImage, 
                i.Qty AS StockQuantity, 
                i.ShippingCost,
                i.Unit AS LogisticUnit,
                pr.NettPrice AS NetPurchasePrice
            FROM Products p
            LEFT JOIN Inventory i ON p.SKU = i.SKU
            LEFT JOIN Prices pr ON p.SKU = pr.SKU
            WHERE p.SKU = @SKU";

                return dbConnection.QueryFirstOrDefault<ProductDto>(query, new { SKU = sku });
            }
        }
    }
}
