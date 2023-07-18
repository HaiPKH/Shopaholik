using System;
using System.Collections.Generic;

namespace ShopaholikWPF.Model
{
    public partial class Message
    {
        public int Mid { get; set; }
        public string Sendername { get; set; } = null!;
        public string Receivername { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Timesent { get; set; }
    }
}
