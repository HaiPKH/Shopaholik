using Microsoft.AspNetCore.SignalR.Client;
using ShopaholikWPF.Model;
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

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
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
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCartItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CartItem cartItem = lvCartItems.SelectedItem as CartItem;
            txtProdName.Text = cartItem.Product.Name;
            txtPrice.Text = cartItem.Product.Price.ToString();
            txtQuantity.Text = cartItem.Quantity.ToString();
        }
    }
}
