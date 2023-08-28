using System;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Models.Entities
{
    public class Gallery
    {
        [Key]
        public int id { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
