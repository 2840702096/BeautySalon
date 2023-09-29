using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Models.Entities
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int? Score { get; set; }
        public string ValidationCode { get; set; }
        public string ImageName { get; set; }

        #region Relations

        public List<Reservations> Reservations { get; set; }

        #endregion

    }
}
