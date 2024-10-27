using APISalledeSport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APISalledeSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // For reading settings like JWT secret key

        public UtilisateurController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; // Inject configuration for secret key
        }

        // Récupérer tous les utilisateurs
        [HttpGet]
        public IEnumerable<Utilisateur> GetUtilisateurs()
        {
            return _context.Utilisateurs.ToList();
        }

        [HttpPost]
        public IActionResult CreateUtilisateur([FromBody] Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns a 400 Bad Request with validation errors
            }

            try
            {
                _context.Utilisateurs.Add(utilisateur);
                _context.SaveChanges();
                return Ok(utilisateur);
            }
            catch (DbUpdateException dbEx) // Catch database update errors
            {
                var innerException = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                return StatusCode(500, new { message = "An error occurred while saving changes", error = innerException });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        // Login endpoint to validate user credentials
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return 400 if the model is invalid
            }

            var user = _context.Utilisateurs.FirstOrDefault(u => u.Email == loginDto.Email && u.MotDePasse == loginDto.MotDePasse);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });  // Return 401 if login is invalid
            }

            // If login is successful, generate JWT token
            var token = GenerateJwtToken(user);
            return Ok(new { user, token });
        }

        private string GenerateJwtToken(Utilisateur user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtKey"]); // Ensure this is your JWT secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Ensure this is added as a claim
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(ClaimTypes.Role, user.Role) // Optional role claim
        }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expires in 7 days
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        [HttpPut("profile")]
        public IActionResult UpdateUtilisateur([FromBody] UtilisateurUpdateDto utilisateurUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return a 400 Bad Request with validation errors
            }

            // Extract user ID from the User.Claims
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid token or user ID not found in token." });
            }

            // Attempt to parse user ID if needed
            if (!int.TryParse(userId, out int id))
            {
                return Unauthorized(new { message = "User ID in token is invalid." });
            }

            var utilisateur = _context.Utilisateurs.FirstOrDefault(u => u.Id == id);
            if (utilisateur == null)
            {
                return NotFound(new { message = "User not found" }); // Return a 404 Not Found if the user does not exist
            }

            try
            {
                // Update properties (do NOT update the role)
                utilisateur.Nom = utilisateurUpdate.Nom;
                utilisateur.Email = utilisateurUpdate.Email;
                utilisateur.MotDePasse = utilisateurUpdate.MotDePasse;
                utilisateur.Telephone = utilisateurUpdate.Telephone;

                _context.Utilisateurs.Update(utilisateur);
                _context.SaveChanges();
                return Ok(utilisateur); // Return the updated user
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                return StatusCode(500, new { message = "An error occurred while updating the user", error = innerException });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }


    }
}
