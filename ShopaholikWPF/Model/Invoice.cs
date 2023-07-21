using System;
using System.Collections.Generic;

namespace ShopaholikWPF.Model
{
    public partial class Invoice
    {
        public decimal Price { get; set; }
        public DateTime TransactionTime { get; set; }
        public string Items { get; set; } = null!;
        public int Id { get; set; }
        public string BuyerName { get; set; }
    }
}
