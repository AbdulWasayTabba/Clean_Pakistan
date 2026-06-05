using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clean_Pakistan.API.Models;
using Clean_Pakistan.API.Data;

[Route("api/[controller]")]
[ApiController]
public class ComplaintsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ComplaintsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Complaint
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Complaint>>> GetComplaint()
    {
        return await _context.Complaints.ToListAsync();
    }

    // GET: api/Complaint/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Complaint>> GetComplaint(int id)
    {
        var complaint = await _context.Complaints.FindAsync(id);

        if (complaint == null)
        {
            return NotFound();
        }

        return complaint;
    }

    // PUT: api/Complaint/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComplaint(int? id, Complaint complaint)
    {
        if (id != complaint.Id)
        {
            return BadRequest();
        }

        _context.Entry(complaint).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ComplaintExists(id))
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

    // POST: api/Complaint
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Complaint>> PostComplaint(Complaint complaint)
    {
        _context.Complaints.Add(complaint);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetComplaint", new { id = complaint.Id }, complaint);
    }

    // DELETE: api/Complaint/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComplaint(int? id)
    {
        var complaint = await _context.Complaints.FindAsync(id);
        if (complaint == null)
        {
            return NotFound();
        }

        _context.Complaints.Remove(complaint);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ComplaintExists(int? id)
    {
        return _context.Complaints.Any(e => e.Id == id);
    }
}
