using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Collections.Generic;


namespace CUAHANG_TAPHOA.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly DataContext _dataContext;

		public HomeController(ILogger<HomeController> logger, DataContext context)
		{
			_logger = logger;
            _dataContext = context;
		}

		public IActionResult Index()
		{
			var products = _dataContext.Product.Include("Category").Include("Brand").ToList();
			// mu?n hi?n th? danh m?c nào thì thêm cÁI ?ó vaof VD: Include("Category").Include("Brand")
			return View(products);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
