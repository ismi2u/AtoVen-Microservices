#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoVen.API.Data;
using AtoVen.API.Entities;

namespace AtoVen.API.Controllers.ApproverControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproverLevelsController : ControllerBase
    {
        private readonly AtoVenDbContext _context;

        public ApproverLevelsController(AtoVenDbContext context)
        {
            _context = context;
        }

        // GET: api/ApproverLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApproverLevel>>> GetApproverLevel()
        {
            return await _context.ApproverLevel.ToListAsync();
        }

        // GET: api/ApproverLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApproverLevel>> GetApproverLevel(int id)
        {
            var approverLevel = await _context.ApproverLevel.FindAsync(id);

            if (approverLevel == null)
            {
                return NotFound();
            }

            return approverLevel;
        }

        // PUT: api/ApproverLevels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApproverLevel(int id, ApproverLevel approverLevel)
        {
            if (id != approverLevel.Id)
            {
                return BadRequest();
            }

            _context.Entry(approverLevel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApproverLevelExists(id))
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

        // POST: api/ApproverLevels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApproverLevel>> PostApproverLevel(ApproverLevel approverLevel)
        {
            _context.ApproverLevel.Add(approverLevel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApproverLevel", new { id = approverLevel.Id }, approverLevel);
        }

        // DELETE: api/ApproverLevels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApproverLevel(int id)
        {
            var approverLevel = await _context.ApproverLevel.FindAsync(id);
            if (approverLevel == null)
            {
                return NotFound();
            }

            _context.ApproverLevel.Remove(approverLevel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApproverLevelExists(int id)
        {
            return _context.ApproverLevel.Any(e => e.Id == id);
        }
    }
}
