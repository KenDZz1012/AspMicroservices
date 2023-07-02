using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfigureOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database ");
            }
        }

        public static IEnumerable<Order> GetPreconfigureOrders()
        {
            return new List<Order>
            {
                new Order() {UserName= "Ken",FirstName="Ng",LastName="ken",EmailAddress="kienteo1012@gmail.com",AddressLine="Ha noi",Country="Viet Nam",TotalPrice=200}
            };
        }
    }
}
