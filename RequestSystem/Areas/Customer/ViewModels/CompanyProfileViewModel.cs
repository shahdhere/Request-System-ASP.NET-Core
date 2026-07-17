using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RequestSystem.Areas.Customer.ViewModels
{
    public class CompanyProfileViewModel
    {
        [Required(ErrorMessage = "اسم الشركة مطلوب.")]
        [MaxLength(100, ErrorMessage = "اسم الشركة لا يمكن أن يتجاوز 100 حرف.")]
        public string CompanyName { get; set; } = null!;

        [Required(ErrorMessage = "نوع الشركة مطلوب.")]
        [MaxLength(50, ErrorMessage = "نوع الشركة لا يمكن أن يتجاوز 50 حرف.")]
        public string CompanyType { get; set; }

        [Required(ErrorMessage = "يجب تحديد إذا كانت هناك منشأة.")]
        public bool? HasFacility { get; set; }

        [Required(ErrorMessage = "رابط الشركة مطلوب.")]
        [MaxLength(200, ErrorMessage = "الرابط لا يمكن أن يتجاوز 200 حرف.")]
        public string CompanyURL { get; set; } 

        [Required(ErrorMessage = "يجب تحديد إذا كانت الشركة في السعودية.")]
        public bool? IsInSaudi { get; set; }

        [Required(ErrorMessage = "الوصف مطلوب.")]
        [MaxLength(500, ErrorMessage = "الوصف لا يمكن أن يتجاوز 500 حرف.")]
        public string CompanyDescription { get; set; } 

        [Required(ErrorMessage = "رأس المال مطلوب.")]
        [MaxLength(50, ErrorMessage = "رأس المال لا يمكن أن يتجاوز 50 حرف.")]
        public string CompanyCapital { get; set; } 

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [Phone(ErrorMessage = "الرجاء إدخال رقم هاتف صحيح.")]
        [MaxLength(20, ErrorMessage = "رقم الهاتف لا يمكن أن يتجاوز 20 رقم.")]
        public string CompanyPhone { get; set; } 

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال بريد إلكتروني صحيح.")]
        [MaxLength(100, ErrorMessage = "البريد الإلكتروني لا يمكن أن يتجاوز 100 حرف.")]
        public string CompanyEmail { get; set; }
        public List<SelectListItem> CompanyCapitalList { get; set; } = new List<SelectListItem>();


    }
}
