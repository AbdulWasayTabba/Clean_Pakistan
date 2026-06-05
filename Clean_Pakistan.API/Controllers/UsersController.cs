using Clean_Pakistan.API;
using Clean_Pakistan.API.Data;
using Clean_Pakistan.API.Models;
using Clean_Pakistan.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly EmailService _emailService;
    private readonly IConfiguration _config;
    public UsersController(ApplicationDbContext context, EmailService emailService,IConfiguration config)
    {
        _context = context;
        _emailService = emailService;
        _config = config;
}

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUser()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // PUT: api/User/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int? id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
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

    // POST: api/User
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int? id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int? id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
    // --- THE ENDPOINTS ---

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest("Email already exists.");

        // Securely hash the password!
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Generate a secure 6-digit OTP
        string otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var user = new User
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            PasswordHash = hashedPassword,
            Otp = otp,
            OtpExpiry = DateTime.UtcNow.AddMinutes(5),
            IsVerified = false // They can't login until they prove they own the email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Trigger your EmailService here!
        await _emailService.SendOtpEmail(user.Email, otp);

        return Ok(new { message = "Account created. Please check your email for the OTP." });
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyOtpRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null) return NotFound("User not found.");
        if (user.Otp != request.Otp) return BadRequest("Invalid OTP.");
        if (user.OtpExpiry < DateTime.UtcNow) return BadRequest("OTP has expired.");

        // Success! Lock in the verification
        user.IsVerified = true;
        user.Otp = null;
        user.OtpExpiry = null;
        await _context.SaveChangesAsync();

        // Generate the JWT so they are instantly logged in after verifying
        var token = GenerateJwtToken(user);
        return Ok(new { message = "Email verified successfully!", token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null) return NotFound("Invalid email or password.");
        if (!user.IsVerified) return BadRequest("Please verify your email first.");

        // Check if the password matches the hash
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return BadRequest("Invalid email or password.");

        var token = GenerateJwtToken(user);
        return Ok(new { message = "Login successful!", token = token });
    }

    // --- HELPER METHOD TO GENERATE TOKEN ---
    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };
        var token = new JwtSecurityToken(issuer: "CleanPakistanAPI", audience: "CleanPakistanWebAndMobile", claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
public class RegisterRequest { public string FullName { get; set; } public string PhoneNumber { get; set; } public string Email { get; set; } public string Password { get; set; } }
public class LoginRequest { public string Email { get; set; } public string Password { get; set; } }
public class VerifyOtpRequest { public string Email { get; set; } public string Otp { get; set; } }