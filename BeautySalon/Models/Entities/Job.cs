using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace BeautySalon.Models.Entities
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام شغل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Name { get; set; }

        [Display(Name = "عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Image { get; set; }
        public bool IsActive { get; set; }

        #region Relations

        public List<SubJob> SubJob { get; set; }

        #endregion

    }
}
