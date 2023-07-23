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
            try
            {
                LoadInvoices(int.Parse(txtRecordsNum.Text));
            }
            catch (Exception ex)
            {
                ShopaholikContext context = new ShopaholikContext();
                lvInvoices.ItemsSource = context.Invoices.Where(invoice => invoice.BuyerName == Application.Current.Properties["Username"] as string).ToList();
            }
        }
        private void LoadInvoices(int recordsNum)
        {
            try
            {
                ShopaholikContext context = new ShopaholikContext();
                ComboBoxItem typeItem = (ComboBoxItem)cboOrder.SelectedItem;
                string order = "";
                if (typeItem != null)
                {
                    if (recordsNum > 0 && typeItem.Content.ToString() != String.Empty)
                    {
                        order = typeItem.Content.ToString();
                        List<Invoice> list = context.Invoices.Where(invoice => invoice.BuyerName == Application.Current.Properties["Username"] as string).Take(recordsNum).ToList();
                        switch (order)
                        {
                            case "Date Ascending":
                                lvInvoices.ItemsSource = list.OrderBy(l => l.TransactionTime).ToList();
                                break;
                            case "Date Descending":
                                lvInvoices.ItemsSource = list.OrderByDescending(l => l.TransactionTime).ToList();
                                break;
                            case "Price Ascending":
                                lvInvoices.ItemsSource = list.OrderBy(l => l.Price).ToList();
                                break;
                            case "Price Descending":
                                lvInvoices.ItemsSource = list.OrderByDescending(l => l.Price).ToList();
                                break;
                            default:
                                MessageBox.Show("default");
                                lvInvoices.ItemsSource = list.ToList();
                                break;
                        }
                    }
                    else if (recordsNum > 0 && typeItem.Content.ToString().Equals(String.Empty))
                    {
                        lvInvoices.ItemsSource = context.Invoices.Where(invoice => invoice.BuyerName == Application.Current.Properties["Username"] as string).Take(recordsNum).ToList();
                    }
                    else if (recordsNum <= 0 && typeItem.Content.ToString() != String.Empty)
                    {
                        order = typeItem.Content.ToString();
                        List<Invoice> list = context.Invoices.Where(invoice => invoice.BuyerName == Application.Current.Properties["Username"] as string).ToList();
                        switch (order)
                        {
                            case "Date Ascending":
                                lvInvoices.ItemsSource = list.OrderBy(l => l.TransactionTime).ToList();
                                break;
                            case "Date Descending":
                                lvInvoices.ItemsSource = list.OrderByDescending(l => l.TransactionTime).ToList();
                                break;
                            case "Price Ascending":
                                lvInvoices.ItemsSource = list.OrderBy(l => l.Price).ToList();
                                break;
                            case "Price Descending":
                                lvInvoices.ItemsSource = list.OrderByDescending(l => l.Price).ToList();
                                break;
                            default:
                                MessageBox.Show("default");
                                lvInvoices.ItemsSource = list.ToList();
                                break;
                        }
                    }
                }
                else
                {
                    lvInvoices.ItemsSource = context.Invoices.Where(invoice => invoice.BuyerName == Application.Current.Properties["Username"] as string).Take(recordsNum).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Invoice invoice = lvInvoices.SelectedItem as Invoice;
            ShopaholikContext context = new ShopaholikContext();
            context.Invoices.Remove(invoice);
            context.SaveChanges();
            lvInvoices.Items.Refresh();
            lvItems.ItemsSource = null;
            if (txtRecordsNum.Text != String.Empty)
            {
                LoadInvoices(int.Parse(txtRecordsNum.Text));
            }
            else
            {
                LoadInvoices(context.Invoices.ToList().Count());
            }
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

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadInvoices(int.Parse(txtRecordsNum.Text));
            }
            catch (Exception ex)
            {
                ShopaholikContext context = new ShopaholikContext();
                LoadInvoices(context.Invoices.ToList().Count());
            }
        }
    }
}
