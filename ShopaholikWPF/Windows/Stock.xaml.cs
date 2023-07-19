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
using Microsoft.AspNetCore.SignalR.Client;

namespace ShopaholikWPF.Windows
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class Stock : Window
    {
        HubConnection connection;
        public Stock()
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

        private Product GetProduct()
        {
            Product product = null;
            try
            {
                product = new Product()
                {
                    Name = txtProdName.Text,
                    UnitsInStock = int.Parse(txtUnitsInStock.Text),
                    Price = int.Parse(txtPrice.Text),
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Prod");
            }
            return product;
        }

        public Product GetProductByName(String name)
        {
            Product product = null;
            try
            {
                ShopaholikContext context = new ShopaholikContext();
                product = context.Products.SingleOrDefault(product => product.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = GetProduct();
                if (GetProductByName(product.Name) != null)
                {
                    throw new Exception("Product already existed.");
                }
                if (product.Price <= 0)
                {
                    throw new Exception("Price cannot be zero or negative.");
                }
                if (product.UnitsInStock <= 0)
                {
                    throw new Exception("Quantity cannot be zero or negative.");
                }
                else
                {
                    this.connection.InvokeAsync("AddProduct", product);
                    txtProdName.Clear();
                    txtUnitsInStock.Clear();
                    txtPrice.Clear();
                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Product product = lvProducts.SelectedItem as Product;
            this.connection.InvokeAsync("DeleteProduct", product);
            txtProdName.Clear();
            txtUnitsInStock.Clear();
            txtPrice.Clear();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = lvProducts.SelectedItem as Product;
                if (txtProdName.Text != product.Name && GetProductByName(txtProdName.Text) != null)
                {
                    MessageBox.Show("Product name already existed.", "Warning");
                    return;
                }
                product.Name = txtProdName.Text;
                if (Convert.ToInt32(txtUnitsInStock.Text.ToString()) <= 0)
                {
                    MessageBox.Show("Quantity cannot be zero or negative", "Warning");
                    return;
                }
                product.UnitsInStock = Convert.ToInt32(txtUnitsInStock.Text.ToString());
                if (Convert.ToInt32(txtPrice.Text.ToString()) <= 0)
                {
                    MessageBox.Show("Price cannot be zero or negative", "Warning");
                    return;
                }
                product.Price = Convert.ToInt32(txtPrice.Text.ToString());
                this.connection.InvokeAsync("UpdateProduct", product);
                LoadProducts();
                txtProdName.Clear();
                txtUnitsInStock.Clear();
                txtPrice.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "An error occurred");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMenu adm = new AdminMenu();
            Application.Current.MainWindow.Content = adm.Content;
            Application.Current.MainWindow.Title = "Admin Menu";
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product product = lvProducts.SelectedItem as Product;
            if (product != null)
            {
                txtProdName.Text = product.Name;
                txtUnitsInStock.Text = product.UnitsInStock.ToString();
                txtPrice.Text = product.Price.ToString();
            }

        }
    }
}
