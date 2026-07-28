using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using UserManagement.Services;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace UserManagement.Controllers
{
    public class UserController : Controller
    {
        IWebHostEnvironment env;//to access root diff folders photo
        IExtraService extraService;
        IUserService UserService;
        UserDbContext db;
        IMapper Mapper;
        public UserController(UserDbContext db, IMapper Mapper, IWebHostEnvironment env,IExtraService extraService,IUserService userService)
        {
            this.db = db;
            this.Mapper = Mapper;
            this.env = env;
            this.extraService= extraService;
            this.UserService= userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UserRegister()
        {
            List<TblRole> lst = db.TblRoles.ToList();
            ViewBag.Roles = new SelectList(lst, "RoleId", "RoleName");
            ViewData["user"] = db.TblUsers.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRegister(UserRegisterDTO UserRDto, IFormFile ProfilePhoto)
        {
            if(!ModelState.IsValid)
            {
                List<TblRole>lst=db.TblRoles.ToList();
                ViewBag.roles = new SelectList(lst, "RoleId", "RoleName");
                return View(UserRDto);
            }
            HttpContext.Session.SetString("User",JsonConvert.SerializeObject(UserRDto));
            string photo = UserRDto.UserName + Path.GetExtension(ProfilePhoto.FileName);
            String photopath = env.WebRootPath + "/ProfilePhoto/" + photo;
            string otp = await extraService.GenerateOTP(6);
            string message = $"<h2>Dear {UserRDto.UserName},<h2><p>Your email confirmation otp is<b>{otp}</b></p>";
            EmailModel model = new EmailModel()
            {
                UserName = UserRDto.UserName,
                EmailId = UserRDto.EmailAddress,
                Subject = "OTP for Email Confirmation",
                Message = message

            };
            UserRDto.ProfilePhoto = photo;

            await extraService.SendEmail(model);
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(UserRDto));
            HttpContext.Session.SetString("otp", otp);
            

            FileStream fs = new FileStream(photopath, FileMode.Create);

            ProfilePhoto.CopyTo(fs);
            fs.Close();




            TblUser user = Mapper.Map<TblUser>(UserRDto);
            user.CreatedAt = DateTime.Now;

            //db.TblUsers.Add(user);

            //db.SaveChanges();
            //ModelState.Clear();
            ViewBag.Message = "User Registered Successfully";



            return RedirectToAction("ConfirmOTP");
        }
        [HttpGet]
        public IActionResult ConfirmOTP()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmOTP(string user_otp)
        {
            string otp = HttpContext.Session.GetString("otp");
            Console.WriteLine("session OTP:"+otp);
            Console.WriteLine("Entered OTP:"+user_otp);
            if (string.IsNullOrWhiteSpace(user_otp))
            {
                ViewBag.errormsg = "Please Enter OTP";
                return View();
            }
            if (string.IsNullOrWhiteSpace(otp))
            {
                ViewBag.errormsg = "OTP Expired";
                return View();
            }
            string json = HttpContext.Session.GetString("User");
            if (!otp.Trim().Equals(user_otp.Trim()))
            {
                ViewBag.errormsg = "Invalid OTP";
                return View();
            }
            UserRegisterDTO dto=System.Text.Json.JsonSerializer.Deserialize<UserRegisterDTO>(json);

            string password = await extraService.GeneratePassword(10);

            TblUser user = Mapper.Map<TblUser>(dto);
            user.ProfilePhoto = dto.ProfilePhoto;
            user.Password = password;
            user.CreatedAt = DateTime.Now;
            user.RegistrationDate = DateTime.Now;
            user.IsActive = 0;
            user.Flag = 1;

            db.TblUsers.Add(user);
            await db.SaveChangesAsync();

            EmailModel email = new EmailModel()
            {
                UserName = user.UserName,
                EmailId = user.EmailAddress,
                Subject = "Registration Successful",
                Message = $@"
        <h2>Dear {user.UserName},</h2>

        <p>Your account has been created successfully.</p>

        <p>
        Email : <b>{user.EmailAddress}</b><br/>
        Password : <b>{password}</b>
        </p>"
            };

            await extraService.SendEmail(email);

            HttpContext.Session.Remove("otp");
            HttpContext.Session.Remove("User");

            ViewBag.regmsg = "Registration Successful!!!!! Please check your EmailId for Login Credentials";

            return RedirectToAction("UserLogin");
        }
          
        
        
        
        [HttpGet]
        public IActionResult UserLogin()
        {

            return View();
        }
        [HttpPost]
        [HttpPost]
        public IActionResult UserLogin(UserLoginDTO loginDto)
        {
            if (!ModelState.IsValid)

            {
                return View(loginDto);
            }
            var user = db.TblUsers.FirstOrDefault(e =>
        e.EmailAddress == loginDto.EmailAddress &&
        e.Password == loginDto.Password);

            if (user == null)
            {
                ViewBag.errormsg = "Invalid EmailId or Password";
                return View(loginDto);
            }

            // Check user status
            if (user.IsActive == 0)
            {
                ViewBag.errormsg =
                    "Your account is inactive. Please contact Admin for activation.";

                return View(loginDto);
            }

            // User is Active
            HttpContext.Session.SetString(
                "EmailAddress",
                user.EmailAddress);

            HttpContext.Session.SetInt32(
                "UserId",
                user.UserId);

            HttpContext.Session.SetString(
                "User",
                JsonConvert.SerializeObject(user));

            return RedirectToAction("UserProfile");
        }
        public IActionResult UserProfile()
        {
            int? userId =
           HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("UserLogin");
            }

            TblUser user = db.TblUsers.FirstOrDefault(x => x.UserId == userId.Value);

            if (user == null)
            {
                return RedirectToAction("UserLogin");
            }
            return View(user);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("UserLogin");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            string user = HttpContext.Session.GetString("User");
            if (user == null)
            {
                return RedirectToAction("UserLogin");
            }
            TblUser u = JsonConvert.DeserializeObject<TblUser>(user);
            ChangePasswordDTO cp = new ChangePasswordDTO()
            {
                UserID = u.UserId,
                EmailAddress = u.EmailAddress
            };

            return View(cp);
        }





        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            // Check current password
            UserLoginDTO ur = new UserLoginDTO()
            {
                EmailAddress = p.EmailAddress,
                Password = p.CurrentPassword
            };

            UserDTO urg = await UserService.CheckLogin(ur);

            if (urg == null)
            {
                ViewBag.errormsg = "Current password does not match.";

                return View(p);
            }

            // Check new and confirm password
            if (p.NewPassword != p.ConfirmPassword)
            {
                ViewBag.errormsg = "New password and confirm password do not match.";

                return View(p);
            }

            // Change password
            await UserService.ChangePassword(p);

            ViewBag.msg = "Password changed successfully.";

            ModelState.Clear();

            return View();
        }
        [HttpGet]
        public IActionResult ChangePhoto()
        {
            string userJson = HttpContext.Session.GetString("User");

            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("UserLogin");
            }

            TblUser user = JsonConvert.DeserializeObject<TblUser>(userJson);
            return View(user);
        }
       


        [HttpPost]
        public async Task<IActionResult> ChangePhoto(IFormFile ProfilePhoto)
        {
            // Get logged-in user ID from session
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("UserLogin");
            }

            // Check file
            if (ProfilePhoto == null || ProfilePhoto.Length == 0)
            {
                ViewBag.errormsg = "Please select a profile photo.";

                TblUser user = db.TblUsers.FirstOrDefault(e => e.UserId == userId.Value);

                return View(user);
            }

            // Get user from database
            TblUser existingUser = db.TblUsers.FirstOrDefault(e => e.UserId == userId.Value);

            if (existingUser == null)
            {
                return RedirectToAction("UserLogin");
            }

            // Get extension
            string extension = Path.GetExtension(ProfilePhoto.FileName);

            // Create new photo name
            string photoName = existingUser.UserName + extension;

            // Create folder path
            string folderPath = Path.Combine(
                env.WebRootPath,
                "ProfilePhoto"
            );

            // Create folder if not exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Full file path
            string filePath = Path.Combine(
                folderPath,
                photoName
            );

            // Save photo
            using (FileStream fs = new FileStream(
                filePath,
                FileMode.Create))
            {
                await ProfilePhoto.CopyToAsync(fs);
            }

            // Update database
            existingUser.ProfilePhoto = photoName;

            db.TblUsers.Update(existingUser);

            await db.SaveChangesAsync();

            // Update session
            HttpContext.Session.SetString(
                "User",
                JsonConvert.SerializeObject(existingUser)
            );

            ViewBag.msg = "Profile photo changed successfully.";

            return View(existingUser);
        }
    }
}
    

    




    

