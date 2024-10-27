namespace APISalledeSport.Models
{
    public class AddAbonnementRequest
    {
        public int UtilisateurId { get; set; }     // The ID of the user
        public int AbonnementId { get; set; }       // The ID of the abonnement
        public decimal Prix { get; set; }
    }
}
