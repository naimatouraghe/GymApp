using System.ComponentModel.DataAnnotations;

namespace APISalledeSport.Models
{
    public class Abonnement
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public decimal Prix { get; set; }

        [Required]
        public DateTime DateDebut { get; set; }

        [Required]
        public DateTime DateFin { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; } // Ensure Utilisateur class is defined
                                                     // Navigation property to Payment
        public ICollection<Paiement> Paiements { get; set; }

    }
}
