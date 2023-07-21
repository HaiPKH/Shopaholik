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
using Microsoft.AspNetCore.SignalR.Client;
using ShopaholikWPF.Model;

namespace ShopaholikWPF.Windows
{
    /// <summary>
    /// Interaction logic for Store.xaml
    /// </summary>
    public partial class Store : Window
    {
        HubConnection connection;
        ShopaholikContext context = new ShopaholikContext();
        List<CartItem> cartItems = new List<CartItem>();
        public Store()
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
            connection.On<CartItem>("itemupdate", (cart)
                 =>
            {
                Dispatcher.BeginInvoke((Action)(() =>

                {
                    //MessageBox.Show("A message was received");
                    ShopaholikContext context = new ShopaholikContext();
                    lvProducts.ItemsSource = context.Products.ToList();
                }));
            });
            connection.On<Product>("addproduct", (product)
                 =>
            {
                Dispatcher.BeginInvoke((Action)(() =>

                {
                    //MessageBox.Show("A message was received");
                    ShopaholikContext context = new ShopaholikContext();
                    lvProducts.ItemsSource = context.Products.ToList();
                }));
            });
            connection.On<Product>("updateproduct", (product)
                 =>
            {
                Dispatcher.BeginInvoke((Action)(() =>

                {
                    //MessageBox.Show("A message was received");
                    ShopaholikContext context = new ShopaholikContext();
                    lvProducts.ItemsSource = context.Products.ToList();
                }));
            });
            connection.On<Product>("deleteproduct", (product)
                 =>
            {
                Dispatcher.BeginInvoke((Action)(() =>

                {
                    //MessageBox.Show("A message was received");
                    ShopaholikContext context = new ShopaholikContext();
                    lvProducts.ItemsSource = context.Products.ToList();
                }));
            });
            LoadProducts();
        }
        private void LoadProducts()
        {
            ShopaholikContext context = new ShopaholikContext();
            lvProducts.ItemsSource = context.Products.ToList();
        }
        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Product product = lvProducts.SelectedItem as Product;
                if (Convert.ToInt32(txtQuantity.Text) >  product.UnitsInStock)
                {
                    MessageBox.Show("The amount specified is greater than the amount left in stock", "Error");
                    txtQuantity.Text = product.UnitsInStock.ToString();
                }
                if (Convert.ToInt32(txtQuantity.Text) <= 0)
                {
                    MessageBox.Show("The amount specified is invalid", "Error");
                    txtQuantity.Text = "1";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CartItem cartItem = new CartItem();
                cartItem.Product = lvProducts.SelectedItem as Product;
                cartItem.Quantity = int.Parse(txtQuantity.Text);
                CartItem item = cartItems.FirstOrDefault(item => item.Product.Name == cartItem.Product.Name);
                if (item != null && item.Quantity < cartItem.Product.UnitsInStock)
                {
                    lbStatus.Foreground = new SolidColorBrush(Colors.Green);
                    item.Quantity += cartItem.Quantity;
                    lbStatus.Content = "Successfully updated to cart";
                }
                else if (item != null && item.Quantity == cartItem.Product.UnitsInStock)
                {
                    lbStatus.Foreground = new SolidColorBrush(Colors.Red);
                    lbStatus.Content = "Quantity exceeded max amount";
                }
                else
                {
                    lbStatus.Foreground = new SolidColorBrush(Colors.Green);
                    cartItems.Add(cartItem);
                    lbStatus.Content = "Successfully added to cart";

                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("The amount specified is invalid", "Error");
                txtQuantity.Text = "1";
                return;
            }
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product product = lvProducts.SelectedItem as Product;
            if (product != null)
            {
                txtProdName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                txtQuantity.Text = "1";
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Save all items to cart and exit?",
                    "Confirm exit",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //string cartItemsString = JsonSerializer.Serialize(cartItems);
                Application.Current.Properties["CartItems"] = cartItems;
                GuestMenu gm = new GuestMenu();
                Application.Current.MainWindow.Content = gm.Content;
                Application.Current.MainWindow.Title = "Menu";
                string allItems = string.Empty;
                foreach (CartItem item in Application.Current.Properties["CartItems"] as List<CartItem>)
                {
                    if (allItems.Length > 0)
                    {
                        allItems += "\n";
                    }
                    else { } //TODO Nothing

                    allItems += item.Product.Name;
                    allItems += "\t";
                    allItems += item.Product.Price;
                    allItems += "\t";
                    allItems += item.Quantity.ToString();
                }
                MessageBox.Show($"My Items:\n{allItems}", "Items");
            }
        }
    }
}
