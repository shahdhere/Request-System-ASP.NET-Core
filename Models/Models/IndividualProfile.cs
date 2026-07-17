using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class IndividualProfile
    {
        [Column("NewId")]
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // ربط مع جدول Users
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال بريد إلكتروني صحيح.")]
        [MaxLength(100, ErrorMessage = "البريد الإلكتروني لا يمكن أن يتجاوز 100 حرف.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب.")]
        [MaxLength(50, ErrorMessage = "الاسم لا يمكن أن يتجاوز 50 حرف.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "رقم الجوال مطلوب.")]
        [Phone(ErrorMessage = "الرجاء إدخال رقم جوال صحيح.")]
        [MaxLength(20, ErrorMessage = "رقم الجوال لا يمكن أن يتجاوز 20 رقم.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "المدينة مطلوبة.")]
        [MaxLength(50, ErrorMessage = "المدينة لا يمكن أن تتجاوز 50 حرف.")]
        public string City { get; set; }

        [Required(ErrorMessage = "المستوى التعليمي مطلوب.")]
        [MaxLength(50, ErrorMessage = "المستوى التعليمي لا يمكن أن يتجاوز 50 حرف.")]
        public string EducationLevel { get; set; }

        [MaxLength(100, ErrorMessage = "التخصص لا يمكن أن يتجاوز 100 حرف.")]
        public string Specialization { get; set; }

        [MaxLength(100, ErrorMessage = "جهة العمل لا يمكن أن تتجاوز 100 حرف.")]
        public string Organization { get; set; }

        [MaxLength(100, ErrorMessage = "المسمى الوظيفي لا يمكن أن يتجاوز 100 حرف.")]
        public string JobTitle { get; set; }

    }
}