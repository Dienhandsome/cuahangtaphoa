using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Reponsitory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CUAHANG_TAPHOA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize()]
    public class UserController : Controller
    {


        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        public UserController(DataContext context, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _dataContext = context;


        }
        [HttpGet]

        public async Task<IActionResult> Index()
        {  // lấy từ csdl 
            var userWithRoles = await (from u in _dataContext.Users
                                       join ur in _dataContext.UserRoles on u.Id equals ur.UserId
                                       join r in _dataContext.Roles on ur.RoleId equals r.Id
                                       select new {User = u, RoleName = r.Name }).ToListAsync();

            return View(userWithRoles); // lấy 3 bằng trong sql NetUserRole sẽ đối chiếu với 2 bảng NetRole và NetUser để hiển thị được cái in rolle trong index
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách vai trò từ cơ sở dữ liệu
            var roles = await _roleManager.Roles.ToListAsync();

            // Tạo SelectList từ danh sách vai trò và gán vào ViewBag.Roles
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            // Trả về view với model rỗng để hiển thị form tạo người dùng
            return View(new AppUserModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUserModel user, string roleId)
        {
            if (ModelState.IsValid)
            {
                var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash); // tạo user
                if (createUserResult.Succeeded)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);
                    if (role != null)
                    {
                        var addToRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
                        if (addToRoleResult.Succeeded)
                        {
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            AddIdentityErrors(addToRoleResult);
                        }
                    }
                }
                else
                {
                    AddIdentityErrors(createUserResult);
                }
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    return View("Error");
                }
                TempData["success"] = "Xóa User thành công";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var roles = await _roleManager.Roles.ToListAsync();
                ViewBag.Roles = new SelectList(roles, "Id", "Name");

                return View(user);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id, AppUserModel updatedUser, string roleId)
            // update được cả password vì lấy thêm roleId của user để sửa pass 
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                
               
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.PasswordHash = _userManager.PasswordHasher.HashPassword(existingUser, updatedUser.PasswordHash);

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    // Cập nhật vai trò
                    var currentRoles = await _userManager.GetRolesAsync(existingUser);
                    await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);

                    var role = await _roleManager.FindByIdAsync(roleId);
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(existingUser, role.Name);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    AddIdentityErrors(result);
                }
            }

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            TempData["error"] = "Model validation failed";
            var errors = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)).ToList();
            string errorMessage = string.Join("/n", errors);
            return View(updatedUser);
        }

    }
}
