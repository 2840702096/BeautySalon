using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace BeautySalon.Models.Entities
{
    public class HappyClients
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string FullName { get; set; }

        [Display(Name = "نظر مشتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Opinion { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
    }
}
