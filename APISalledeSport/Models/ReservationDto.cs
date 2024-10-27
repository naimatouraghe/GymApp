namespace APISalledeSport.Models
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime DateHeure { get; set; }
        public int UtilisateurId { get; set; }
        public int ActiviteId { get; set; }
        public string ActiviteNom { get; set; }
    }
}