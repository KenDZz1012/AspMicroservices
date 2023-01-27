using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        public readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            //var query = ;
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("Select * from Coupon Where ProductName = @ProductName", new {ProductName = productName});
            if(coupon == null)
            {
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }
            return coupon;

        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var query = "Insert into Coupon(ProductName,Description,Amount) values (@ProductName,@Description,@Amount)";
            var affected = await connection.ExecuteAsync(query,new {ProductName = coupon.ProductName, Description = coupon.Description,Amount = coupon.Amount});
            if (affected == 0) return false;
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var query = "Delete from Coupon Where ProductName = @ProductName";
            var affected = await connection.ExecuteAsync(query, new { ProductName = productName });
            if (affected == 0) return false;
            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var query = "Update Coupon Set ProductName=@ProductName,Description=@Description,Amount=@Amount";
            var affected = await connection.ExecuteAsync(query, new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            if (affected == 0) return false;
            return true;
        }
    }
}
