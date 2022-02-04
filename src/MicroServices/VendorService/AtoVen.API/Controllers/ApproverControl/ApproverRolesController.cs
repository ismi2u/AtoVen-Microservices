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
    public class ApproverRolesController : ControllerBase
    {
        private readonly AtovenDbContext _context;

        public ApproverRolesController(AtovenDbContext context)
        {
            _context = context;
        }

        // GET: api/ApproverRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApproverRole>>> GetApproverRole()
        {
            return await _context.ApproverRoles.ToListAsync();
        }

        // GET: api/ApproverRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApproverRole>> GetApproverRole(int id)
        {
            var approverRole = await _context.ApproverRoles.FindAsync(id);

            if (approverRole == null)
            {
                return NotFound();
            }

            return approverRole;
        }

        // PUT: api/ApproverRoles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApproverRole(int id, ApproverRole approverRole)
        {
            if (id != approverRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(approverRole).State = EntityState.Modified;

            try
            {
                var updateApproverRole = await _context.ApproverRoles.FindAsync(approverRole.Id);

                updateApproverRole.Id = approverRole.Id;
                updateApproverRole.RoleName = approverRole.RoleName;
                updateApproverRole.IsEnabled = approverRole.IsEnabled;

                _context.ApproverRoles.Update(updateApproverRole);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApproverRoleExists(id))
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

        // POST: api/ApproverRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApproverRole>> PostApproverRole(ApproverRoleDTO approverRole)
        {
            ApproverRole newApproverRole = new ApproverRole();
            newApproverRole.RoleName = approverRole.RoleName;
            newApproverRole.IsEnabled = approverRole.IsEnabled;

            _context.ApproverRoles.Add(newApproverRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApproverRole", new { id = newApproverRole.Id }, newApproverRole);
        }

        // DELETE: api/ApproverRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApproverRole(int id)
        {
            var approverRole = await _context.ApproverRoles.FindAsync(id);
            if (approverRole == null)
            {
                return NotFound();
            }

            _context.ApproverRoles.Remove(approverRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApproverRoleExists(int id)
        {
            return _context.ApproverRoles.Any(e => e.Id == id);
        }
    }
}
