﻿#nullable disable
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
        private readonly AtoVenDbContext _context;

        public ApproverRolesController(AtoVenDbContext context)
        {
            _context = context;
        }

        // GET: api/ApproverRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApproverRole>>> GetApproverRole()
        {
            return await _context.ApproverRole.ToListAsync();
        }

        // GET: api/ApproverRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApproverRole>> GetApproverRole(int id)
        {
            var approverRole = await _context.ApproverRole.FindAsync(id);

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
        public async Task<ActionResult<ApproverRole>> PostApproverRole(ApproverRole approverRole)
        {
            _context.ApproverRole.Add(approverRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApproverRole", new { id = approverRole.Id }, approverRole);
        }

        // DELETE: api/ApproverRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApproverRole(int id)
        {
            var approverRole = await _context.ApproverRole.FindAsync(id);
            if (approverRole == null)
            {
                return NotFound();
            }

            _context.ApproverRole.Remove(approverRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApproverRoleExists(int id)
        {
            return _context.ApproverRole.Any(e => e.Id == id);
        }
    }
}