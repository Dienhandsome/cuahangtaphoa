using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



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
			// Retrieve the category based on the Slug
			var category = _dataContext.Category.Where(c => c.Slug == Slug).FirstOrDefault();
			if (category == null)
			{
				return RedirectToAction("Index");
			}

			// Retrieve the products in the specified category and order them by Id in descending order
			var productsByCategory = await _dataContext.Product
														.Where(p => p.CategoryId == category.Id)
														.OrderByDescending(p => p.Id)
														.ToListAsync();

			return View(productsByCategory);
		}

	}
}
