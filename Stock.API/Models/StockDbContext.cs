using Microsoft.EntityFrameworkCore;

namespace Stock.API.Models
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
