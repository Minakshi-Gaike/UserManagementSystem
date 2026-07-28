using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class AdminController : Controller
    {
        UserDbContext db;
        
        public AdminController(UserDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            string admin = HttpContext.Session.GetString("Admin");

            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpGet]
        // Admin Dashboard

        public IActionResult AdminLogin()
        {
            return View();

        }


        // POST: Admin Login
        [HttpPost]
        public IActionResult AdminLogin(AdminDTO adminDto)
        {
            if (!ModelState.IsValid)
            {
                return View(adminDto);
            }

            TblAdmin admin = db.TblAdmins
                .FirstOrDefault(e =>
                    e.UserName == adminDto.UserName &&
                    e.Password == adminDto.Password);

            if (admin != null)
            {
                HttpContext.Session.SetInt32(
                    "AdminId",
                    admin.AdminId);

                HttpContext.Session.SetString(
                    "Admin",
                    admin.UserName);

                return RedirectToAction("UserList");
            }

            ViewBag.errormsg = "Invalid Admin Username or Password";

            return View(adminDto);
        }

        // Display all users
        public IActionResult UserList()
        {
            string admin = HttpContext.Session.GetString("Admin");

            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("AdminLogin");
            }

            List<TblUser> users = db.TblUsers
                .OrderByDescending(e => e.UserId)
                .ToList();

            return View(users);
        }


        // Activate User
        public async Task<IActionResult> ActivateUser(int id)
        {
            string admin = HttpContext.Session.GetString("Admin");

            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("AdminLogin");
            }

            TblUser user = await db.TblUsers
                .FirstOrDefaultAsync(e => e.UserId == id);

            if (user == null)
            {
                TempData["msg"] = "User not found";
                return RedirectToAction("UserList");
            }

            user.IsActive = 1;
            user.UpdatedAt = DateTime.Now;

            await db.SaveChangesAsync();

            TempData["msg"] = "User activated successfully";

            return RedirectToAction("UserList");
        }


        // Deactivate User
        public async Task<IActionResult> DeactivateUser(int id)
        {
            string admin = HttpContext.Session.GetString("Admin");

            if (string.IsNullOrEmpty(admin))
            {
                return RedirectToAction("AdminLogin");
            }

            TblUser user = await db.TblUsers
                .FirstOrDefaultAsync(e => e.UserId == id);

            if (user == null)
            {
                TempData["msg"] = "User not found";
                return RedirectToAction("UserList");
            }

            user.IsActive = 0;
            user.UpdatedAt = DateTime.Now;

            await db.SaveChangesAsync();

            TempData["msg"] = "User deactivated successfully";

            return RedirectToAction("UserList");
        }


        // Admin Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Admin");
            HttpContext.Session.Remove("AdminId");
            return RedirectToAction("AdminLogin");
        }
    }
}
    

