using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class Document : BaseModel
    {
        #region -- Properties --

        [StringLength(255)]
        [Required]
        public string Filename { get; set; }

        [StringLength(64)]
        [Required]
        public string GuidFilename { get; set; }

        [DisplayFormat(DataFormatString="{0:MM/dd/yyyy}")]
        public DateTime DateUploaded { get; set; }

        public int DocumentTypeId { get; set; }
        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentType { get; set; }

        public int? ActionMemoId { get; set; }
        [ForeignKey("ActionMemoId")]
        public ActionMemo ActionMemo { get; set; }

        #endregion
    }
}
