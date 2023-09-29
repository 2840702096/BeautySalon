using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace BeautySalon.Models.Entities
{
    public class HappyClients
    {
        public HappyClients()
        {
            
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int User { get; set; }

        public string ImageName { get; set; }
        public string FullName { get; set; }

        [Display(Name = "نظر مشتری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Opinion { get; set; }
        public DateTime Date { get; set; }

        #region Relations

        [ForeignKey("User")]
        public User UserId { get; set; }

        #endregion
    }
}
