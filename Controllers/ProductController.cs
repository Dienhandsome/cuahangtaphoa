using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CUAHANG_TAPHOA.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		public ProductController(DataContext context)
		{
		  _dataContext = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Detail(int? Id)
		{
			if(Id == null) return RedirectToAction("Index");

			var productById  = _dataContext.Product.Where(p => p.Id == Id).FirstOrDefault();
			
			return View(productById);
		}
	}
}
