using APISalledeSport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APISalledeSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all reservations
        [HttpGet]
        public IEnumerable<Reservation> GetReservations()
        {
            return _context.Reservations.ToList();
        }

        // Get a single reservation by Id
        [HttpGet("{utilisateurId}")]
        public IActionResult GetReservationsByUser(int utilisateurId)
        {
            var reservations = _context.Reservations
                .Include(r => r.Activite)
                .Include(r => r.Utilisateur)
                .Where(r => r.UtilisateurId == utilisateurId)
                .Select(r => new ReservationDto
                {
                    Id = r.Id,
                    DateHeure = r.DateHeure,
                    UtilisateurId = r.UtilisateurId,
                    ActiviteId = r.ActiviteId,
                    ActiviteNom = r.Activite.Nom // Assuming you have a property 'Nom' in Activite
                })
                .ToList();

            return Ok(reservations);
        }


        // POST: api/Reservation
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(ReservationDto reservationDto)
        {
            // Validate foreign keys
            var utilisateurExists = await _context.Utilisateurs.AnyAsync(u => u.Id == reservationDto.UtilisateurId);
            var activiteExists = await _context.Activites.AnyAsync(a => a.Id == reservationDto.ActiviteId);

            if (!utilisateurExists || !activiteExists)
            {
                return BadRequest("Invalid UtilisateurId or ActiviteId.");
            }

            var reservation = new Reservation
            {
                DateHeure = reservationDto.DateHeure,
                UtilisateurId = reservationDto.UtilisateurId,
                ActiviteId = reservationDto.ActiviteId
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservationsByUser), new { utilisateurId = reservation.UtilisateurId }, reservation);

        }

        // PUT: api/Reservation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, ReservationDto reservationDto)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Update the fields
            reservation.DateHeure = reservationDto.DateHeure;
            reservation.UtilisateurId = reservationDto.UtilisateurId;
            reservation.ActiviteId = reservationDto.ActiviteId;

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Reservation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to check if a reservation exists
        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}