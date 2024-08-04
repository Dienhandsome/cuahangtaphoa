using CUAHANG_TAPHOA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CUAHANG_TAPHOA.Reponsitory
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<BrandModel> Brand { get; set; }
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<OrderModel> Order { get; set; }
        public  DbSet<OrderDetail> orderDetail { get; set; }
		
	}
}
