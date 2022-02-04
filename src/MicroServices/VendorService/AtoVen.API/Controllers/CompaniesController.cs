#nullable disable
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AtoVen.API.Data;
using AtoVen.API.Entities;
using Microsoft.AspNetCore.Identity;
using AtoVen.API.Entities.UserLoginEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using AtoVen.API.Entities.ValiationResultEntities;
using System.Text.Json;
using EmailSendService;
using ValidationLibrary;

namespace AtoVen.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly AtovenDbContext _context;
        private readonly SchwarzDbContext _SchwarzContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public StringBuilder emailBodyStrBuilder= new StringBuilder();

        public CompaniesController(AtovenDbContext context, IEmailSender emailSender,
                                UserManager<IdentityUser> userManager, 
                                SignInManager<IdentityUser> signInManager,
                                SchwarzDbContext schwarzContext)
        {
            _SchwarzContext = schwarzContext;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // GET: api/Companies
        [HttpGet]
        [ActionName("GetCompanies")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompanies()
        {
            List<CompanyDTO> ListCompanyDTOs = new();

            var ListCompanies = await _context.Companies.ToListAsync();

            foreach (Company company in ListCompanies)
            {
                CompanyDTO companyDTO = new CompanyDTO();

                companyDTO.Id = company.Id;
                companyDTO.CompanyName = company.CompanyName;
                companyDTO.CommercialRegistrationNo = company.CommercialRegistrationNo;
                companyDTO.Language = company.Language;
                companyDTO.Country = company.Country;
                companyDTO.Region = company.Region;
                companyDTO.District = company.District;
                companyDTO.PostalCode = company.PostalCode;
                companyDTO.City = company.City;
                companyDTO.Street = company.Street;
                companyDTO.HouseNo = company.HouseNo;
                companyDTO.Building = company.Building;
                companyDTO.Floor = company.Floor;
                companyDTO.Room = company.Room;
                companyDTO.POBox = company.POBox;
                companyDTO.PhoneNo = company.PhoneNo;
                companyDTO.FaxNumber = company.FaxNumber;
                companyDTO.Email = company.Email;
                companyDTO.MobileNo = company.MobileNo;
                companyDTO.Website = company.Website;
                companyDTO.VendorType = company.VendorType;
                companyDTO.AccountGroup = company.AccountGroup;
                companyDTO.VatNo = company.VatNo;
                companyDTO.Notes = company.Notes;



                List<BankDTO> ListBankDTOs = new();
                var ListBanks = _context.Banks.Where(b => b.CompanyID == companyDTO.Id).ToList();

                foreach (Bank bank in ListBanks)
                {
                    BankDTO bankDTO = new BankDTO();

                    bankDTO.Id = bank.Id;
                    bankDTO.CompanyID = bank.CompanyID;
                    bankDTO.CompanyName = _context.Companies.Find(bank.CompanyID).CompanyName;
                    bankDTO.Country = bank.Country;
                    bankDTO.BankKey = bank.BankKey;
                    bankDTO.BankName = bank.BankName;
                    bankDTO.SwiftCode = bank.SwiftCode;
                    bankDTO.BankAccount = bank.BankAccount;
                    bankDTO.AccountHolderName = bank.AccountHolderName;
                    bankDTO.IBAN = bank.IBAN;
                    bankDTO.Currency = bank.Currency;

                    ListBankDTOs.Add(bankDTO);
                }


                companyDTO.ListOfCompanyBanks = ListBankDTOs;



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

                ListCompanyDTOs.Add(companyDTO);
            }

            return ListCompanyDTOs;
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        [ActionName("GetCompanyById")]
        public async Task<ActionResult<CompanyDTO>> GetCompany(int id)
        {
            Company company = await _context.Companies.FindAsync(id);

            CompanyDTO companyDTO = new CompanyDTO();

            companyDTO.Id = company.Id;
            companyDTO.CompanyName = company.CompanyName;
            companyDTO.CommercialRegistrationNo = company.CommercialRegistrationNo;
            companyDTO.Language = company.Language;
            companyDTO.Country = company.Country;
            companyDTO.Region = company.Region;
            companyDTO.District = company.District;
            companyDTO.PostalCode = company.PostalCode;
            companyDTO.City = company.City;
            companyDTO.Street = company.Street;
            companyDTO.HouseNo = company.HouseNo;
            companyDTO.Building = company.Building;
            companyDTO.Floor = company.Floor;
            companyDTO.Room = company.Room;
            companyDTO.POBox = company.POBox;
            companyDTO.PhoneNo = company.PhoneNo;
            companyDTO.FaxNumber = company.FaxNumber;
            companyDTO.Email = company.Email;
            companyDTO.MobileNo = company.MobileNo;
            companyDTO.Website = company.Website;
            companyDTO.VendorType = company.VendorType;
            companyDTO.AccountGroup = company.AccountGroup;
            companyDTO.VatNo = company.VatNo;
            companyDTO.Notes = company.Notes;

            List<BankDTO> ListBankDTOs = new();
            var ListBanks = _context.Banks.Where(b => b.CompanyID == companyDTO.Id).ToList();

            foreach (Bank bank in ListBanks)
            {
                BankDTO bankDTO = new BankDTO();

                bankDTO.Id = bank.Id;
                bankDTO.CompanyID = bank.CompanyID;
                bankDTO.CompanyName = _context.Companies.Find(bank.CompanyID).CompanyName;
                bankDTO.Country = bank.Country;
                bankDTO.BankKey = bank.BankKey;
                bankDTO.BankName = bank.BankName;
                bankDTO.SwiftCode = bank.SwiftCode;
                bankDTO.BankAccount = bank.BankAccount;
                bankDTO.AccountHolderName = bank.AccountHolderName;
                bankDTO.IBAN = bank.IBAN;
                bankDTO.Currency = bank.Currency;

                ListBankDTOs.Add(bankDTO);

               
            }


            companyDTO.ListOfCompanyBanks = ListBankDTOs;


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

            companyDTO.ListOfCompanyContacts = ListContactDTOs;


            return companyDTO;

        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ActionName("UpdateCompany")]
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

        [HttpPost]
        [ActionName("RegisterCompany")]
        public async Task<ActionResult<Company>> PostCompany(CompanyDTO company)
        {


            ////////////////////////////////////////////////////////////////////
            //// ***************   VAT Validation   *********************///////
            ////////////////////////////////////////////////////////////////////

            VATValidation vatvalidation = new VATValidation();
            if(vatvalidation.ValidateVAT(company.VatNo) != "Valid VAT Number")
            {
                return Ok("Invalid VAT Number");
            }

            emailBodyStrBuilder.AppendLine("VAT Number: Validated");

            ////////////////////////////////////////////////////////////////////
            //// *************** Address Validation *********************///////
            ////////////////////////////////////////////////////////////////////
            AddressValidation addValidation = new AddressValidation();
            AddressValidationInputs addressValidationInputs = new AddressValidationInputs();
            addressValidationInputs.HouseNo = company.HouseNo;
            addressValidationInputs.Street = company.Street;
            addressValidationInputs.City = company.City;
            addressValidationInputs.Region = company.Region;
            addressValidationInputs.Country = company.Country;
            addressValidationInputs.Language = company.Language;
            addressValidationInputs.PostalCode = company.PostalCode;
            

            if (addValidation.ValidateStreetAddress(addressValidationInputs) != "")
            {
                return Ok("Invalid Street Address");
            }
            //
            emailBodyStrBuilder.AppendLine("Street Address: Validated");


            int newCompId = 0;
            int[] arrBankIds;
            int[] arrContactIds;

            int totalBankCount = company.ListOfCompanyBanks.Count;
            int totalContactCount = company.ListOfCompanyContacts.Count;

            int intBankCount = 0;
            int intContactCount = 0;
            Company newCompany;



            using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
            {
                //assign values
                newCompany = new Company();

                newCompany.AccountGroup = company.AccountGroup;
                newCompany.Building = company.Building;
                newCompany.City = company.City;
                newCompany.CommercialRegistrationNo = company.CommercialRegistrationNo;
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

                //Non DTO fields below here
                //newCompany.Password = GenerateRandomPassword(null);
                //newCompany.IsVendorInitiated = true;
                //newCompany.RecordDate = DateTime.Now;

                //newCompany.ApproverID = 1;
                //newCompany.ApproverRoleID = 1;
                //newCompany.IsApproved = false;
                //newCompany.ApprovedDate = null;


                _context.Companies.Add(newCompany);
                await _context.SaveChangesAsync();

                emailBodyStrBuilder.AppendLine("================================================================");
                emailBodyStrBuilder.AppendLine("Vendor Company Details: " + newCompany.ToString());

                //Get the DB Generated Identity Column Value after save.
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                newCompId = newCompany.Id;
                ///>>>>>>>>>>>>>>>>>>>>>>>>>>>

                arrContactIds = new int[totalContactCount]; //Initialize the array with count
                foreach (ContactDTO contact in company.ListOfCompanyContacts)
                {
                    Contact newContact = new Contact() { };

                    newContact.CompanyID = newCompId; //Db generated Identity column value
                    newContact.Email = contact.Email;
                    newContact.FaxNo = contact.FaxNo;
                    newContact.Language = contact.Language;
                    newContact.PhoneNo = contact.PhoneNo;
                    newContact.MobileNo = contact.MobileNo;
                    newContact.Department = contact.Department;
                    newContact.FirstName = contact.FirstName;
                    newContact.Address = contact.Address;
                    newContact.LastName = contact.LastName;
                    newContact.Designation = contact.Designation;
                    newContact.Country = contact.Country;

                    _context.Contacts.Add(newContact);
                    await _context.SaveChangesAsync();


                    emailBodyStrBuilder.AppendLine("================================================================");
                    emailBodyStrBuilder.AppendLine("Vendor Contact-" + intContactCount + 1 + " Details: ");
                    emailBodyStrBuilder.AppendLine(newContact.ToString());
                    arrContactIds[intContactCount] = newContact.Id; //Assign new Contact ID to array
                    intContactCount += 1;
                }

               

                arrBankIds = new int[totalBankCount]; // Initialize the array with count

                foreach (BankDTO bank in company.ListOfCompanyBanks)
                {
                    ////////////////////////////////////////////////////////////////////
                    //// *************** IBAN Number Validation *****************///////
                    ////////////////////////////////////////////////////////////////////
                    IBANValidation ibanvalidation = new IBANValidation();
                    if (ibanvalidation.ValidateIBAN(bank.IBAN) != "Valid IBAN Number")
                    {
                        return Ok("Invalid IBAN Number");
                    }

                    Bank newBank = new Bank() { };

                    newBank.CompanyID = newCompId; //Db generated Identity column value
                    newBank.AccountHolderName = bank.AccountHolderName;
                    newBank.BankAccount = bank.BankAccount;
                    newBank.Country = bank.Country;
                    newBank.BankKey = bank.BankKey;
                    newBank.BankName = bank.BankName;
                    newBank.Currency = bank.Currency;
                    newBank.IBAN = bank.IBAN;
                    newBank.SwiftCode = bank.SwiftCode;

                 

                    _context.Banks.Add(newBank);
                    await _context.SaveChangesAsync();
                    emailBodyStrBuilder.AppendLine("================================================================");
                    emailBodyStrBuilder.AppendLine("Vendor Bank-" + intBankCount + 1 + " Details: ");
                    emailBodyStrBuilder.AppendLine(newBank.ToString());

                    arrBankIds[intBankCount] = newBank.Id; //Assign new bank ID to array
                    intBankCount += 1;
                }

                await AtoVenDbContextTransaction.CommitAsync();
            }

            //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
            ////////////////////////////////////////////////////////////////////
            //// *************** Send Email to Approver *****************///////
            ////////////////////////////////////////////////////////////////////
            /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//

            await SendEmailInHtml("atocash@atominosconsulting.com",
                             "New Vendor " + company.CompanyName +
                             " approval request", emailBodyStrBuilder.ToString()) ;




            ///Register a userId for the Vendor
            ///
            var user = new IdentityUser
            {
                UserName = newCompany.Email,
                Email = newCompany.Email,
                NormalizedUserName = newCompany.CompanyName

            };

            var result = await _userManager.CreateAsync(user, GenerateRandomPassword(null));

            if (result.Succeeded)
            {
                //await _signInManager.SignInAsync(user, isPersistent: false);
            }


            return CreatedAtAction("GetCompany", new { id = newCompId }, company);
        }


        /// <summary>
        /// Generate Random Password satisfying all the Password Requirement chars
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        private static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }


        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        [ActionName("DeleteCompany")]
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


        /// <summary>
        /// Send Email method
        /// </summary>
        /// <param name="sendToEmailAddress"></param>
        /// <param name="emailSubject"></param>
        /// <param name="bodyContent"></param>
        /// <returns></returns>
        private async Task SendEmailInHtml(string sendToEmailAddress, string emailSubject, string bodyContent)
        {
            var approverMailAddress = sendToEmailAddress;
            string subject = emailSubject;
            string content = bodyContent;
            var messagemail = new Message(new string[] { approverMailAddress }, subject, content);
            await _emailSender.SendEmailAsync(messagemail);
        }



    }
}
