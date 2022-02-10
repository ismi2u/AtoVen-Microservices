#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataService.Entities;
using DataService.DataContext;
using Microsoft.AspNetCore.Authorization;

namespace AtoVen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, AtoVenAdmin, Approver")]
    public class ContactsController : ControllerBase
    {
        private readonly AtoVenDbContext _context;

        public ContactsController(AtoVenDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContacts()
        {

            List<ContactDTO> ListContactDTOs = new();

            var ListContacts = await _context.Contacts.ToListAsync();

            foreach (Contact contact in ListContacts)
            {
                ContactDTO contactDTO = new ContactDTO();

                contactDTO.Id = contact.Id;
                contactDTO.CompanyID = contact.CompanyID;
                contactDTO.CompanyName = _context.Companies.Find(contact.CompanyID).CompanyName;
                contactDTO.FirstName = contact.FirstName;
                contactDTO.LastName = contact.LastName;
                contactDTO.Address = contact.Address;
                contactDTO.Designation = contact.Designation;
                contactDTO.Department = contact.Department;
                contactDTO.MobileNo = contact.MobileNo;
                contactDTO.FaxNo = contact.FaxNo;
                contactDTO.Email = contact.Email;
                contactDTO.Language = contact.Language;
                contactDTO.Country = contact.Country;

                ListContactDTOs.Add(contactDTO);

            }

            return ListContactDTOs;
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDTO>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            ContactDTO contactDTO = new ContactDTO();

            contactDTO.Id = contact.Id;
            contactDTO.CompanyID = contact.CompanyID;
            contactDTO.CompanyName = _context.Companies.Find(contact.CompanyID).CompanyName;
            contactDTO.FirstName = contact.FirstName;
            contactDTO.LastName = contact.LastName;
            contactDTO.Address = contact.Address;
            contactDTO.Designation = contact.Designation;
            contactDTO.Department = contact.Department;
            contactDTO.MobileNo = contact.MobileNo;
            contactDTO.FaxNo = contact.FaxNo;
            contactDTO.Email = contact.Email;
            contactDTO.Language = contact.Language;
            contactDTO.Country = contact.Country;

            if (contact == null)
            {
                return NotFound();
            }

            return contactDTO;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, ContactDTO contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                var updateContact = await _context.Contacts.FindAsync(contact.Id);

                updateContact.Id = contact.Id;
                updateContact.CompanyID = contact.CompanyID;
                updateContact.FirstName = contact.FirstName;
                updateContact.LastName = contact.LastName;
                updateContact.Address = contact.Address;
                updateContact.Designation = contact.Designation;
                updateContact.Department = contact.Department;
                updateContact.MobileNo = contact.MobileNo;
                updateContact.FaxNo = contact.FaxNo;
                updateContact.Email = contact.Email;
                updateContact.Language = contact.Language;
                updateContact.Country = contact.Country;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(ContactDTO contact)
        {

            Contact newContact = new Contact();

            newContact.CompanyID = contact.CompanyID;
            newContact.FirstName = contact.FirstName;
            newContact.LastName = contact.LastName;
            newContact.Address = contact.Address;
            newContact.Designation = contact.Designation;
            newContact.Department = contact.Department;
            newContact.MobileNo = contact.MobileNo;
            newContact.FaxNo = contact.FaxNo;
            newContact.Email = contact.Email;
            newContact.Language = contact.Language;
            newContact.Country = contact.Country;

            _context.Contacts.Add(newContact);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = newContact.Id }, newContact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
