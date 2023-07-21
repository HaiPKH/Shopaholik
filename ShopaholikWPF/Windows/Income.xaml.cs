using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ShopaholikWPF.Model;

namespace ShopaholikWPF.Windows
{
    /// <summary>
    /// Interaction logic for Income.xaml
    /// </summary>
    public partial class Income : Window
    {
        ShopaholikContext context = new ShopaholikContext();
        List<Invoice> invoices;
        public Income()
        {
            InitializeComponent();
            invoices =  context.Invoices.ToList();
        }
    }
}
