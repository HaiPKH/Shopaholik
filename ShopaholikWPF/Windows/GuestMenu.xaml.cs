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
    /// Interaction logic for GuestMenu.xaml
    /// </summary>
    public partial class GuestMenu : Window
    {
        public GuestMenu()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Store store = new Store();
            Application.Current.MainWindow.Content = store.Content;
            Application.Current.MainWindow.Title = "Browse Store";
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            Cart cart = new Cart();
            Application.Current.MainWindow.Content = cart.Content;
            Application.Current.MainWindow.Title = "Your Cart";
        }

        private void btnHist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMessage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGuestLogout_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
