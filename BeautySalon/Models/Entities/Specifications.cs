using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace BeautySalon.Models.Entities
{
    public class Specifications
    {
        public int Id { get; set; }
        public string Image { get; set; }

        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Title { get; set; }

        [Display(Name = "متن ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Body { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
