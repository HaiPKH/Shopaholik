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
    /// Interaction logic for Cart.xaml
    /// </summary>
    public partial class Cart : Window
    {
        List<CartItem> cartItems = Application.Current.Properties["CartItems"] as List<CartItem>;
        HubConnection connection;
        public Cart()
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
            int totalPrice = 0;
            if (cartItems != null)
            {
                foreach (CartItem cartItem in cartItems)
                {
                    totalPrice += cartItem.Product.Price * cartItem.Quantity;
                }
                txtTotalPrice.Text = totalPrice.ToString();
            }

            LoadItems();
        }

        private void LoadItems()
        {
            lvCartItems.ItemsSource = cartItems;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GuestMenu gm = new GuestMenu();
            Application.Current.MainWindow.Content = gm.Content;
            Application.Current.MainWindow.Title = "Menu";
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            if (lvCartItems.ItemsSource != null && txtTotalPrice.Text != string.Empty)
            {
                this.connection.InvokeAsync("PurchaseItems", cartItems);
                ShopaholikContext context = new ShopaholikContext();
                Invoice invoice = new Invoice();
                invoice.BuyerName = Application.Current.Properties["Username"] as string;
                invoice.TransactionTime = DateTime.Now;
                invoice.Price = int.Parse(txtTotalPrice.Text);
                invoice.Items = JsonSerializer.Serialize(cartItems);
                this.connection.InvokeAsync("AddInvoice", invoice);
                Application.Current.Properties["CartItems"] = null;
                cartItems.Clear();
                lvCartItems.ItemsSource = null;
                txtPrice.Clear();
                txtQuantity.Clear();
                txtProdName.Clear();
                txtTotalPrice.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("There are still empty fields", "Error");
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvCartItems.ItemsSource != null && txtTotalPrice.Text != string.Empty)
            {
                CartItem item = lvCartItems.SelectedItem as CartItem;
                CartItem cartItem = cartItems.FirstOrDefault(i => i.Product.Name == item.Product.Name);
                if (cartItem != null)
                {
                    cartItem.Quantity = int.Parse(txtQuantity.Text);
                }
                else
                {
                    MessageBox.Show("Error");
                }
                LoadItems();
                lvCartItems.Items.Refresh();
                txtPrice.Clear();
                txtQuantity.Clear();
                txtProdName.Clear();
            }
            else
            {
                MessageBox.Show("No records found", "Error");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvCartItems.ItemsSource != null && txtTotalPrice.Text != string.Empty)
            {
                CartItem item = lvCartItems.SelectedItem as CartItem;
                cartItems.Remove(item);
                LoadItems();
                lvCartItems.Items.Refresh();
                txtPrice.Clear();
                txtQuantity.Clear();
                txtProdName.Clear();
            }
            else
            {
                MessageBox.Show("No records found", "Error");
            }
        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int totalPrice = 0;
                CartItem item = lvCartItems.SelectedItem as CartItem;
                if (Convert.ToInt32(txtQuantity.Text) >  item.Product.UnitsInStock)
                {
                    MessageBox.Show("The amount specified is greater than the amount left in stock", "Error");
                    txtQuantity.Text = item.Product.UnitsInStock.ToString();
                }
                if (Convert.ToInt32(txtQuantity.Text) <= 0)
                {
                    MessageBox.Show("The amount specified is invalid", "Error");
                    txtQuantity.Text = "1";
                }
                foreach (CartItem cartItem in cartItems)
                {
                    totalPrice += cartItem.Product.Price * cartItem.Quantity;
                }
                txtTotalPrice.Text = totalPrice.ToString();

            }
            catch (Exception ex)
            {

            }
        }

        private void lvCartItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CartItem cartItem = lvCartItems.SelectedItem as CartItem;
                txtProdName.Text = cartItem.Product.Name;
                txtPrice.Text = cartItem.Product.Price.ToString();
                txtQuantity.Text = cartItem.Quantity.ToString();
            }
            catch (Exception ex)
            {
                txtTotalPrice.Clear();
            }

        }
    }
}
