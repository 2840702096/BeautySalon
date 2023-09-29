using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BeautySalon.Models.Entities
{
    public class Reservations
    {
        public Reservations()
        {
            
        }

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WorkingTimeId { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int ReservationCost { get; set; }
        public int? FinalPayment { get; set; }
        public int JobId { get; set; }
        public string JopName { get; set; }
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime DayDate { get; set; }
        public string DayName { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("WorkingTimeId")]
        public WorkingTime WorkingTime { get; set; }

        [ForeignKey("AdminId")]
        public Admin Admin { get; set; }

        #endregion

    }
}
