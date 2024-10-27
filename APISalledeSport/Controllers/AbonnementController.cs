using APISalledeSport.Models;
using Microsoft.AspNetCore.Mvc;

namespace APISalledeSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonnementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AbonnementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Récupérer tous les abonnements
        [HttpGet]
        public ActionResult<IEnumerable<Abonnement>> GetAbonnements()
        {
            var abonnements = _context.Abonnements.ToList();
            if (abonnements == null || !abonnements.Any())
            {
                return NotFound("No abonnements found.");
            }
            return Ok(abonnements);
        }

        // Récupérer un abonnement par ID
        [HttpGet("{id}")]
        public ActionResult<Abonnement> GetAbonnement(int id)
        {
            var abonnement = _context.Abonnements.Find(id);
            if (abonnement == null)
            {
                return NotFound();
            }
            return Ok(abonnement);
        }

        // Créer un abonnement avec paiement
        [HttpPost]
        public IActionResult CreateAbonnement([FromBody] AbonnementDto abonnementDto)
        {
            // Validate input
            if (abonnementDto == null)
            {
                return BadRequest("Abonnement data is required.");
            }

            // Create Abonnement
            var newAbonnement = new Abonnement
            {
                Type = abonnementDto.Type,
                Prix = abonnementDto.Prix,
                DateDebut = abonnementDto.DateDebut,
                DateFin = abonnementDto.DateFin,
                UtilisateurId = abonnementDto.UtilisateurId
            };

            _context.Abonnements.Add(newAbonnement);
            _context.SaveChanges();

            // Create Paiement associated with the new Abonnement
            var newPaiement = new Paiement
            {
                Prix = abonnementDto.Prix,
                Date = DateTime.UtcNow,
                UtilisateurId = abonnementDto.UtilisateurId,
                AbonnementId = newAbonnement.Id // Link to the created Abonnement
            };

            _context.Paiements.Add(newPaiement);
            _context.SaveChanges();

            // Fetch Utilisateur for response (if needed)
            var utilisateur = _context.Utilisateurs.Find(abonnementDto.UtilisateurId);

            return CreatedAtAction(nameof(GetAbonnement), new { id = newAbonnement.Id }, new
            {
                Paiement = new
                {
                    newPaiement.Id,
                    newPaiement.Prix,
                    newPaiement.Date
                },
                Abonnement = new
                {
                    newAbonnement.Id,
                    newAbonnement.Type,
                    newAbonnement.Prix,
                    newAbonnement.DateDebut,
                    newAbonnement.DateFin
                },
                Utilisateur = utilisateur != null ? new
                {
                    utilisateur.Id,
                    utilisateur.Nom,
                    utilisateur.Email
                } : null
            });
        }

        // Supprimer un abonnement
        [HttpDelete("{id}")]
        public IActionResult DeleteAbonnement(int id)
        {
            var abonnement = _context.Abonnements.Find(id);
            if (abonnement == null)
            {
                return NotFound();
            }

            _context.Abonnements.Remove(abonnement);
            _context.SaveChanges();
            return NoContent();
        }

        // Récupérer les abonnements par ID d'utilisateur
        [HttpGet("Utilisateur/{utilisateurId}")]
        public ActionResult<IEnumerable<object>> GetAbonnementsByUserId(int utilisateurId)
        {
            if (utilisateurId <= 0)
            {
                return BadRequest("Invalid utilisateurId.");
            }

            var userExists = _context.Utilisateurs.Any(u => u.Id == utilisateurId);
            if (!userExists)
            {
                return NotFound($"Utilisateur with ID {utilisateurId} not found.");
            }

            var abonnements = _context.Abonnements
                .Where(a => a.UtilisateurId == utilisateurId)
                .Select(a => new
                {
                    a.Id,
                    a.Type,
                    a.Prix,
                    a.DateDebut,
                    a.DateFin
                })
                .ToList();

            if (!abonnements.Any())
            {
                return NotFound("No abonnements found.");
            }

            return Ok(abonnements);
        }
    }
}
