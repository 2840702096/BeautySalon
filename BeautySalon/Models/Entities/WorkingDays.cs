using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace BeautySalon.Models.Entities
{
    public class WorkingDays
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام روز")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        #region Relations

        public List<WorkingTime> WorkingTime { get; set; }

        #endregion

    }
}
