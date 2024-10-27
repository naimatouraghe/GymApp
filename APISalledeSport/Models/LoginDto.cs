using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APISalledeSport.Models
{
    public class LoginDto
    {
        public string Email { get; set; }

        [Required]
        [Column("mot_de_passe")]  // Maps to 'mot_de_passe' in the database
        public string MotDePasse { get; set; }  // Use the correct property for password
    }
}
