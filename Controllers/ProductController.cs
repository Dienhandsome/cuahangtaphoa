using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
            var products = _dataContext.Product.Include("Category").Include("Brand").ToList();
            // mu?n hi?n th? danh m?c nào thì thêm cÁI ?ó vaof VD: Include("Category").Include("Brand")
            return View(products);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = await _dataContext.Product
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();  
            ViewBag.Keyword = searchTerm;

            return View(products);
        }
        public async Task<IActionResult> Detail(int? id)
		{
			if(id == null) return RedirectToAction("Index");

			var productById = await _dataContext.Product
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productById == null)
            {
                return NotFound();
            }

            return View(productById);
		}
	}
}
