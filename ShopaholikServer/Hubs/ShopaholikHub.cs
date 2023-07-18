using Microsoft.AspNetCore.SignalR;
using System.Windows;
using ShopaholikWPF.Model;
namespace ShopaholikServer.Hubs
{
    public class ShopaholikHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            MessageBox.Show("Connected", "Wutdehell");
            return Task.CompletedTask;
        }
    }
}
