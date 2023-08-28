using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BeautySalon.Models.Entities
{
    public class SubJob
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام شغل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Name { get; set; }
        public int Score { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string Price { get; set; }
        public string ReservationCost { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Image { get; set; }
        public bool IsActive { get; set; }

        #region Relations

        [ForeignKey("ParentId")]
        public Job Job { get; set; }

        public List<Admin> Admin { get; set; }

        public List<Weblogs> Weblogs { get; set; }

        #endregion
    }
}
