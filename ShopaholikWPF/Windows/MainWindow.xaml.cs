using ShopaholikWPF.Model;
using ShopaholikWPF.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopaholikWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShopaholikContext context = new ShopaholikContext();
            if(!context.Accounts.Where(a => a.Role == "admin").ToList().Any())
            {
                btnAdminCreate.Visibility = Visibility.Visible;
                btnAdminLogin.Visibility = Visibility.Hidden;
            }
        }

        private void AdminLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Account accountInput = new Account();
            accountInput.Username = txtUsername.Text;
            accountInput.Password = txtPassword.Password;
            accountInput.Role = "admin";
            var AccountDb = new ShopaholikContext();
            bool userFound = false;
            foreach (var account in AccountDb.Accounts.ToList())
            {
                if (accountInput.Username.Equals(account.Username) && accountInput.Password.Equals(account.Password) && accountInput.Role.Equals(account.Role.Trim()))
                {
                    AdminMenu adm = new AdminMenu();
                    Application.Current.MainWindow.Content = adm.Content;
                    Application.Current.MainWindow.Title = "Admin Menu";
                    Application.Current.Properties["Username"] = txtUsername.Text;
                    userFound = true;
                }
            }
            if (!userFound)
            {
                MessageBox.Show("Login failed!", "Warning");
            }

        }

        private void GuestLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Account accountInput = new Account();
            accountInput.Username = txtUsername.Text;
            accountInput.Password = txtPassword.Password;
            accountInput.Role = "guest";
            var AccountDb = new ShopaholikContext();
            bool userFound = false;
            foreach (var account in AccountDb.Accounts.ToList())
            {
                if (accountInput.Username.Equals(account.Username) && accountInput.Password.Equals(account.Password) && accountInput.Role.Equals(account.Role.Trim()))
                {
                    GuestMenu gm = new GuestMenu();
                    Application.Current.MainWindow.Content = gm.Content;
                    Application.Current.MainWindow.Title = "Menu";
                    Application.Current.Properties["Username"] = txtUsername.Text;
                    userFound = true;
                }
            }
            if (!userFound)
            {
                MessageBox.Show("Login failed!", "Warning");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnAdminCreate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGuestCreate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
