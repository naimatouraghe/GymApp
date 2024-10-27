namespace APISalledeSport.Models
{
    public class PaiementDto
    {
        public int Id { get; set; } // Unique identifier for the payment
        public decimal Prix { get; set; } // Amount paid
        public DateTime Date { get; set; } // Payment date

        // These properties associate the payment with a user and an abonnement
        public int UtilisateurId { get; set; } // ID of the user making the payment
        public int AbonnementId { get; set; } // ID of the associated abonnement
        public string TypeAbonnement { get; set; } // Added for plan selection

        // Optional: Consider adding more details if needed
        // e.g., PaymentMethod (string), Status (string) etc.
    }
}
