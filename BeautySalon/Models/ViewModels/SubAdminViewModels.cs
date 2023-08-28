using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace BeautySalon.Models.ViewModels
{
    public class CreateWorkingTimeViewModel
    {
        [Display(Name = "روز")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Day { get; set; }

        [Display(Name = "ساعت شروع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "ساعت پایان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "قیمت اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Price { get; set; }

        [Display(Name = "هزینه رزرواسیون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ReservationCost { get; set; }
    }

    public class EditWorkingTimeViewModel
    {
        public int DayId { get; set; }
        public string DayName { get; set; }

        [Display(Name = "روز")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Day { get; set; }

        [Display(Name = "ساعت شروع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "ساعت پایان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "قیمت اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Price { get; set; }

        [Display(Name = "هزینه رزرواسیون")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ReservationCost { get; set; }
    }
}
