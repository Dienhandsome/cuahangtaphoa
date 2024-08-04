using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CUAHANG_TAPHOA.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize]// khi đăng nhập thì mới cho vào order
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Order.OrderByDescending(p => p.Id).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var detailsOrder = await _dataContext.orderDetail.Include(od => od.Product).Where(od =>od.OrderCode == ordercode).ToListAsync();
            return View(detailsOrder);
        }
    }
}
