using Microsoft.EntityFrameworkCore;

namespace APISalledeSport.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Abonnement> Abonnements { get; set; }
        public DbSet<Activite> Activites { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Paiement> Paiements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Utilisateur to Reservation relationship
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Utilisateur)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UtilisateurId);

            // Configure Reservation to Activite relationship
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Activite)
                .WithMany()
                .HasForeignKey(r => r.ActiviteId);

            // Configure Utilisateur properties
            modelBuilder.Entity<Utilisateur>()
                .Property(u => u.Role)
                .IsRequired(false); // Explicitly make Role optional

            // Configure Paiement properties and relationships
            modelBuilder.Entity<Paiement>()
                .Property(p => p.Prix)
                .HasPrecision(18, 2); // Define the precision

            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.Utilisateur)
                .WithMany(u => u.Paiements) // Assuming Utilisateur has a collection of Paiements
                .HasForeignKey(p => p.UtilisateurId);

            // Configure Abonnement properties and relationships
            modelBuilder.Entity<Abonnement>()
                .Property(a => a.Prix)
                .HasPrecision(18, 2); // Define the precision

            modelBuilder.Entity<Abonnement>()
                .HasOne(a => a.Utilisateur)
                .WithMany(u => u.Abonnements) // Assuming Utilisateur has a collection of Abonnements
                .HasForeignKey(a => a.UtilisateurId);

            // Configure the relationship between Abonnement and Paiement
            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.Abonnement)
                .WithMany(a => a.Paiements) // Assuming Abonnement has a collection of Payments
                .HasForeignKey(p => p.AbonnementId);
        }
    }
}
