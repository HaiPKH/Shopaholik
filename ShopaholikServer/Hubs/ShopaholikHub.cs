using Microsoft.AspNetCore.SignalR;
using System.Windows;
using ShopaholikWPF.Model;
using Microsoft.EntityFrameworkCore;
namespace ShopaholikServer.Hubs
{
    public class ShopaholikHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            MessageBox.Show("Connected", "Wutdehell");
            return Task.CompletedTask;
        }

        public async Task PurchaseItems(List<CartItem> cart)
        {
            try
            {
                ShopaholikContext context = new ShopaholikContext();
                foreach (CartItem item in cart)
                {
                    Product product = context.Products.FirstOrDefault(p => p.Name == item.Product.Name);
                    if (product != null)
                    {
                        if (product.UnitsInStock - item.Quantity <= 0)
                        {
                            context.Products.Remove(product);
                            context.SaveChanges();
                            await Clients.All.SendAsync("itemupdate", cart);
                        }
                        else
                        {
                            product.UnitsInStock = product.UnitsInStock - item.Quantity;
                            context.Entry<Product>(product).State = EntityState.Modified;
                            context.SaveChanges();
                            await Clients.All.SendAsync("itemupdate", cart);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task AddProduct(Product product)
        {
            try
            {
                ShopaholikContext context = new ShopaholikContext();
                context.Products.Add(product);
                context.SaveChanges();
                await Clients.All.SendAsync("addproduct", product);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task UpdateProduct(Product product)
        {
            try
            {
                ShopaholikContext context = new ShopaholikContext();
                context.Entry<Product>(product).State = EntityState.Modified;
                context.SaveChanges();
                await Clients.All.SendAsync("updateproduct", product);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task DeleteProduct(Product product)
        {
            try
            {
                ShopaholikContext context = new ShopaholikContext();
                context.Products.Remove(product);
                context.SaveChanges();
                await Clients.All.SendAsync("deleteproduct", product);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
