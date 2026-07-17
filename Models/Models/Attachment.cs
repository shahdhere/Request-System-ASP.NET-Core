using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Attachment
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("RequestId")]
        public Guid RequestId { get; set; } // اللي يربط الملف بالطلب

        public string FileName { get; set; }
        public string FileExtension { get; set; } 
        public string ContentType { get; set; }
        public byte[] FileData { get; set; } // Binary
        public DateTime UploadDate { get; set; }
        public virtual Request Request { get; set; }
    }
}
