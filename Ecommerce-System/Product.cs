using System;

namespace Ecommerce_System
{
    public abstract class Product : IComparable<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public Product(int id, string name, decimal price, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id,-4} | Name: {Name,-15} | Price: ${Price,-8:F2} | Stock: {StockQuantity,-4}");
        }

        public int CompareTo(Product? other)
        {
            if (other is null) return 1;
            return this.Price.CompareTo(other.Price);
        }

        public void AddStock(int quantity) => StockQuantity += quantity;

        public void RemoveStock(int quantity)
        {
            if (quantity > StockQuantity)
                throw new InsufficientStockException($"Cannot remove {quantity} units. Available: {StockQuantity}.");

            StockQuantity -= quantity;
        }
    }

    public class ElectronicProduct : Product
    {
        public int WarrantyMonths { get; set; }

        public ElectronicProduct(int id, string name, decimal price, int stockQuantity, int warrantyMonths)
            : base(id, name, price, stockQuantity)
        {
            WarrantyMonths = warrantyMonths;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"[Type: Electronics] Warranty: {WarrantyMonths} Months");
        }
    }

    public class ClothingProduct : Product
    {
        public string Size { get; set; }

        public ClothingProduct(int id, string name, decimal price, int stockQuantity, string size)
            : base(id, name, price, stockQuantity)
        {
            Size = size;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"[Type: Clothing] Size: {Size}");
        }
    }
}