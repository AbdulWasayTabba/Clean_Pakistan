using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clean_Pakistan.API.Models;
using Clean_Pakistan.API.Data;

[Route("api/[controller]")]
[ApiController]
public class CivicIssuesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CivicIssuesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/CivicIssue
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CivicIssue>>> GetCivicIssue()
    {
        return await _context.CivicIssues.ToListAsync();
    }

    // GET: api/CivicIssue/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CivicIssue>> GetCivicIssue(int id)
    {
        var civicissue = await _context.CivicIssues.FindAsync(id);

        if (civicissue == null)
        {
            return NotFound();
        }

        return civicissue;
    }

    // PUT: api/CivicIssue/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCivicIssue(int? id, CivicIssue civicissue)
    {
        if (id != civicissue.Id)
        {
            return BadRequest();
        }

        _context.Entry(civicissue).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CivicIssueExists(id))
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

    // POST: api/CivicIssue
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CivicIssue>> PostCivicIssue(CivicIssue civicissue)
    {
        _context.CivicIssues.Add(civicissue);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCivicIssue", new { id = civicissue.Id }, civicissue);
    }

    // DELETE: api/CivicIssue/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCivicIssue(int? id)
    {
        var civicissue = await _context.CivicIssues.FindAsync(id);
        if (civicissue == null)
        {
            return NotFound();
        }

        _context.CivicIssues.Remove(civicissue);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CivicIssueExists(int? id)
    {
        return _context.CivicIssues.Any(e => e.Id == id);
    }
}
