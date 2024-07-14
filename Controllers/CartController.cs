using Azure.Core;
using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Models.ViewModel;
using CUAHANG_TAPHOA.Reponsitory;
using Gocery.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CUAHANG_TAPHOA.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		public CartController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index()
		{
			List<CartModel> cartItems = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();
			//                                               GetJson đã đăng ký ;             Nếu chưa có thì tạo mới 1 CartModel                       
			CartItemViewModel cartVM = new()
			{
				CartItems = cartItems,
				// CartItems NẰM BÊN KHAI BÁO CartItemViewModel còn cartItems gán cho nó ở phiaas trên 
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}

		// Thực hiện chức năng thêm vào giỏ hàng
		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await _dataContext.Product.FindAsync(Id);
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();
			CartModel cartItem = cart.FirstOrDefault(c => c.ProductId == Id);
			if (cartItem == null)
			{
				cartItem = new CartModel(product) { Quantity = 1 }; // Nếu chưa có trong giỏ hàng, tạo mới và đặt Quantity là 1
				cart.Add(cartItem);    // có 2 cách nếu không thêm Quantity ở đây thì thêm thẳng ngay lúc đầu ở ModelCART
			}
			else
			{
				cartItem.Quantity += 1; // Nếu đã có trong giỏ hàng, tăng Quantity lên 1
			}

			HttpContext.Session.SetJson("Cart", cart); // Lưu lại giỏ hàng vào session

            // Thông báo notification nhưng action có hoàn thành chưa
            TempData["success"] = "Add product item successfully";

            return Redirect(Request.Headers["Referer"].ToString());
		}

		public  async Task<IActionResult> Decrease(int Id)
		{
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart");
			
			CartModel cartItem = cart.Where(c=> c.ProductId == Id).FirstOrDefault();
			
			if(cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if(cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");

			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

            // Thông báo notification nhưng action có hoàn thành chưa
            TempData["success"] = "Decrease product Quantity in ProductCart Successfully!!!";
            return RedirectToAction("Index");
		}

		public async Task<IActionResult> Increase(int Id)
		{
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart");

			CartModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

		
			if (cartItem.Quantity >= 1)
			{
				++cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");

			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
            // Thông báo notification nhưng action có hoàn thành chưa
            TempData["success"] = "Increase product Quantity in ProductCart Successfully!!!";
            return RedirectToAction("Index");
		}

		public async Task<IActionResult> Remove(int Id)
		{
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart");

			cart.RemoveAll(p => p.ProductId == Id);

			HttpContext.Session.SetJson("Cart", cart); // Luôn cập nhật session ngay cả khi giỏ hàng có sản phẩm còn lại

            TempData["success"] = "Remove product in ProductCart Successfully!!!";
            return RedirectToAction("Index");
		}


		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");

            TempData["success"] = "Clear product in ProductCart Successfully!!!";
            return RedirectToAction("Index");
		}

    }
}
