using System.ComponentModel.DataAnnotations;
namespace RequestSystem.Areas.Customer.ViewModels
{
    public class OtpInputViewModel
    {        
            public string Email { get; set; }

            [Required(ErrorMessage = "رمز التحقق مطلوب")]
            public string Code { get; set; }
        public string RequestType { get; set; }
    }
}
