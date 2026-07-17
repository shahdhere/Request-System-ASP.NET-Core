using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RequestSystem.Areas.Customer.ViewModels;
using Models.Models;
using System.Security.Claims;
using System.Net;
using System.Net.Mail;

namespace RequestSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // EmailInput / OTP
        public IActionResult Index()
        {
            return RedirectToAction("EmailInput");
        }

        public IActionResult EmailInput()
        {
            return View(new EmailInputViewModel());
        }

        [HttpPost]
        public IActionResult SendOtp(EmailInputViewModel model)
        {
            if (!ModelState.IsValid)
                return View("EmailInput", model);

            string otp = new Random().Next(10000, 99999).ToString();
            DateTime expiryTime = DateTime.Now.AddMinutes(1);

            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("Email", model.Email);
            HttpContext.Session.SetString("OTPExpiry", expiryTime.ToString());

            SendOtpEmail(model.Email, otp);

            TempData["SuccessMessage"] = "تم إرسال رمز التحقق إلى بريدك الإلكتروني";

            return RedirectToAction("OtpInput");
        }

        public IActionResult OtpInput()
        {
            string email = HttpContext.Session.GetString("Email");
            if (email == null)
                return RedirectToAction("EmailInput");

            return View(new OtpInputViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(OtpInputViewModel model)
        {
            var savedOtp = HttpContext.Session.GetString("OTP");
            var email = HttpContext.Session.GetString("Email");
            var expiryString = HttpContext.Session.GetString("OTPExpiry");

            if (savedOtp == null || expiryString == null)
            {
                TempData["ErrorMessage"] = "انتهت صلاحية رمز التحقق، يرجى إعادة الإرسال";
                return RedirectToAction("OtpInput");
            }

            DateTime expiryTime = DateTime.Parse(expiryString);
            if (DateTime.Now > expiryTime)
            {
                TempData["ErrorMessage"] = "انتهت صلاحية رمز التحقق، يرجى إعادة الإرسال";
                HttpContext.Session.Remove("OTP");
                HttpContext.Session.Remove("OTPExpiry");
                return RedirectToAction("OtpInput");
            }

            if (model.Code != savedOtp || model.Email != email)
            {
                TempData["ErrorMessage"] = "رمز التحقق غير صحيح";
                return View("OtpInput", model);
            }

            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("OTPExpiry");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                user = new User { Id = Guid.NewGuid(), Email = email };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.Session.SetString("UserId", user.Id.ToString());

            }
            else if (user?.UserType != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                return RedirectToAction("MyRequests", "Request");

            }
            return RedirectToAction("SelectRequestType");

        }

        private void SendOtpEmail(string email, string otp)
        {
            var fromAddress = new MailAddress("za265545@gmail.com", "MOH");
            var toAddress = new MailAddress(email);
            const string fromPassword = "imaztatudrxggltb";
            const string subject = "رمز التحقق OTP";
            string body = $"رمز التحقق الخاص بك هو: {otp}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        [HttpPost]
        public IActionResult ResendOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "البريد الإلكتروني غير موجود لإعادة الإرسال";
                return RedirectToAction("OtpInput");
            }

            string otp = new Random().Next(10000, 99999).ToString();
            DateTime expiryTime = DateTime.Now.AddMinutes(1);

            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("Email", email);
            HttpContext.Session.SetString("OTPExpiry", expiryTime.ToString());

            SendOtpEmail(email, otp);

            TempData["SuccessMessage"] = "تم إرسال رمز التحقق من جديد إلى بريدك الإلكتروني";
            return RedirectToAction("OtpInput");
        }

        // SelectRequestType
        public IActionResult SelectRequestType()
        {
            var user = GetCurrentUser();
            if (user == null) return RedirectToAction("EmailInput");

            if (user.IndividualProfile != null || user.CompanyProfile != null)
                return RedirectToAction("MyRequests");

            return View(new NewRequestViewModel());
        }

        [HttpPost]
        public IActionResult SelectRequestType(NewRequestViewModel model)
        {
            var user = GetCurrentUser();
            if (user == null) return RedirectToAction("EmailInput");


            if (string.IsNullOrEmpty(model.RequestType))
            {
                ModelState.AddModelError("", "يرجى اختيار نوع التسجيل");
                return View(model);
            }
            // set session value for request type
            return RedirectToAction("CreateRequest", "Request", new { area = "Customer", requestType = model.RequestType });
        }

        // Create Individual Profile
        public IActionResult CreateIndividual()
        {
            return View(new IndividualProfileViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateIndividual(IndividualProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = GetCurrentUser();
            if (user == null) return RedirectToAction("EmailInput");

            var profile = new IndividualProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber
            };

            _context.IndividualProfiles.Add(profile);
            await _context.SaveChangesAsync();

            user.IndividualProfile = profile;
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("RequestType", "فرد");

            return RedirectToAction("MyRequests");
        }

        // Create Company Profile
        public IActionResult CreateCompany()
        {
            return View(new CompanyProfileViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = GetCurrentUser();
            if (user == null) return RedirectToAction("EmailInput");

            var profile = new CompanyProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CompanyName = model.CompanyName,
                CompanyEmail = model.CompanyEmail,
                CompanyPhone = model.CompanyPhone
            };

            _context.CompanyProfiles.Add(profile);
            await _context.SaveChangesAsync();

            user.CompanyProfile = profile;
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("RequestType", "شركة");

            return RedirectToAction("MyRequests");
        }

        // جلب المستخدم الحالي من Claims (بريد المستخدم)
        private User? GetCurrentUser()
        {
            var email = User?.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return null;
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
