using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BeautySalon.Models.ViewModels
{
    public class ReservationViewModel
    {
        [Display(Name = "زمان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int TimeId { get; set; }
        public string Description { get; set; }
    }
}
