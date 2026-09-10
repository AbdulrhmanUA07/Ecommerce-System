using System;

namespace Ecommerce_System
{
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string message) : base(message) { }

    }
}