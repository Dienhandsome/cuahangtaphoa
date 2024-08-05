using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace CUAHANG_TAPHOA.Areas.Admin.Controllers
{
	[Area("Admin")]

	[Authorize(Roles = "Admin,publisher")]// khi đăng nhập thì mới cho vào category
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;

		}

		[Route("Index")]
		public async Task<IActionResult> Index(int pg = 1) // khi phân trang mới sd như này
		{
			List<CategoryModel> category = _dataContext.Category.ToList();

			const int pageSize = 5;
			if (pg < 1)
			{
				pg = 1;
			}
			int recsCount = category.Count();
			var pager = new Paginate(recsCount, pg, pageSize);
			int recSkip = (pg - 1) * pageSize;
			var data = category.Skip(recSkip).Take(pager.PageSize).ToList();
			ViewBag.Pager = pager;
			return View(data);
			//return View(await _dataContext.Category.OrderByDescending(p => p.Id).ToListAsync());
		}
		[HttpGet]
		public IActionResult Create()
		{

			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryModel category)
		{

			if (ModelState.IsValid)
			{
				// thêm data
				category.Slug = category.Name.Replace(" ", "-");
				var slug = await _dataContext.Category.FirstOrDefaultAsync(p => p.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Danh mục đã có trong database!!!");
					return View(category);
				}


				_dataContext.Add(category);
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
			return View(category);
		}

		public async Task<IActionResult> Delete(int Id)
		{
			var category = await _dataContext.Category.FindAsync(Id);
			if (category == null)
			{
				// Handle the case where the category is not found
				TempData["error"] = "Danh mục không tìm thấy";
				return RedirectToAction("Index");
			}

			try
			{
				_dataContext.Category.Remove(category);
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

		public async Task<IActionResult> Edit(int Id)
		{
			CategoryModel category = await _dataContext.Category.FindAsync(Id);
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CategoryModel category)
		{

			if (ModelState.IsValid)
			{
				// thêm data
				category.Slug = category.Name.Replace(" ", "-");
				var slug = await _dataContext.Category.FirstOrDefaultAsync(p => p.Slug == category.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Danh mục đã có trong database!!!");
					return View(category);
				}


				_dataContext.Update(category);
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
			return View(category);
		}
	}
}