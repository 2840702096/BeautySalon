using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Models.Entities
{
    public class WorkingTime
    {
        [Key]
        public int Id { get; set; }
        public int DayId { get; set; }
        public string DayName { get; set; }
        public DateTime DayDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string TimePeriodText { get; set; }
        public bool IsActive { get; set; }
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string Price { get; set; }
        public string ReservationCost { get; set; }
        public bool IsReserved { get; set; }

        #region Relations

        [ForeignKey("DayId")]
        public WorkingDays WorkingDays { get; set; }

        [ForeignKey("AdminId")]
        public Admin Admin { get; set; }

        public List<Reservations> Reservations { get; set; }

        #endregion

    }
}
