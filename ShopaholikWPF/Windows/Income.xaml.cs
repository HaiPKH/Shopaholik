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
        public Income()
        {
            InitializeComponent();
            LoadIncome();
        }

        private void LoadIncome()
        {
            decimal totalPrice = 0;
            ComboBoxItem typeItem = (ComboBoxItem)cboGroup.SelectedItem;
            if (typeItem != null)
            {
                switch (typeItem.Content.ToString())
                {
                    case "Day":
                        lvInvoices.ItemsSource = context.Invoices.GroupBy(x => x.TransactionTime.Date).Select(g => new { Time = g.Key.Date.ToShortDateString(), Revenue = g.Sum(x => x.Price) }).ToList();
                        break;
                    case "Month":
                        lvInvoices.ItemsSource = context.Invoices.GroupBy(x => new { x.TransactionTime.Month, x.TransactionTime.Year}).Select(g => new { Time = g.Key.Month.ToString() + "/" + g.Key.Year.ToString(), Revenue = g.Sum(x => x.Price) }).ToList();
                        break;
                    case "Year":
                        lvInvoices.ItemsSource = context.Invoices.GroupBy(x => x.TransactionTime.Year).Select(g => new { Time = g.Key, Revenue = g.Sum(x => x.Price) }).ToList();
                        break;
                    default:
                        lvInvoices.ItemsSource = context.Invoices.GroupBy(x => x.TransactionTime.Date).Select(g => new { Time = g.Key.Date.ToShortDateString(), Revenue = g.Sum(x => x.Price) }).ToList();
                        break;
                }
            }
            else
            {
                lvInvoices.ItemsSource =  context.Invoices.ToList();
            }
            foreach (Invoice inv in context.Invoices.ToList())
            {
                totalPrice += inv.Price;
            }
            lbTotalIncome.Content = totalPrice;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMenu adm = new AdminMenu();
            Application.Current.MainWindow.Content = adm.Content;
            Application.Current.MainWindow.Title = "Admin Menu";
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadIncome();
        }
    }
}
