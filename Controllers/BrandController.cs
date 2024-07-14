using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CUAHANG_TAPHOA.Controllers
{

	public class BrandController: Controller
	{
		private readonly DataContext _dataContext;

		public BrandController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			// Retrieve the category based on the Slug
			var brand = _dataContext.Brand.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brand == null)
			{
				return RedirectToAction("Index");
			}

			// Retrieve the products in the specified category and order them by Id in descending order
			var productsByBrand = await _dataContext.Product
														.Where(p => p.BrandId == brand.Id)
														.OrderByDescending(p => p.Id)
														.ToListAsync();

			return View(productsByBrand);
		}

	}
}
