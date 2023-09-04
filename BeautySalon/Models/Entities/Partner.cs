using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace BeautySalon.Models.Entities
{
    public class Partner
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Title { get; set; }

        [Display(Name = "عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
    }
}
