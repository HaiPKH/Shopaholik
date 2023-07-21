using Microsoft.AspNetCore.SignalR.Client;
using ShopaholikWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShopaholikWPF.Windows
{
    /// <summary>
    /// Interaction logic for PurchaseHistory.xaml
    /// </summary>
    public partial class PurchaseHistory : Window
    {
        HubConnection connection;
        public PurchaseHistory()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5176/shopaholikhub")
               .Build();
            connection.StartAsync().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    MessageBox.Show(task.Exception + "\n" +task.Exception.StackTrace, "Warning");
                    connection.StartAsync();
                }
            });
            connection.On<Invoice>("addinvoice", (invoice)
                 =>
            {
                Dispatcher.BeginInvoke((Action)(() =>

                {
                    ShopaholikContext context = new ShopaholikContext();
                    lvInvoices.ItemsSource = context.Invoices.Where(invoice => invoice.BuyerName == Application.Current.Properties["Username"] as string).ToList();
                    lvInvoices.Items.Refresh();
                }));
            });
            LoadInvoices();
        }
        private void LoadInvoices()
        {
            ShopaholikContext context = new ShopaholikContext();
            lvInvoices.ItemsSource = context.Invoices.Where(invoice => invoice.BuyerName == Application.Current.Properties["Username"] as string).ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Invoice invoice = lvInvoices.SelectedItem as Invoice;
            ShopaholikContext context = new ShopaholikContext();
            context.Invoices.Remove(invoice);
            context.SaveChanges();
            lvInvoices.Items.Refresh();
            lvItems.ItemsSource = null;
            LoadInvoices();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMenu adm = new AdminMenu();
            Application.Current.MainWindow.Content = adm.Content;
            Application.Current.MainWindow.Title = "Admin Menu";
        }

        private void lvInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Invoice invoice = lvInvoices.SelectedItem as Invoice;
            if (invoice != null)
            {
                List<CartItem> items = new List<CartItem>();
                items = JsonSerializer.Deserialize<List<CartItem>>(invoice.Items);
                lvItems.ItemsSource = items;
            }

        }
    }
}
