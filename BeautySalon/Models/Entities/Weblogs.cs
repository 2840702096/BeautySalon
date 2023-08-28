using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace BeautySalon.Models.Entities
{
    public class Weblogs
    {
        public Weblogs()
        {
            
        }

        [Key]
        public int WeblogId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        [Display(Name = "عنوان وبلاگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string WeblogTitle { get; set; }
        public DateTime CreateDate { get; set; }
        public int Visit { get; set; }

        [Display(Name = "عکس وبلاگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }

        [Display(Name = "متن وبلاگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string WeblogBody { get; set; }

        public bool IsActive { get; set; }

        #region Relations

        [ForeignKey("CategoryId")]
        public SubJob SubJob { get; set; }

        #endregion
    }
}
