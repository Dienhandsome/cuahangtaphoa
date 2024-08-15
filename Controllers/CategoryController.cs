using CUAHANG_TAPHOA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using CUAHANG_TAPHOA.Reponsitory;

namespace CUAHANG_TAPHOA.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;

		public CategoryController(DataContext Context)
		{
			_dataContext = Context;
		}

		public async Task<IActionResult> Index(string Slug = "")
		{
			var category = _dataContext.Category.FirstOrDefault(c => c.Slug == Slug);
			if (category == null)
			{
				ViewBag.ErrorMessage = "Không tìm thấy danh mục.";
				return View(new List<ProductModel>());
			}

			var productsByCategory = await _dataContext.Product
														.Where(p => p.CategoryId == category.Id)
														.OrderByDescending(p => p.Id)
														.ToListAsync();

			if (!productsByCategory.Any())
			{
				ViewBag.ErrorMessage = "Không có sản phẩm nào trong danh mục này.";
			}

			return View(productsByCategory);
		}

	}
}
