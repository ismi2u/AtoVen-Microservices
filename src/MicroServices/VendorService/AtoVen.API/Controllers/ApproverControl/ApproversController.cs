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
        public async Task<ActionResult<IEnumerable<ApproverDTO>>> GetApprovers()
        {

            List<ApproverDTO> ListApproverDTO = new();

            var ListApprovers = await _context.Approvers.ToListAsync();

            foreach(Approver approver in ListApprovers)
            {
                ApproverDTO approverDTO = new ApproverDTO();

                approverDTO.Id = approver.Id;
                approverDTO.Name = approver.Name;
                approverDTO.Email = approver.Email;
                approverDTO.ApproverRoleID = approver.ApproverRoleID;
                approverDTO.ApproverRole = _context.ApproverRoles.Find(approver.ApproverRoleID).RoleName;
                approverDTO.ApproverLevelID = approver.ApproverLevelID;
                approverDTO.ApproverLevel = _context.ApproverLevels.Find(approver.ApproverLevelID).Level;
                approverDTO.IsEnabled = approver.IsEnabled;

                ListApproverDTO.Add(approverDTO);
            }

            return ListApproverDTO;
        }

        // GET: api/Approvers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApproverDTO>> GetApprover(int id)
        {
            Approver approver = await _context.Approvers.FindAsync(id);
            if (approver == null)
            {
                return NotFound();
            }

            ApproverDTO approverDTO = new ApproverDTO();

            approverDTO.Id = approver.Id;
            approverDTO.Name = approver.Name;
            approverDTO.Email = approver.Email;
            approverDTO.ApproverRoleID = approver.ApproverRoleID;
            approverDTO.ApproverRole= _context.ApproverRoles.Find(approver.ApproverRoleID).RoleName;
            approverDTO.ApproverLevelID = approver.ApproverLevelID;
            approverDTO.ApproverLevel= _context.ApproverLevels.Find(approver.ApproverLevelID).Level;
            approverDTO.IsEnabled = approver.IsEnabled;

            return approverDTO;
        }

        // PUT: api/Approvers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprover(int id, ApproverDTO approver)
        {
            if (id != approver.Id)
            {
                return BadRequest();
            }

            _context.Entry(approver).State = EntityState.Modified;

            try
            {
                var updateApprover = await _context.Approvers.FindAsync(approver.Id);

                updateApprover.Id = approver.Id;
                updateApprover.Name = approver.Name;
                updateApprover.Email = approver.Email;
                updateApprover.ApproverRoleID = approver.ApproverRoleID;
                updateApprover.ApproverLevelID = approver.ApproverLevelID;
                updateApprover.IsEnabled = approver.IsEnabled;


                _context.Approvers.Update(updateApprover);
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
        public async Task<ActionResult<Approver>> PostApprover(ApproverDTO approver)
        {
            Approver newApprover = new Approver();
            newApprover.Name = approver.Name;
            newApprover.Email = approver.Email;
            newApprover.ApproverRoleID = approver.ApproverRoleID;
            newApprover.ApproverLevelID = approver.ApproverLevelID;
            newApprover.IsEnabled = approver.IsEnabled;

        _context.Approvers.Add(newApprover);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApprover", new { id = newApprover.Id }, newApprover);
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
