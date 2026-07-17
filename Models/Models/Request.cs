using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Request
    {
        [Key]
        public Guid Id { get; set; }

        public int RequestNo { get; set; } 


        [Required(ErrorMessage = "رقم المستخدم مطلوب.")]
        public Guid UserId { get; set; } 
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required(ErrorMessage = "نوع الطلب مطلوب.")]
        public string RequestType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // بيانات الابتكار الجديد:

        public bool IsAlreadyImplemented { get; set; }

        [Required(ErrorMessage = "الدولة مطلوبة.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "الوصف المختصر مطلوب.")]
        [MaxLength(300, ErrorMessage = "الوصف المختصر لا يمكن أن يتجاوز 300 حرف.")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "تأثير الابتكار مطلوب.")]
        [MaxLength(500, ErrorMessage = "الوصف لا يمكن أن يتجاوز 500 حرف.")]
        public string HealthImpactDescription { get; set; }
        // الغرض من تقديم الحل
        [MaxLength(200, ErrorMessage = "الغرض لا يمكن أن يتجاوز 200 حرف.")]
        public string SelectedOption { get; set; }


        [Required(ErrorMessage = "نضج الحل مطلوب.")]
        public string SolutionMaturity { get; set; }

        [Required(ErrorMessage = "تميّز الفكرة مطلوب.")]
        [MaxLength(500, ErrorMessage = "الوصف لا يمكن أن يتجاوز 500 حرف.")]
        public string UniquenessDescription { get; set; }

        [MaxLength(500, ErrorMessage = "الاحتياجات لا يمكن أن تتجاوز 500 حرف.")]
        public string ExperimentRequirements { get; set; }

        [Required(ErrorMessage = "نوع التقنية مطلوب.")]
        public string TechnologyCategory { get; set; }

        [Required(ErrorMessage = "الفئة المستهدفة مطلوبة.")]
        [MaxLength(200, ErrorMessage = "الفئة المستهدفة لا يمكن أن تتجاوز 200 حرف.")]
        public string TargetAudience { get; set; }

        [MaxLength(300, ErrorMessage = "حجم السوق لا يمكن أن يتجاوز 300 حرف.")]
        public string MarketSize { get; set; }

        [MaxLength(300, ErrorMessage = "المهارات لا يمكن أن تتجاوز 300 حرف.")]
        public string TeamSkillsAndResources { get; set; }

        public string? AttachedFilePath { get; set; }

        [Required(ErrorMessage = "حالة الطلب مطلوبة.")]
        [MaxLength(50, ErrorMessage = "الحالة لا يمكن أن تتجاوز 50 حرف.")]
        public string Status { get; set; }

        [MaxLength(200, ErrorMessage = "الإجراء المطلوب لا يمكن أن يتجاوز 200 حرف.")]
        public string ActionRequired { get; set; }
        public DateTime RequestDate { get; set; }
        public virtual Attachment? Attachment { get; set; }

    }
}
