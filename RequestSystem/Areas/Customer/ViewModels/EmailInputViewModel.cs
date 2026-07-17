using System.ComponentModel.DataAnnotations;

namespace RequestSystem.Areas.Customer.ViewModels
{
    public class EmailInputViewModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }
    }
}