using APISalledeSport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APISalledeSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaiementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaiementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Récupérer un paiement par ID avec détails utilisateur et abonnement
        [HttpGet("{id}")]
        public ActionResult<Paiement> GetPaiement(int id)
        {
            var paiement = _context.Paiements
                .Include(p => p.Utilisateur)
                .Include(p => p.Abonnement)
                .FirstOrDefault(p => p.Id == id);

            if (paiement == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                paiement.Id,
                paiement.Prix,
                paiement.Date,
                paiement.UtilisateurId,
                Utilisateur = new
                {
                    paiement.Utilisateur.Id,
                    paiement.Utilisateur.Nom,
                    paiement.Utilisateur.Email,
                    paiement.Utilisateur.Telephone,
                    paiement.Utilisateur.Role,
                    Reservations = paiement.Utilisateur.Reservations.Select(r => new
                    {
                        r.Id,
                        r.DateHeure,
                        r.UtilisateurId,
                        Utilisateur = r.Utilisateur,
                        r.ActiviteId,
                        Activite = new
                        {
                            r.Activite.Id,
                            r.Activite.Nom,
                            r.Activite.Description,
                            r.Activite.CapaciteMax,
                            r.Activite.DateHeure,
                            r.Activite.ImageUrl
                        }
                    }).ToList()
                },
                paiement.AbonnementId,
                Abonnement = new
                {
                    paiement.Abonnement.Id,
                    paiement.Abonnement.Type,
                    paiement.Abonnement.Prix,
                    paiement.Abonnement.DateDebut,
                    paiement.Abonnement.DateFin,
                    paiement.Abonnement.UtilisateurId
                }
            });
        }

        [HttpPost]
        public IActionResult CreatePaiementAndAbonnement([FromBody] PaiementDto paiementDto)
        {
            if (paiementDto == null)
            {
                return BadRequest("Invalid payment data.");
            }

            var utilisateur = _context.Utilisateurs.Find(paiementDto.UtilisateurId);
            if (utilisateur == null)
            {
                return BadRequest("Invalid utilisateur.");
            }

            // Define abonnement based on user selection
            Abonnement newAbonnement;
            switch (paiementDto.TypeAbonnement)
            {
                case "mensuel":
                    newAbonnement = new Abonnement
                    {
                        Type = "mensuel",
                        Prix = paiementDto.Prix,
                        DateDebut = DateTime.UtcNow,
                        DateFin = DateTime.UtcNow.AddMonths(1) // Adjust as necessary
                    };
                    break;

                case "trimestriel":
                    newAbonnement = new Abonnement
                    {
                        Type = "trimestriel",
                        Prix = paiementDto.Prix * 1.5m, // Example of pricing logic
                        DateDebut = DateTime.UtcNow,
                        DateFin = DateTime.UtcNow.AddMonths(3) // Longer duration
                    };
                    break;

                case "annuel":
                    newAbonnement = new Abonnement
                    {
                        Type = "annuel",
                        Prix = paiementDto.Prix * 2, // Example of pricing logic
                        DateDebut = DateTime.UtcNow,
                        DateFin = DateTime.UtcNow.AddMonths(6) // Longer duration
                    };
                    break;

                default:
                    return BadRequest("Invalid subscription type.");
            }

            newAbonnement.UtilisateurId = paiementDto.UtilisateurId; // Link to the user
            _context.Abonnements.Add(newAbonnement);
            _context.SaveChanges(); // Save the abonnement to get the ID

            // Now create the Paiement associated with the new Abonnement
            var newPaiement = new Paiement
            {
                Prix = paiementDto.Prix,
                UtilisateurId = paiementDto.UtilisateurId,
                AbonnementId = paiementDto.AbonnementId,
                TypeAbonnement = paiementDto.TypeAbonnement // Make sure this is set
            };

            _context.Paiements.Add(newPaiement);
            _context.SaveChanges();

            // Return the created paiement and abonnement details
            return CreatedAtAction(nameof(GetPaiement), new { id = newPaiement.Id }, new
            {
                Paiement = new
                {
                    newPaiement.Id,
                    newPaiement.Prix,
                    newPaiement.Date,
                    newPaiement.UtilisateurId,
                    AbonnementId = newAbonnement.Id,
                    Abonnement = new
                    {
                        newAbonnement.Id,
                        newAbonnement.Type,
                        newAbonnement.Prix,
                        newAbonnement.DateDebut,
                        newAbonnement.DateFin
                    }
                }
            });
        }


        // Other methods...
    }
}
