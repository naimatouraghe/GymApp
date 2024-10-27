using APISalledeSport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APISalledeSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiviteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActiviteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Récupérer toutes les activités
        [HttpGet]
        public ActionResult<IEnumerable<Activite>> GetActivites()
        {
            return Ok(_context.Activites.ToList());
        }

        // Récupérer une activité par ID
        [HttpGet("{id}")]
        public ActionResult<Activite> GetActivite(int id)
        {
            var activite = _context.Activites.Find(id);

            if (activite == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return Ok(activite);
        }

        // Créer une activité
        [HttpPost]
        public async Task<ActionResult<Activite>> CreateActivite([FromBody] ActiviteDto activiteDto)
        {
            if (activiteDto == null)
            {
                return BadRequest("Activite is null"); // Return 400 if request body is null
            }

            // Optionally, validate the input here, if needed

            // Create a new Activite instance and map properties
            var activite = new Activite
            {
                // If the ID is provided, you may choose to ignore it or handle accordingly
                // Here we just skip setting the Id for a new insert
                Nom = activiteDto.Nom,
                Description = activiteDto.Description,
                CapaciteMax = activiteDto.CapaciteMax,
                ImageUrl = activiteDto.ImageUrl
            };

            _context.Activites.Add(activite);
            await _context.SaveChangesAsync(); // Use async to save changes

            return CreatedAtAction(nameof(GetActivite), new { id = activite.Id }, activite); // Return 201 with the created resource
        }

        // Mettre à jour une activité
        // PUT: api/Activite/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivite(int id, ActiviteDto activiteDto)
        {
            // Find the existing Activite
            var activite = await _context.Activites.FindAsync(id);

            if (activite == null)
            {
                return NotFound(); // Return 404 if the activity is not found
            }

            // Update the fields from the DTO
            activite.Nom = activiteDto.Nom;
            activite.Description = activiteDto.Description;
            activite.CapaciteMax = activiteDto.CapaciteMax;
            activite.ImageUrl = activiteDto.ImageUrl;

            _context.Entry(activite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // Save changes asynchronously
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActiviteExists(id)) // Check if the activity still exists
                {
                    return NotFound(); // Return 404 if not found
                }
                else
                {
                    throw; // Rethrow the exception if it was a different issue
                }
            }

            return NoContent(); // Return 204 No Content on success
        }


        // Supprimer une activité
        // DELETE: api/Activite/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivite(int id)
        {
            var activite = await _context.Activites.FindAsync(id);

            if (activite == null)
            {
                return NotFound(); // Return 404 if the activity is not found
            }

            _context.Activites.Remove(activite); // Remove the activity
            await _context.SaveChangesAsync(); // Save changes asynchronously

            return NoContent(); // Return 204 No Content on success
        }

        private bool ActiviteExists(int id)
        {
            return _context.Activites.Any(e => e.Id == id); // Return true if found
        }

    }
}
