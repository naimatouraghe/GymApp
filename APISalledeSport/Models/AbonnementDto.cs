namespace APISalledeSport.Models
{
    public class AbonnementDto
    {
        public int Id { get; set; } // Used for updating an existing abonnement
        public string Type { get; set; }
        public decimal Prix { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }

        // This will be used to associate the abonnement with a user
        public int UtilisateurId { get; set; }

        // Use a collection for multiple payments related to this abonnement
        public List<int> PaiementIds { get; set; } // Changed from a single PaiementId to a list
    }
}
