﻿using System;
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
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        public AdminMenu()
        {
            InitializeComponent();
        }

        private void btnAdminCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateAdmin cad = new CreateAdmin();
            cad.Show();
        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            Stock stock = new Stock();
            Application.Current.MainWindow.Content = stock.Content;
            Application.Current.MainWindow.Title = "Manage Stock";
        }

        private void btnInvoice_Click(object sender, RoutedEventArgs e)
        {
            Invoices invoices = new Invoices();
            Application.Current.MainWindow.Content = invoices.Content;
            Application.Current.MainWindow.Title = "View Invoices";
        }


        private void btnAdminLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Application.Current.MainWindow.Content = mw.Content;
            Application.Current.MainWindow.Title = "Welcome to Shopaholik";
        }

        private void btnIncome_Click(object sender, RoutedEventArgs e)
        {
            Income inc = new Income();
            Application.Current.MainWindow.Content = inc.Content;
            Application.Current.MainWindow.Title = "Income Chart";
        }
    }
}
