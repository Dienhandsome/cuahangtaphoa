using CUAHANG_TAPHOA.Models;
using Microsoft.EntityFrameworkCore;

namespace CUAHANG_TAPHOA.Reponsitory
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<BrandModel> Brand { get; set; }
        public DbSet<ProductModel> Product { get; set; }
    }
}
