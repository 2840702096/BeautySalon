using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Models.Entities
{
    public class AboutUs
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
