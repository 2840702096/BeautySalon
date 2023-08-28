using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BeautySalon.Models.ViewModels
{
    public class EditProfileInfoViewModel
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FullName { get; set; }

        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Phone { get; set; }
        public string CurrentImage { get; set; }

        [Display(Name = "عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile ImageName { get; set; }
    }
}
