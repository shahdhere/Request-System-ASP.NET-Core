using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RequestSystem.Areas.Customer.ViewModels
{
    public class NewRequestViewModel
    {
        // معلومات أساسية عن الطلب
        [Required(ErrorMessage = "يجب اختيار خيار*")]
        public string RequestType { get; set; } // فرد أو شركة

        // معلومات عن الابتكار
        [Required(ErrorMessage = "يجب اختيار خيار نعم أو لا*")]
        public bool? SolutionAlreadyImplemented { get; set; } // نعم أو لا
        [Required(ErrorMessage = "اختر الدولة*")]
        public string Country { get; set; } // الدولة
        public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "الحقل مطلوب*")]
        public string InnovationDescription { get; set; } // وصف مختصر (ShortDescription)
        [Required(ErrorMessage = "الحقل مطلوب*")]
        public string HowItServesHealthcare { get; set; } // تأثير الابتكار (HealthImpactDescription)
        [Required(ErrorMessage = "اختر الغرض*")]
        public string SelectedOption { get; set; } // الغرض من تقديم الحل
        public List<SelectListItem> MyNewOptions { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "اختر درجة النضج*")]
        public string SolutionMaturity { get; set; } // درجة نضج الحل
        public List<SelectListItem> SolutionMaturities { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "الحقل مطلوب*")]
        public string WhyUnique { get; set; } // وصف التميز (UniquenessDescription)
        [Required(ErrorMessage = "الحقل مطلوب*")]
        public string NeededResources { get; set; } // الاحتياجات التجريبية (ExperimentRequirements)
        [Required(ErrorMessage = "اختر التقنية*")]
        public string TechnologyCategory { get; set; } // التقنية المستخدمة
        public List<SelectListItem> TechnologyCategories { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "الحقل مطلوب*")]
        public string HowAligned { get; set; } // مدى المواءمة مع الأهداف الاستراتيجية (يمكن Optional)
        [Required(ErrorMessage = "الحقل مطلوب*")]
        public string TargetAudience { get; set; } // الفئة المستهدفة
        [Required(ErrorMessage = "الحقل مطلوب*")]
        public string MarketSize { get; set; } // حجم السوق
        [Required(ErrorMessage = "الحقل مطلوب*")]
        public string TeamSkills { get; set; } // مهارات الفريق والموارد
        public string FileName { get; set; }
        public IFormFile? Attachment { get; set; } // ملف PDF، optional
        public Guid? AttachmentId { get; set; }
        public string AttachedFilePath { get; set; }
        public object UserProfile { get; set; }
        public bool IsProfileReadOnly { get; set; } = false;
        public bool ShowProfile { get; set; }
        public IndividualProfileViewModel IndividualProfile { get; set; } = new IndividualProfileViewModel();
        public CompanyProfileViewModel CompanyProfile { get; set; } = new CompanyProfileViewModel();
    }
}
