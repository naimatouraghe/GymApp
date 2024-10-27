namespace APISalledeSport.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime DateHeure { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; }

        public int ActiviteId { get; set; }
        public Activite Activite { get; set; }
    }
}
