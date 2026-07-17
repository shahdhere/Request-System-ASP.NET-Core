using System.ComponentModel.DataAnnotations;

namespace RequestSystem.Areas.Customer.ViewModels
{
    public class SelectRequestType
    {
        [Required(ErrorMessage = "يجب اختيار خيار*")]
        public string RequestType { get; set; } // فرد أو شركة

    }
}
