using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models.IRepository;
using NEWS_App.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.Scripting;
using System.Security.Claims;
using NEWS_App.Models.IRepositoryImpl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace NEWS_App.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        public UserController(IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }


        public static string GenerateOTP()
        {
            Random random = new Random();
            int otp = random.Next(100000, 1000000); 
            return otp.ToString("D6"); 
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.notification = true;
            var user1 = await _userRepository.GetByUsernameAsync(user.Email);

                  if (user1 != null)
                 {
                        ModelState.AddModelError("Email", "Email already exists.");

                         return View(user);
                 }

            string userJson = JsonConvert.SerializeObject(user);
            HttpContext.Session.SetString("UserSession", userJson);

            var otp = GenerateOTP();
            HttpContext.Session.SetString("SinupOTP", otp);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com") // SMTP server
            {
                Port = 587,
                Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("MAIL"), "gejq ukif bayt huio"),
                EnableSsl = true 
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(Environment.GetEnvironmentVariable("MAIL")),
                Subject = "Social news",
                Body = $"<p>Dear {user.Username}</p>{otp}<p>",
                IsBodyHtml = true 
            };

            mailMessage.To.Add(user.Email); 

            try
            {
                smtpClient.Send(mailMessage); 
                Console.WriteLine("Email sent successfully.");
                return RedirectToAction("Verification");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }

            return RedirectToAction("Signup");
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string PasswordHash)
        {
            if (await _userRepository.ValidateUser(email, PasswordHash))
            {
               var user1 = await _userRepository.GetByUsernameAsync(email);

                var NotificationCount= _notificationRepository.CountByUserId(user1.Id);

                HttpContext.Session.SetInt32("UserId", user1.Id);
                HttpContext.Session.SetString("Username", email);
                HttpContext.Session.SetInt32("Notification", NotificationCount);
                if (user1.notification) {
                    HttpContext.Session.SetInt32("Status", 1);
                 }
                else
                {
                    HttpContext.Session.SetInt32("Status", 0);
                }
                //  HttpContext.Session.SetString("IsAuthenticated", "true");

                var returnUrl = HttpContext.Session.GetString("ReturnUrl");
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    HttpContext.Session.Remove("ReturnUrl");
                    return Redirect(returnUrl);
                }
                return RedirectToAction("index", "Articles");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }
        [HttpGet]
        public IActionResult Forgot()
        {
        return View();
        }
        [HttpPost]
        public async Task<IActionResult> Forgot(string email)
        {
            var otp = GenerateOTP();
            HttpContext.Session.SetString("ForgotOTP", otp);
            HttpContext.Session.SetString("Forgotemail", email);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com") // SMTP server
            {
                Port = 587,
                Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("MAIL"), "pqyo yoqe yqxa kvjd"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(Environment.GetEnvironmentVariable("MAIL")),
                Subject = "Social news",
                Body = $"<p>{otp}<p>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return RedirectToAction("Verification");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
            return RedirectToAction("Forgot");

        }
        [HttpGet]
        public IActionResult Verification()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Verification(string otp)
        {
            string userJson = HttpContext.Session.GetString("UserSession");
            string otpcode = HttpContext.Session.GetString("SinupOTP");

            string Forgototp= HttpContext.Session.GetString("ForgotOTP");
            if (otpcode ==null && Forgototp == null)
                return RedirectToAction("Login");

            if (otpcode == otp)
            {
                HttpContext.Session.Remove("UserSession");
                HttpContext.Session.Remove("SinupOTP");
                if (!string.IsNullOrEmpty(userJson))
                {
                    User user = JsonConvert.DeserializeObject<User>(userJson);
                   _userRepository.AddAsync(user);
                    return RedirectToAction("Login");
                }
                
            }
            if(Forgototp == otp)
            {
                return RedirectToAction("ChangePassword");
            }

            ModelState.AddModelError(string.Empty, "Invalid OTP.");
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string confirmPassword)
        {
            string Forgotemail = HttpContext.Session.GetString("Forgotemail");
            if(Forgotemail == null)
                return RedirectToAction("Login");

            var user1 = await _userRepository.GetByUsernameAsync(Forgotemail);
            if (user1 == null)
            {
                ModelState.AddModelError("Email", "Email already exists.");
               return RedirectToAction("Login");
            }
            user1.PasswordHash= confirmPassword;
            await _userRepository.updateUser(user1);
            return RedirectToAction("Login");

        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Articles");
        }
        [HttpPost]
        public async Task<IActionResult> Notification()
        {
            var userId=HttpContext.Session.GetInt32("UserId") ?? 0;
            var noti = false;
            if (userId == 0) {
                return Json(noti);
            }
            noti = await _userRepository.ToggleNotifications(userId);
            return Json(noti);
        }

    }
}
