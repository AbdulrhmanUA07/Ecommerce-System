using System;
using System.Collections.Generic;

namespace Ecommerce_System
{
    public class Repository<T> where T : class
    {
        protected readonly List<T> _items = new List<T>();

        public virtual void Add(T item) => _items.Add(item);
        public virtual bool Remove(T item) => _items.Remove(item);
        public virtual T? Find(Predicate<T> match) => _items.Find(match);
        public virtual List<T> GetAll() => new List<T>(_items);


    }
}