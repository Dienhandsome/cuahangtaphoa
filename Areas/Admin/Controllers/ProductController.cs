using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CUAHANG_TAPHOA.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private string filePath;

		public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Product.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Category = new SelectList(_dataContext.Category, "Id", "Name");
			ViewBag.Brand = new SelectList(_dataContext.Brand, "Id", "Name");
			return View(new ProductModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Category = new SelectList(_dataContext.Category, "Id", "Name", product.CategoryId);
			ViewBag.Brand = new SelectList(_dataContext.Brand, "Id", "Name", product.BrandId);

			if (ModelState.IsValid)
			{
				// thêm data
				product.Slug = product.Name.Replace(" ", "-");
				var slug = await _dataContext.Product.FirstOrDefaultAsync(p => p.Slug == product.Slug);
				if (slug != null)
				{
					ModelState.AddModelError(" ", "Sản phẩm đã có trong database!!!");
					return View(product);
				}

				if (product.ImageUpload != null)
				{
					string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName; // tạo chuôi xhinhf ảnh random (tên)
					string filePath = Path.Combine(uploadsDir, imageName);

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close(); // Sau khi tạo xong thì đóng lại
					product.Image = imageName; // Để có thể upload vào đưuọc cột Image trong csdl nếu không có dòng này thì sẽ không hiện ảnh được
				}

				_dataContext.Add(product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Sản phẩm thêm thành công";
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
			return View(product);
		}
        public async Task<IActionResult> Edit(int Id)
        {
            var product = await _dataContext.Product.FindAsync(Id);
            if (product == null)
            {
                // Handle the case where the product is not found
                return NotFound();
            }

            ViewBag.Category = new SelectList(_dataContext.Category, "Id", "Name", product.CategoryId);
            ViewBag.Brand = new SelectList(_dataContext.Brand, "Id", "Name", product.BrandId);

            return View(product);
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductModel product)
		{
			ViewBag.Category = new SelectList(_dataContext.Category, "Id", "Name", product.CategoryId);
			ViewBag.Brand = new SelectList(_dataContext.Brand, "Id", "Name", product.BrandId);

			var existed_product = _dataContext.Product.Find(product.Id);

			if (ModelState.IsValid)
			{
				// thêm data
				product.Slug = product.Name.Replace(" ", "-");
				var slug = await _dataContext.Product.FirstOrDefaultAsync(p => p.Slug == product.Slug);
				if (slug != null)
				{
					ModelState.AddModelError(" ", "Sản phẩm đã có trong database!!!");
					return View(product);
				}

				if (product.ImageUpload != null)
				{
					string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName; // tạo chuôi xhinhf ảnh random (tên)
					string filePath = Path.Combine(uploadsDir, imageName);  // xử lý hình ảnh cũ 

					//Delete old image
					string oldfilePath = Path.Combine(uploadsDir, existed_product.Image);
					try
					{
						if (System.IO.File.Exists(oldfilePath))
						{
							System.IO.File.Delete(oldfilePath);
						}
					}
					catch (Exception ex)
					{
						ModelState.AddModelError("", "An error ............ image");
					}

					FileStream fs = new FileStream(filePath, FileMode.Create); // Khai báo hình ảnh mới khi chúng ta update 
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close(); // Sau khi tạo xong thì đóng lại
					existed_product.Image = imageName; // Để có thể upload vào đưuọc cột Image trong csdl nếu không có dòng này thì sẽ không hiện ảnh được


					
				}

				// Update other product properties
				existed_product.Name = product.Name;
				existed_product.Description = product.Description;
				existed_product.Price = product.Price;
				existed_product.BrandId = product.BrandId;
				existed_product.CategoryId = product.CategoryId;

				_dataContext.Update(existed_product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Cập nhật thêm thành công";
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
			return View(product);
		}

		public async Task<IActionResult> Delete(int Id)
		{
			ProductModel product = await _dataContext.Product.FindAsync(Id);
			if (product == null)
			{
				return NotFound();
			}
			string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
			string oldfilePath = Path.Combine(uploadsDir, product.Image);
			try
			{
				if (System.IO.File.Exists(oldfilePath))
				{
					System.IO.File.Delete(oldfilePath);
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "An error ............ image");
			}

			_dataContext.Product.Remove(product);
			await _dataContext.SaveChangesAsync();
			TempData["error"] = " Sản phẩm đã xóa";
			return RedirectToAction("Index");
		}




		//    [HttpPost]
		//    public async Task<IActionResult> Create(ProductModel product)
		//    {
		//        if (ModelState.IsValid)
		//        {
		//            // Xử lý upload hình ảnh
		//            if (product.ImageUpload != null)
		//            {
		//                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath,"images/");
		//                string fileName = Path.GetFileNameWithoutExtension(product.ImageUpload.FileName);
		//                string extension = Path.GetExtension(product.ImageUpload.FileName);
		//                product.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
		//                string path = Path.Combine(uploadDir, fileName);

		//                using (var fileStream = new FileStream(path, FileMode.Create))
		//                {
		//                    await product.ImageUpload.CopyToAsync(fileStream);
		//                }
		//            }
		//            else
		//            {
		//                // Gán giá trị mặc định hoặc xử lý trường hợp không có hình ảnh được upload
		//                product.Image = "default.png";
		//            }

		//            // Tạo slug nếu chưa có
		//            if (string.IsNullOrEmpty(product.Slug))
		//            {
		//                product.Slug = GenerateSlug(product.Name);
		//            }

		//            _dataContext.Add(product);
		//            await _dataContext.SaveChangesAsync(); // Dòng này đã được sửa đúng
		//            TempData["success"] = "ok lưu thành công";
		//            return RedirectToAction(nameof(Index));
		//        }

		//        ViewBag.Category = new SelectList(_dataContext.Category, "Id", "Name", product.CategoryId);
		//        ViewBag.Brand = new SelectList(_dataContext.Brand, "Id", "Name", product.BrandId);
		//        TempData["error"] = "Có một vài chỗ bị lỗi";
		//        return View(product);
		//    }

		//    // Phương thức tạo slug từ tên sản phẩm
		//    private string GenerateSlug(string name)
		//    {
		//        string slug = name.ToLower();
		//        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", ""); // Loại bỏ các ký tự không hợp lệ
		//        slug = Regex.Replace(slug, @"\s+", " ").Trim(); // Thay thế nhiều khoảng trắng bằng một khoảng trắng
		//        slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim(); // Giới hạn độ dài của slug
		//        slug = Regex.Replace(slug, @"\s", "-"); // Thay thế khoảng trắng bằng dấu gạch ngang
		//        return slug;
		//    }
	}
}
