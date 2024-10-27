using System.ComponentModel.DataAnnotations.Schema;

namespace APISalledeSport.Models
{
    public class ActiviteDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public int CapaciteMax { get; set; }
        public DateTime DateHeure { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }
    }
}
