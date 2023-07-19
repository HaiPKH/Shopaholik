using System;
using System.Collections.Generic;

namespace ShopaholikWPF.Model
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public int UnitsInStock { get; set; }
        public int Price { get; set; }
    }
}
