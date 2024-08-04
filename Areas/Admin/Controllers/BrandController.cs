using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CUAHANG_TAPHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize] // khi đăng nhập thì mới cho vào brand
	public class BrandController: Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;

        }
   
        public async Task<IActionResult> Index()
        {

            return View(await _dataContext.Brand.OrderByDescending(p => p.Id).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {

            if (ModelState.IsValid)
            {
                // thêm data
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Brand.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database!!!");
                    return View(brand);
                }


                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error thêm không thành công";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);

            }
            return View(brand);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _dataContext.Brand.FindAsync(Id);
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {

            if (ModelState.IsValid)
            {
                // thêm data
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Brand.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database!!!");
                    return View(brand);
                }


                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error thêm không thành công";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);

            }
            return View(brand);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var brand = await _dataContext.Brand.FindAsync(Id);
            if (brand == null)
            {
                // Handle the case where the category is not found
                TempData["error"] = "Danh mục không tìm thấy";
                return RedirectToAction("Index");
            }

            try
            {
                _dataContext.Brand.Remove(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Danh mục đã được xóa thành công";
            }
            catch (Exception ex)
            {
                // Handle exceptions if there are any issues with the delete operation
                TempData["error"] = "Có lỗi xảy ra khi xóa danh mục: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
