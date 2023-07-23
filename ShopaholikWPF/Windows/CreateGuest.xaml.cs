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
using Microsoft.EntityFrameworkCore;
using ShopaholikWPF.Model;

namespace ShopaholikWPF.Windows
{
    /// <summary>
    /// Interaction logic for CreateGuest.xaml
    /// </summary>
    public partial class CreateGuest : Window
    {
        public CreateGuest()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            ShopaholikContext context = new ShopaholikContext();
            Account account = new Account();
            account.Username = txtGuestName.Text;
            account.Password = pwdGuest.Password;
            account.Role = "guest";
            if (context.Accounts.Where(a => a.Username == account.Username).Any())
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "Account already existed, would you like to update the password of the existing account?", "Warning",
                    MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    context.Entry<Account>(account).State = EntityState.Modified;
                    context.SaveChanges();
                    MessageBox.Show("Successfully updated", "Success");
                }
                else
                {
                    return;
                }
            }
            else
            {
                context.Accounts.Add(account);
                context.SaveChanges();
                MessageBox.Show("Successfully added", "Success");
            }

            Close();
        }
    }
}
