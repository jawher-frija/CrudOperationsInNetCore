using Microsoft.EntityFrameworkCore;

namespace CrudOperationsInNetCore.Models
{
    public class BrandDbContext : DbContext
    {
        public BrandDbContext(DbContextOptions<BrandDbContext> options) :base(options)
        {
            
        }
       public DbSet<Brand> Brands { get; set; }
    }
}
