using System;
using System.Collections.Generic;

namespace Ecommerce_System
{
    public class ProductInventoryList : List<Product>
    {
        public int GetTotalStockUnits()
        {
            int sum = 0;
            foreach (var item in this) sum += item.StockQuantity;
            return sum;
        }
    }

    public static class ProductExtensions
    {
        public static bool IsExpensive(this Product? product) => product != null && product.Price > 5000;
    }

    internal class Program
    {
        static ProductRepository repository = new ProductRepository();
        static Dictionary<int, Product> productLookup = new Dictionary<int, Product>();

        static void Main(string[] args)
        {
            SeedData();

            while (true)
            {
                Console.WriteLine("\n --- E-COMMERCE SYSTEM --- ");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Remove Product");
                Console.WriteLine("3. Display All Products");
                Console.WriteLine("4. Search Product");
                Console.WriteLine("5. Sort by Price");
                Console.WriteLine("6. Sort by Name");
                Console.WriteLine("7. Add Stock");
                Console.WriteLine("8. Remove Stock");
                Console.WriteLine("9. Display Expensive Products");
                Console.WriteLine("10. Display Price > 1000");
                Console.WriteLine("11. Demo Interfaces");
                Console.WriteLine("12. Demo Inherited List");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1": AddProduct(); break;
                    case "2": RemoveProduct(); break;
                    case "3": DisplayAll(); break;
                    case "4": SearchProduct(); break;
                    case "5": repository.SortByPrice(); DisplayAll(); break;
                    case "6": repository.SortByName(); DisplayAll(); break;
                    case "7": ModifyStock(isAdding: true); break;
                    case "8": ModifyStock(isAdding: false); break;
                    case "9": DisplayExpensive(); break;
                    case "10": DisplayAbove1000(); break;
                    case "11": DemoInterfaces(repository.GetAll(), repository.GetAll(), repository.GetAll()); break;
                    case "12": DemoInheritedList(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid selection."); break;
                }
            }
        }

        static void SeedData()
        {
            AddEntity(new ElectronicProduct(101, "Gaming Laptop", 6200m, 10, 24));
            AddEntity(new ElectronicProduct(102, "Phone", 3500m, 20, 12));
            AddEntity(new ClothingProduct(103, "Jacket", 1200m, 15, "L"));
            AddEntity(new ClothingProduct(104, "T-Shirt", 350m, 50, "M"));
        }

        static void AddEntity(Product product)
        {
            repository.Add(product);
            productLookup[product.Id] = product;
        }

        static void AddProduct()
        {
            Console.Write("Type (1: Electronic, 2: Clothing): ");
            string? type = Console.ReadLine();

            Console.Write("ID: "); int.TryParse(Console.ReadLine(), out int id);
            Console.Write("Name: "); string name = Console.ReadLine() ?? "";
            Console.Write("Price: "); decimal.TryParse(Console.ReadLine(), out decimal price);
            Console.Write("Stock: "); int.TryParse(Console.ReadLine(), out int stock);

            if (type == "1")
            {
                Console.Write("Warranty: "); int.TryParse(Console.ReadLine(), out int w);
                AddEntity(new ElectronicProduct(id, name, price, stock, w));
            }
            else
            {
                Console.Write("Size: "); string size = Console.ReadLine() ?? "M";
                AddEntity(new ClothingProduct(id, name, price, stock, size));
            }
        }

        static void RemoveProduct()
        {
            Console.Write("Enter ID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int id) && productLookup.TryGetValue(id, out var p))
            {
                repository.Remove(p);
                productLookup.Remove(id);
                Console.WriteLine("Removed successfully.");
            }
            else Console.WriteLine("Product not found.");
        }

        static void DisplayAll()
        {
            foreach (var p in repository.GetAll()) p.DisplayInfo();
        }

        static void SearchProduct()
        {
            Console.Write("Search by (1: ID, 2: Name): ");
            if (Console.ReadLine() == "1")
            {
                Console.Write("Enter ID: "); int.TryParse(Console.ReadLine(), out int id);
                if (productLookup.TryGetValue(id, out var p)) p.DisplayInfo();
                else Console.WriteLine("Not found.");
            }
            else
            {
                Console.Write("Enter Name: "); string query = Console.ReadLine() ?? "";
                var p = repository.Find(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
                if (p != null) p.DisplayInfo();
                else Console.WriteLine("Not found.");
            }
        }

        static void ModifyStock(bool isAdding)
        {
            Console.Write("Product ID: "); int.TryParse(Console.ReadLine(), out int id);
            if (!productLookup.TryGetValue(id, out var product)) { Console.WriteLine("Not found."); return; }

            Console.Write("Quantity: "); int.TryParse(Console.ReadLine(), out int qty);
            if (isAdding)
            {
                product.AddStock(qty);
                Console.WriteLine($"New stock: {product.StockQuantity}");
            }
            else
            {
                try
                {
                    product.RemoveStock(qty);
                    Console.WriteLine($"Remaining stock: {product.StockQuantity}");
                }
                catch (InsufficientStockException ex)
                {
                    Console.WriteLine($"[Stock Error]: {ex.Message}");
                }
            }
        }

        static void DisplayExpensive()
        {
            foreach (var p in repository.GetAll())
                if (p.IsExpensive()) p.DisplayInfo();
        }

        static IEnumerable<Product> FilterAbove1000(IEnumerable<Product> source)
        {
            foreach (var p in source)
                if (p.Price > 1000) yield return p;
        }

        static void DisplayAbove1000()
        {
            foreach (var p in FilterAbove1000(repository.GetAll())) p.DisplayInfo();
        }

        static void DemoInterfaces(IList<Product> list, ICollection<Product> coll, IEnumerable<Product> enu)
        {
            Console.WriteLine($"[IList] Index 0: {list[0].Name}");
            Console.WriteLine($"[ICollection] Count: {coll.Count}");
            Console.Write("[IEnumerable] IDs: ");
            foreach (var item in enu) Console.Write($"{item.Id} ");
            Console.WriteLine();
        }

        static void DemoInheritedList()
        {
            ProductInventoryList list = new ProductInventoryList();
            list.AddRange(repository.GetAll());
            Console.WriteLine($"Total inventory stock: {list.GetTotalStockUnits()} items.");
        }
    }
}