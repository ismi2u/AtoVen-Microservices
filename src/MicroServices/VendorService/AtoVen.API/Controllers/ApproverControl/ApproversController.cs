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
    public class ApproversController : ControllerBase
    {
        private readonly AtoVenDbContext _context;

        public ApproversController(AtoVenDbContext context)
        {
            _context = context;
        }

        // GET: api/Approvers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Approver>>> GetApprovers()
        {
            return await _context.Approvers.ToListAsync();
        }

        // GET: api/Approvers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Approver>> GetApprover(int id)
        {
            var approver = await _context.Approvers.FindAsync(id);

            if (approver == null)
            {
                return NotFound();
            }

            return approver;
        }

        // PUT: api/Approvers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprover(int id, Approver approver)
        {
            if (id != approver.Id)
            {
                return BadRequest();
            }

            _context.Entry(approver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApproverExists(id))
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

        // POST: api/Approvers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Approver>> PostApprover(Approver approver)
        {
            _context.Approvers.Add(approver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApprover", new { id = approver.Id }, approver);
        }

        // DELETE: api/Approvers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApprover(int id)
        {
            var approver = await _context.Approvers.FindAsync(id);
            if (approver == null)
            {
                return NotFound();
            }

            _context.Approvers.Remove(approver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApproverExists(int id)
        {
            return _context.Approvers.Any(e => e.Id == id);
        }
    }
}
