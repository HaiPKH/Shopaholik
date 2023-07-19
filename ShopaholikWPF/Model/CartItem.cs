using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopaholikWPF.Model
{
    public class CartItem
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
