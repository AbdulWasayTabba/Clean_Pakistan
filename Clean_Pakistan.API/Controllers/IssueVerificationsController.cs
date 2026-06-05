using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clean_Pakistan.API.Models;
using Clean_Pakistan.API.Data;

[Route("api/[controller]")]
[ApiController]
public class IssueVerificationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public IssueVerificationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/IssueVerification
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IssueVerification>>> GetIssueVerification()
    {
        return await _context.IssueVerifications.ToListAsync();
    }

    // GET: api/IssueVerification/5
    [HttpGet("{id}")]
    public async Task<ActionResult<IssueVerification>> GetIssueVerification(int id)
    {
        var issueverification = await _context.IssueVerifications.FindAsync(id);

        if (issueverification == null)
        {
            return NotFound();
        }

        return issueverification;
    }

    // PUT: api/IssueVerification/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutIssueVerification(int? id, IssueVerification issueverification)
    {
        if (id != issueverification.Id)
        {
            return BadRequest();
        }

        _context.Entry(issueverification).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!IssueVerificationExists(id))
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

    // POST: api/IssueVerification
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<IssueVerification>> PostIssueVerification(IssueVerification issueverification)
    {
        _context.IssueVerifications.Add(issueverification);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetIssueVerification", new { id = issueverification.Id }, issueverification);
    }

    // DELETE: api/IssueVerification/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIssueVerification(int? id)
    {
        var issueverification = await _context.IssueVerifications.FindAsync(id);
        if (issueverification == null)
        {
            return NotFound();
        }

        _context.IssueVerifications.Remove(issueverification);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IssueVerificationExists(int? id)
    {
        return _context.IssueVerifications.Any(e => e.Id == id);
    }
}
