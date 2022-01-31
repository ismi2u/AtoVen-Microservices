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
//using EmailSender;

namespace AtoVen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly AtoVenDbContext _context;
        //private readonly IEmailSender _emailSender;

        //public CompaniesController(AtoVenDbContext context, IEmailSender emailSender)
        public CompaniesController(AtoVenDbContext context)
        {
            _context = context;
            //_emailSender = emailSender;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(CompanyDTO company)
        {
            int newCompId = 0;

            using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
            {
                //assign values
                Company newCompany = new Company();

                newCompany.AccountGroup = company.AccountGroup;
                newCompany.Building = company.Building;
                newCompany.City = company.City;
                newCompany.CommResidenceNo = company.CommResidenceNo;
                newCompany.CompanyName = company.CompanyName;
                newCompany.Country = company.Country;
                newCompany.District = company.District;
                newCompany.Email = company.Email;
                newCompany.FaxNumber = company.FaxNumber;
                newCompany.Floor = company.Floor;
                newCompany.HouseNo = company.HouseNo;
                newCompany.Language = company.Language;
                newCompany.MobileNo = company.MobileNo;
                newCompany.Notes = company.Notes;
                newCompany.PhoneNo = company.PhoneNo;
                newCompany.POBox = company.POBox;
                newCompany.PostalCode = company.PostalCode;
                newCompany.Region = company.Region;
                newCompany.Room = company.Room;
                newCompany.Street = company.Street;
                newCompany.VatNo = company.VatNo;
                newCompany.VendorType = company.VendorType;
                newCompany.Website = company.Website;

                _context.Companies.Add(newCompany);
                await _context.SaveChangesAsync();

                //Get the DB Generated Identity Column Value after save.
                //////////////////////////////////////////////////

                newCompId = newCompany.Id;

                //////////////////////////////////////////////////


                foreach (ContactDTO contact in company.ListOfCompanyContacts)
                {
                    Contact newContact = new Contact() { };

                    newContact.CompanyID = newCompId; //Db generated Identity column value
                    newContact.Email = contact.Email;
                    newContact.FaxNo = contact.FaxNo;
                    newContact.Language = contact.Language;
                    newContact.PhoneNo = contact.PhoneNo;
                    newContact.Department = contact.Department;
                    newContact.FirstName = contact.FirstName;
                    newContact.FormofAddress = contact.FormofAddress;
                    newContact.LastName = contact.LastName;
                    newContact.Position = contact.Position;
                    newContact.Country = contact.Country;

                    _context.Contacts.Add(newContact);
                    await _context.SaveChangesAsync();
                }

                foreach (BankDTO bank in company.ListOfCompanyBanks)
                {
                    Bank newBank = new Bank() { };

                    newBank.CompanyID = newCompId; //Db generated Identity column value
                    newBank.AccountHolder = bank.AccountHolder;
                    newBank.BankAccount = bank.BankAccount;
                    newBank.Country = bank.Country;
                    newBank.BankKey = bank.BankKey;
                    newBank.BankName = bank.BankName;
                    newBank.Currency = bank.Currency;
                    newBank.IBAN = bank.IBAN;
                    newBank.SwiftCode = bank.SwiftCode;

                    _context.Banks.Add(newBank);
                    await _context.SaveChangesAsync();
                }

                await AtoVenDbContextTransaction.CommitAsync();
            }


            return CreatedAtAction("GetCompany", new { id = newCompId }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }

        //private async Task SendEmailInHtml()
        //{
        //    var approverMailAddress = "manager@gmail.com";
        //    string subject = "New Vendor Approval Request";
        //    string content = "Vendor Approval Request : Request Date " + DateTime.Now;
        //    var messagemail = new Message(new string[] { approverMailAddress }, subject, content);
        //    await _emailSender.SendEmailAsync(messagemail);
        //}
    }
}
