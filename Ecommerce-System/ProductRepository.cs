using System;
using System.Collections.Generic;

namespace Ecommerce_System
{
    public class ProductNameComparer : IComparer<Product>
    {
        public int Compare(Product? x, Product? y) =>
            string.Compare(x?.Name, y?.Name, StringComparison.OrdinalIgnoreCase);
    }

    public class ProductRepository : Repository<Product>
    {
        public Product? FindById(int id) => _items.Find(p => p.Id == id);

        public void SortByPrice() => _items.Sort();

        public void SortByName() => _items.Sort(new ProductNameComparer());

    }
}