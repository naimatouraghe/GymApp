namespace APISalledeSport.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public string Telephone { get; set; }
        public string Role { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>(); // Navigation property to Reservations
        public ICollection<Abonnement> Abonnements { get; set; } = new List<Abonnement>();
        public ICollection<Paiement> Paiements { get; set; } = new List<Paiement>();

    }
}
