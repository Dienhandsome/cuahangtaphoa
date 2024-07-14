using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;

using Microsoft.EntityFrameworkCore;

namespace Gocery.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Product.Any())
            {
                CategoryModel macbook = new CategoryModel { Name = "Apple", Slug = "apple", Description = "Apple is Large Brand in the world", Status = 1 };
                CategoryModel pc = new CategoryModel { Name = "Samsung", Slug = "samsung", Description = "samsung is Large Brand in the world", Status = 1 };
                BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is Large Brand in the world", Status = 1 };
                BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "samsung", Description = "samsung is Large Brand in the world", Status = 1 };

                _context.Product.AddRange(
                    new ProductModel { Name = "MacBook", Slug = "macbook", Description = "Mac is the best", Image = "1.png", Category = macbook, Brand = apple, Price = 1300 },
                    new ProductModel { Name = "Pc", Slug = "pc", Description = "pc is the best", Image = "2.png", Category = pc, Brand = samsung, Price = 1300 }
                );
                _context.SaveChanges();
            }
        }

		
	}
}
