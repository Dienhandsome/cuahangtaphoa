using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Gocery.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace CUAHANG_TAPHOA.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		private int currentMaxStatus;

		public CheckoutController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel(); // tạo đơn hàng mới

				// Lấy giá trị Status hiện tại cao nhất, nếu không có thì gán giá trị 1
				var currentMaxStatus = _dataContext.Order.Any() ? _dataContext.Order.Max(o => o.Status) : 1;


				orderItem.OrderCode = ordercode;
				orderItem.UserName = userEmail;
				//orderItem.Status = +1;
				orderItem.Status = currentMaxStatus + 1; // Tăng giá trị Status
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();

				List<CartModel> cartItems = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();
				foreach (var item in cartItems)
				{
					var orderdetails = new OrderDetail();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId = item.ProductId;
					orderdetails.Price = item.Price;
					orderdetails.Quantity = item.Quantity;
					_dataContext.Add(orderdetails); _dataContext.SaveChanges();


				}
				HttpContext.Session.Remove("Cart"); // Sau khi checkout thì xóa bỏ sản phẩm và thông báo thành công , quay về index cart
				TempData["success"] = "Checkout thành công";
				return RedirectToAction("Index", "Cart");
			}
			return View();
		}


		public IActionResult Index()
		{
			return View();
		}
	}
}
