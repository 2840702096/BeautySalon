using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Models.Entities
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int SubJobId { get; set; }
        public string SubJobName { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string UserName { get; set; }

        [Display(Name = "رمز")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Password { get; set; }
        public int AdminRole { get; set; }
        public int? Percent { get; set; }
        public string Phone { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Image { get; set; }


        #region Relations

        public SubJob SubJob { get; set; }

        public List<WorkingTime> WorkingTime { get; set; }

        public List<Reservations> Reservations { get; set; }

        #endregion
    }
}
