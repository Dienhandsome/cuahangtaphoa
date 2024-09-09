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
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _dataContext.Order.OrderByDescending(p => p.Id).ToListAsync());
        //}

        public async Task<IActionResult> Index(int pg = 1) // khi phân trang mới sd như này
        {
            List<OrderModel> orders = _dataContext.Order.ToList();

            const int pageSize = 5; // 5 ITEMSS TRÊN TRANG
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = orders.Count();
            var pager = new Paginate(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = orders.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            return View(data);
            //return View(await _dataContext.Category.OrderByDescending(p => p.Id).ToListAsync());
        }

        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var detailsOrder = await _dataContext.orderDetail.Include(od => od.Product).Where(od =>od.OrderCode == ordercode).ToListAsync();
            return View(detailsOrder);
        }
    }
}
