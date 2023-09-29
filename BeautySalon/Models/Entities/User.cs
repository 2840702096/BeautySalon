using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace BeautySalon.Models.Entities
{
    public class User
    {
        public User()
        {
            
        }

        [Key]
        public int id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int? Score { get; set; }
        public string ValidationCode { get; set; }
        public bool IsActive { get; set; }
        public string ImageName { get; set; }

        #region Relations

        public List<Reservations> Reservations { get; set; }
        public List<User> Users { get; set; }

        #endregion

    }
}
