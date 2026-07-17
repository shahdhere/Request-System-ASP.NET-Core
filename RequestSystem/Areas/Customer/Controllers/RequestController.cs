using Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using RequestSystem.Areas.Customer.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;

namespace RequestSystem.Areas.Customer.Controllers
{
    //[Authorize]
    [Area("Customer")]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult SelectRequestType()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("EmailInput", "User", new { area = "Customer" });

            var user = _context.Users
                               .Include(u => u.IndividualProfile)
                               .Include(u => u.CompanyProfile)
                               .FirstOrDefault(u => u.Id == userId);

            if (!string.IsNullOrEmpty(user?.UserType))
            {
                HttpContext.Session.SetString("RequestType", user.UserType);
                return RedirectToAction("MyRequests");
            }

            if (user?.IndividualProfile != null)
            {
                HttpContext.Session.SetString("RequestType", "فرد");
                return RedirectToAction("MyRequests");
            }
            else if (user?.CompanyProfile != null)
            {
                HttpContext.Session.SetString("RequestType", "شركة");
                return RedirectToAction("MyRequests");
            }

            return View(new NewRequestViewModel());
        }

        [HttpPost]
        public IActionResult SelectRequestType(NewRequestViewModel model)
        {
            if (string.IsNullOrEmpty(model.RequestType))
            {
                ModelState.AddModelError("", "يرجى اختيار نوع التسجيل");
                return View(model);
            }

            HttpContext.Session.SetString("RequestType", model.RequestType);

            return RedirectToAction("MyRequests");
        }

        // ===== 4. إرسال الطلب =====
        public IActionResult CreateRequest(string requestType)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("EmailInput", "User", new { area = "Customer" });

            if (string.IsNullOrEmpty(requestType))
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null && !string.IsNullOrEmpty(user.UserType))
                {
                    requestType = user.UserType;
                    // احفظه في Session عشان الصفحة الجاية تستخدمه
                    HttpContext.Session.SetString("RequestType", requestType);
                }
                var isIndividual = requestType == "فرد" ? "فرد" : "شركة";
                var model = PrepareRequestViewModel(isIndividual);
                model.RequestType = requestType;
                model.IsProfileReadOnly = true;
                model.ShowProfile = false;
                return View(model);
            }
            else
            {
                var isIndividual = requestType == "شركة" ? "شركة" : "فرد";
                var model = PrepareRequestViewModel(isIndividual);
                model.RequestType = requestType;
                model.IsProfileReadOnly = false;
                model.ShowProfile = true;
                return View(model);
            }
        }

        public ApplicationDbContext Get_context()
        {
            return _context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(NewRequestViewModel model)
        {

            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("EmailInput", "User", new { area = "Customer" });

            var user = await _context.Users
                .Include(u => u.IndividualProfile)
                .Include(u => u.CompanyProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);


            // ===== 4. حفظ البروفايل =====
            if (model.RequestType == "فرد" && user.IndividualProfile == null)
            {

                var individualProfile = new IndividualProfile
                {
                    Id = Guid.NewGuid(), // العمود الجديد GUID
                    Email = model.IndividualProfile.Email,
                    UserId = userId.Value,
                    Name = model.IndividualProfile.Name,
                    PhoneNumber = model.IndividualProfile.PhoneNumber,
                    City = model.IndividualProfile.City,
                    EducationLevel = model.IndividualProfile.EducationLevel,
                    Specialization = model.IndividualProfile.Specialization,
                    Organization = model.IndividualProfile.Organization,
                    JobTitle = model.IndividualProfile.JobTitle
                };

                _context.IndividualProfiles.Add(individualProfile);

            }
            else if (model.RequestType == "شركة" && user.CompanyProfile == null)
            {
                var companyProfile = new CompanyProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = userId.Value,
                    CompanyName = model.CompanyProfile.CompanyName,
                    CompanyEmail = model.CompanyProfile.CompanyEmail,
                    CompanyPhone = model.CompanyProfile.CompanyPhone,
                    CompanyCapital = model.CompanyProfile.CompanyCapital,
                    CompanyURL = model.CompanyProfile.CompanyURL,
                    CompanyType = model.CompanyProfile.CompanyType,
                    CompanyDescription = model.CompanyProfile.CompanyDescription,
                    HasFacility = model.CompanyProfile.HasFacility
                };

                _context.CompanyProfiles.Add(companyProfile);
            }

            Request newRequest = new Request
            {
                Id = Guid.NewGuid(),
                RequestNo = GenerateUniqueRequestNo(),
                UserId = userId.Value,
                RequestType = model.RequestType,
                Country = model.Country,
                ShortDescription = model.InnovationDescription,
                HealthImpactDescription = model.HowItServesHealthcare,
                SelectedOption = model.SelectedOption,
                SolutionMaturity = model.SolutionMaturity,
                UniquenessDescription = model.WhyUnique,
                ExperimentRequirements = model.NeededResources,
                TechnologyCategory = model.TechnologyCategory,
                TargetAudience = model.TargetAudience,
                MarketSize = model.MarketSize,
                TeamSkillsAndResources = model.TeamSkills,
                CreatedAt = DateTime.Now,
                RequestDate = DateTime.Now,
                Status = "جديد",
                ActionRequired = "قيد المراجعة",
                IsAlreadyImplemented = model.SolutionAlreadyImplemented ?? false,
                AttachedFilePath = null
            };

            _context.Requests.Add(newRequest);
            await _context.SaveChangesAsync();


            // بعد حفظ الطلب
            if (user != null && user.UserType == null)
            {
                user.UserType = model.RequestType;
                await _context.SaveChangesAsync();
            }
            // رفع المرفقات
            if (model.Attachment != null && model.Attachment.Length > 0)
            {
                using var ms = new MemoryStream();
                await model.Attachment.CopyToAsync(ms);

                var attachment = new Attachment
                {
                    Id = Guid.NewGuid(),
                    RequestId = newRequest.Id,
                    FileName = Path.GetFileNameWithoutExtension(model.Attachment.FileName),
                    FileExtension = Path.GetExtension(model.Attachment.FileName),
                    ContentType = model.Attachment.ContentType,
                    FileData = ms.ToArray(),
                    UploadDate = DateTime.Now
                };
                _context.Attachments.Add(attachment);
                await _context.SaveChangesAsync();
                model.AttachmentId = attachment.Id;
                newRequest.AttachedFilePath = attachment.Id.ToString();
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("MyRequests", new { area = "Customer" });
        }


        // ===== 5. عرض جميع الطلبات =====
        public IActionResult MyRequests()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("EmailInput", "User", new { area = "Customer" });

            var requests = _context.Requests
                                   .Where(r => r.UserId == userId)
                                   .OrderByDescending(r => r.CreatedAt)
                                   .ToList();

            return View(requests);
        }
        public async Task<IActionResult> DownloadAttachment(Guid id)
        {
            var attachment = await _context.Attachments.FirstOrDefaultAsync(a => a.Id == id);
            if (attachment == null)
                return NotFound();

            return File(attachment.FileData, attachment.ContentType, string.Format("{0}.{1}", attachment.FileName, attachment.FileExtension));
        }

        public IActionResult RequestDetails(Guid id)
        {
            // جلب الطلب من قاعدة البيانات حسب Guid
            var request = _context.Requests.Include(r => r.Attachment).FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            // تحويل Request إلى ViewModel
            var model = new RequestDetailsViewModel
            {
                Id = request.Id,
                RequestNo = request.RequestNo,
                RequestType = request.RequestType,
                Country = request.Country,
                ShortDescription = request.ShortDescription,
                HealthImpactDescription = request.HealthImpactDescription,
                SolutionMaturity = request.SolutionMaturity,
                TechnologyCategory = request.TechnologyCategory,
                MarketSize = request.MarketSize,
                TargetAudience = request.TargetAudience,
                Status = request.Status,
                AttachmentId = request.Attachment != null ? request.Attachment.Id : null,
                ActionRequired = request.ActionRequired,
                CreatedAt = request.CreatedAt
            };

            return View(model);
        }

        // == Helpers ==
        private Guid? GetCurrentUserId()
        {
            // ابحث مباشرة في الـ Claims التي تم إنشاؤها عند VerifyOtp
            var claimId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(claimId)) 
                return null;

            return Guid.TryParse(claimId, out var guid) ? guid : null;
        }


        private NewRequestViewModel PrepareRequestViewModel(string isIndividual)
        {
            var userId = GetCurrentUserId();
            var model = PopulateSelectLists(new NewRequestViewModel());

            if (userId == null) return model;

            if (isIndividual == "فرد")
            {
                var user = _context.Users.Include(u => u.IndividualProfile).FirstOrDefault(u => u.Id == userId);
                if (user?.IndividualProfile != null)

                {
                    model.IndividualProfile = new IndividualProfileViewModel
                    {
                        Name = user.IndividualProfile.Name,
                        PhoneNumber = user.IndividualProfile.PhoneNumber,
                        City = user.IndividualProfile.City,
                        EducationLevel = user.IndividualProfile.EducationLevel,
                        Specialization = user.IndividualProfile.Specialization,
                        Organization = user.IndividualProfile.Organization,
                        JobTitle = user.IndividualProfile.JobTitle
                    };
                }
            }

            else
            {
                var user = _context.Users.Include(u => u.CompanyProfile).FirstOrDefault(u => u.Id == userId);
                if (user?.CompanyProfile != null)
                {
                    model.CompanyProfile = new CompanyProfileViewModel
                    {
                        CompanyName = user.CompanyProfile.CompanyName,
                        CompanyEmail = user.CompanyProfile.CompanyEmail,
                        CompanyPhone = user.CompanyProfile.CompanyPhone,
                        CompanyCapital = user.CompanyProfile.CompanyCapital,
                        HasFacility = user.CompanyProfile.HasFacility
                    };
                }
            }

            return model;
        }


        private int GenerateUniqueRequestNo()
        {
            int requestNo;
            do
            {
                requestNo = RandomNumberGenerator.GetInt32(1000, 10000);
            }
            while (_context.Requests.Any(r => r.RequestNo == requestNo));

            return requestNo;
        }


        private NewRequestViewModel PopulateSelectLists(NewRequestViewModel model = null)
        {
            var vm = model ?? new NewRequestViewModel();

            vm.Countries = new List<SelectListItem>
            {
                new SelectListItem { Value = "SA", Text = "السعودية" },
                new SelectListItem { Value = "AE", Text = "الإمارات" },
                new SelectListItem { Value = "EG", Text = "مصر" },
                new SelectListItem { Value = "KW", Text = "الكويت" },
                new SelectListItem { Value = "Other", Text = "دولة أخرى" }
            };

            vm.MyNewOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Option1", Text = "الاستفادة من الخبرات المتوفرة" },
                new SelectListItem { Value = "Option2", Text = "الوصول للبيانات المتاحة" },
                new SelectListItem { Value = "Option3", Text = "أخرى" }
            };

            vm.SolutionMaturities = new List<SelectListItem>
            {
                new SelectListItem { Value = "Idea", Text = "Idea" },
                new SelectListItem { Value = "POC", Text = "POC" },
                new SelectListItem { Value = "MVP", Text = "MVP" },
                new SelectListItem { Value = "Other", Text = "Other" }
            };

            vm.TechnologyCategories = new List<SelectListItem>
            {
                new SelectListItem { Value = "Digital Health", Text = "Digital Health" },
                new SelectListItem { Value = "Wearable Devices", Text = "Wearable Devices" },
                new SelectListItem { Value = "AR-VR-XR", Text = "AR-VR-XR" },
                new SelectListItem { Value = "Sensors & IoT", Text = "Sensors & IoT" },
                new SelectListItem { Value = "Robotics", Text = "Robotics" },
                new SelectListItem { Value = "mHealth", Text = "mHealth" },
                new SelectListItem { Value = "Blockchain", Text = "Blockchain" },
                new SelectListItem { Value = "AI", Text = "AI" },
                new SelectListItem { Value = "Other", Text = "Other" }
            };

            vm.CompanyProfile ??= new CompanyProfileViewModel();
            vm.CompanyProfile.CompanyCapitalList = new List<SelectListItem>
            {
                new SelectListItem { Value = "LessThan500k", Text = "أقل من 500,000 ريال" },
                new SelectListItem { Value = "500kTo1M", Text = "من 500,000 إلى أقل من 1,000,000 ريال" },
                new SelectListItem { Value = "1MTo2M", Text = "من 1,000,000 إلى أقل من 2,000,000 ريال" },
                new SelectListItem { Value = "MoreThan2M", Text = "أكثر من 2,000,000 ريال" },
                new SelectListItem { Value = "Other", Text = "أخرى" }
            };

            return vm;
        }
    }
}