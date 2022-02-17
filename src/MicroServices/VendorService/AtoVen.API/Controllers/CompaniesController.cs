#nullable disable
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting;

using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using EmailSendService;
using ValidationLibrary;
using DataService.Entities;
using DataService.DataContext;
using DataService.AccountControl.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.StaticFiles;
using System.Transactions;

namespace AtoVen.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin, AtoVenAdmin, Approver, Vendor")]
    public class CompaniesController : ControllerBase
    {
        private readonly AtoVenDbContext _context;
        private readonly SchwarzDbContext _schwarzContext;
        private readonly ILogger<CompaniesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment hostingEnvironment;

        public StringBuilder emailBodyBuilder = new StringBuilder();

        public CompaniesController(AtoVenDbContext context, IEmailSender emailSender,
                                UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                SchwarzDbContext schwarzContext,
                                IWebHostEnvironment hostEnv,
                                RoleManager<IdentityRole> roleManager,
                                ILogger<CompaniesController> logger)
        {
            _schwarzContext = schwarzContext;
            _logger = logger;
            _context = context;
            hostingEnvironment = hostEnv;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/Companies
        [HttpGet]
        [ActionName("GetCompanies")]
        //[Authorize(Roles = "Admin, AtoVenAdmin, Approver")]
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


                companyDTO.IsVendorInitiated = company.IsVendorInitiated;
                companyDTO.RecordDate = company.RecordDate;
                companyDTO.IsApproved = company.IsApproved;
                companyDTO.ApprovedDate = company.ApprovedDate ?? null;



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

                var ListContacts = _context.Contacts.Where(c => c.CompanyID == companyDTO.Id).ToList();

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
                    contactDTO.PhoneNo = contact.PhoneNo;
                    contactDTO.FaxNo = contact.FaxNo;
                    contactDTO.Email = contact.Email;
                    contactDTO.Language = contact.Language;
                    contactDTO.Country = contact.Country;

                    ListContactDTOs.Add(contactDTO);

                }

                companyDTO.ListOfCompanyContacts = ListContactDTOs;

                ListCompanyDTOs.Add(companyDTO);
            }

            _logger.LogInformation("Company Added: " + DateTime.Now);

            return Ok(ListCompanyDTOs.OrderByDescending(o => o.RecordDate).ToList());

        }


        [HttpGet]
        [ActionName("GetCompaniesApproved")]
        //[Authorize(Roles = "Admin, AtoVenAdmin, Approver")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompaniesApproved()
        {
            List<CompanyDTO> ListCompanyDTOs = new();

            var ListCompanies = await _context.Companies.Where(c => c.IsApproved == true).ToListAsync();

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


                companyDTO.IsVendorInitiated = company.IsVendorInitiated;
                companyDTO.RecordDate = company.RecordDate;
                companyDTO.IsApproved = company.IsApproved;
                companyDTO.ApprovedDate = company.ApprovedDate ?? null;



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

                var ListContacts = _context.Contacts.Where(c => c.CompanyID == companyDTO.Id).ToList();

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
                    contactDTO.PhoneNo = contact.PhoneNo;
                    contactDTO.FaxNo = contact.FaxNo;
                    contactDTO.Email = contact.Email;
                    contactDTO.Language = contact.Language;
                    contactDTO.Country = contact.Country;

                    ListContactDTOs.Add(contactDTO);

                }

                companyDTO.ListOfCompanyContacts = ListContactDTOs;

                ListCompanyDTOs.Add(companyDTO);
            }

            _logger.LogInformation("Company Added: " + DateTime.Now);

            return Ok(ListCompanyDTOs.OrderByDescending(o => o.RecordDate).ToList());

        }


        // GET: api/Companies/5
        [HttpGet("{id}")]
        [ActionName("GetCompanyById")]
        //[Authorize(Roles = "Admin, AtoVenAdmin, Approver, Vendor")]
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


            companyDTO.IsVendorInitiated = company.IsVendorInitiated;
            companyDTO.RecordDate = company.RecordDate;
            companyDTO.IsApproved = company.IsApproved;
            companyDTO.ApprovedDate = company.ApprovedDate ?? null;

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
            var ListContacts = _context.Contacts.Where(b => b.CompanyID == companyDTO.Id).ToList();

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
                contactDTO.PhoneNo = contact.PhoneNo;
                contactDTO.FaxNo = contact.FaxNo;
                contactDTO.Email = contact.Email;
                contactDTO.Language = contact.Language;
                contactDTO.Country = contact.Country;

                ListContactDTOs.Add(contactDTO);

            }

            companyDTO.ListOfCompanyContacts = ListContactDTOs;

            return Ok(companyDTO);
        }


        [HttpGet("{id}")]
        [ActionName("GetCompanyDuplicatesByCompId")]
        public async Task<IEnumerable<CompanyDTO>> GetCompanyDuplicatesByCompId(int id)
        {
            List<CompanyDTO> ListCompanyDTOs = new();

            Company checkCompany = await _context.Companies.FindAsync(id);

            DuplicatesValidation duplicate = new DuplicatesValidation(_context, _schwarzContext);
            var ListDuplicateCompanies = await duplicate.CheckDuplicates(checkCompany);

            foreach (Company company in ListDuplicateCompanies)
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


                companyDTO.IsVendorInitiated = company.IsVendorInitiated;
                companyDTO.RecordDate = company.RecordDate;
                companyDTO.IsApproved = company.IsApproved;
                companyDTO.ApprovedDate = company.ApprovedDate ?? null;



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

                var ListContacts = _context.Contacts.Where(c => c.CompanyID == companyDTO.Id).ToList();

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
                    contactDTO.PhoneNo = contact.PhoneNo;
                    contactDTO.FaxNo = contact.FaxNo;
                    contactDTO.Email = contact.Email;
                    contactDTO.Language = contact.Language;
                    contactDTO.Country = contact.Country;

                    ListContactDTOs.Add(contactDTO);

                }

                companyDTO.ListOfCompanyContacts = ListContactDTOs;

                ListCompanyDTOs.Add(companyDTO);

            }

            ListCompanyDTOs = ListCompanyDTOs.Distinct().ToList();
            return ListCompanyDTOs.OrderByDescending(o => o.RecordDate).ToList();
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ActionName("UpdateCompany")]
        //[Authorize(Roles = "Admin, AtoVenAdmin, Approver, Vendor")]
        public async Task<IActionResult> PutCompany(int id, CompanyPutDTO companyPutDTO)
        {

            DateTime curDateTime = DateTime.Now;

            emailBodyBuilder.AppendLine("===========================================================");
            emailBodyBuilder.AppendLine("Update Existing Vendor " + companyPutDTO.CompanyName + " Approval request");
            emailBodyBuilder.AppendLine("===========================================================");
            if (id != companyPutDTO.Id)
            {
                return Ok(new { Status = "Failure", Message = "Company Id Invalid!" });
            }

            ////////////////// UPDATE EXISTING COMPANY ///////////////////////
            ////////////////////////////////////////////////////////////////////
            //// ***************   VAT Validation   *********************///////
            ////////////////////////////////////////////////////////////////////

            try
            {

                VATValidation vatvalidation = new VATValidation();
                if (vatvalidation.ValidateVAT(companyPutDTO.VatNo) != "Valid VAT Number")
                {

                    return Ok(new { Status = "Failure", Message = "Invalid VAT Number: " + companyPutDTO.VatNo });
                }

                emailBodyBuilder.AppendLine("VAT Number: Validated");

                ////////////////// UPDATE TO EXISTING RECORD ///////////////////////
                ////////////////////////////////////////////////////////////////////
                //// *************** Address Validation *********************///////
                ////////////////////////////////////////////////////////////////////
                //AddressValidation addValidation = new AddressValidation();
                //AddressValidationInputs addressValidationInputs = new AddressValidationInputs();
                //addressValidationInputs.HouseNo = companyPutDTO.HouseNo;
                //addressValidationInputs.Street = companyPutDTO.Street;
                //addressValidationInputs.City = companyPutDTO.City;
                //addressValidationInputs.Region = companyPutDTO.Region;
                //addressValidationInputs.Country = companyPutDTO.Country;
                //addressValidationInputs.Language = companyPutDTO.Language;
                //addressValidationInputs.PostalCode = companyPutDTO.PostalCode;

                //if (addValidation.ValidateStreetAddress(addressValidationInputs) != "")
                //{
                //    return Ok(new { Status = "Failure", Message = "Invalid Street Address" });
                //}
                //
                //emailBodyBuilder.AppendLine("Street Address: Validated");


                int updateCompId = 0;
                //int[] arrBankIds;
                //int[] arrContactIds;

                //int totalBankCount = companyPutDTO.ListOfCompanyBanks.Count;
                //int totalContactCount = companyPutDTO.ListOfCompanyContacts.Count;

                //int intBankCount = 0;
                //int intContactCount = 0;
                Company updateCompany;
                string newCompanyPassword = "";


                //XXX Check if this is already approved record
                //if already approved then update the company, Bank and Contacts record
                //Create approval flow and send email to approval flow

                Company currentCompanyDetail = await _context.Companies.FindAsync(companyPutDTO.Id);
                bool isApprovalFlowAvailable = false;
                bool isCompanyAlreadyApproved = false;
                List<ApprovalFlow> ListApprovalFlowForCompany = _context.ApprovalFlows.Where(a => a.CompanyID == companyPutDTO.Id).ToList();
                if (ListApprovalFlowForCompany.Count > 0)
                {
                    isApprovalFlowAvailable = true;
                }
                if (currentCompanyDetail.IsApproved)
                {
                    isCompanyAlreadyApproved = true;
                }

                using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
                {
                    //assign values
                    updateCompany = await _context.Companies.FindAsync(companyPutDTO.Id);

                    updateCompany.AccountGroup = companyPutDTO.AccountGroup;
                    updateCompany.Building = companyPutDTO.Building;
                    updateCompany.City = companyPutDTO.City;
                    updateCompany.CommercialRegistrationNo = companyPutDTO.CommercialRegistrationNo;
                    updateCompany.CompanyName = companyPutDTO.CompanyName;
                    updateCompany.Country = companyPutDTO.Country;
                    updateCompany.District = companyPutDTO.District;
                    updateCompany.FaxNumber = companyPutDTO.FaxNumber;
                    updateCompany.Floor = companyPutDTO.Floor;
                    updateCompany.HouseNo = companyPutDTO.HouseNo;
                    updateCompany.Language = companyPutDTO.Language;
                    updateCompany.MobileNo = companyPutDTO.MobileNo;
                    updateCompany.Notes = companyPutDTO.Notes;
                    updateCompany.PhoneNo = companyPutDTO.PhoneNo;
                    updateCompany.POBox = companyPutDTO.POBox;
                    updateCompany.PostalCode = companyPutDTO.PostalCode;
                    updateCompany.Region = companyPutDTO.Region;
                    updateCompany.Room = companyPutDTO.Room;
                    updateCompany.Street = companyPutDTO.Street;
                    updateCompany.VatNo = companyPutDTO.VatNo;
                    updateCompany.VendorType = companyPutDTO.VendorType;
                    updateCompany.Website = companyPutDTO.Website;

                    updateCompany.IsVendorInitiated = companyPutDTO.IsVendorInitiated ?? false;
                    updateCompany.RecordDate = curDateTime;
                    updateCompany.IsApproved = currentCompanyDetail.IsApproved ? currentCompanyDetail.IsApproved : false;//
                    updateCompany.ApprovedDate = currentCompanyDetail.IsApproved ? currentCompanyDetail.ApprovedDate : null;

                    _context.Companies.Update(updateCompany);
                    //await _context.SaveChangesAsync();

                    emailBodyBuilder.AppendLine("==================================================================================================");
                    emailBodyBuilder.AppendLine("Vendor Company Details: " + updateCompany.CompanyName);
                    emailBodyBuilder.AppendLine("                        " + updateCompany.City + ", " + updateCompany.Country + ", " + updateCompany.PostalCode);
                    emailBodyBuilder.AppendLine("                        " + updateCompany.MobileNo + ", " + updateCompany.PhoneNo);
                    emailBodyBuilder.AppendLine("Registration No: " + updateCompany.CommercialRegistrationNo);
                    emailBodyBuilder.AppendLine("==================================================================================================");

                    //Get the DB Generated Identity Column Value after save.
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    updateCompId = updateCompany.Id;
                    ///>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    //remove all existing Contacts related to the company
                    _context.Contacts.RemoveRange(_context.Contacts.Where(c => c.CompanyID == updateCompId));
                    //await _context.SaveChangesAsync();
                    foreach (ContactPutDTO contact in companyPutDTO.ListOfCompanyContacts)
                    {
                        Contact newContact = new();

                        newContact.CompanyID = updateCompId; //Db generated Identity column value
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
                        //await _context.SaveChangesAsync();


                        emailBodyBuilder.AppendLine("================================================================");
                        emailBodyBuilder.AppendLine("Vendor Contact Details: ");
                        //emailBodyBuilder.AppendLine(JsonConvert.SerializeObject(newContact));

                    }

                    //remove all existing Contacts related to the company
                    _context.Banks.RemoveRange(_context.Banks.Where(c => c.CompanyID == updateCompId));
                    //await _context.SaveChangesAsync();
                    foreach (BankPutDTO bank in companyPutDTO.ListOfCompanyBanks)
                    {   ////////////////// UPDATE TO EXISTING RECORD ///////////////////////
                        ////////////////////////////////////////////////////////////////////
                        //// *************** IBAN Number Validation *****************///////
                        ////////////////////////////////////////////////////////////////////
                        IBANValidation ibanvalidation = new IBANValidation();
                        if (ibanvalidation.ValidateIBAN(bank.IBAN) != "Valid IBAN Number")
                        {
                            return Ok(new { Status = "Failure", Message = "Invalid IBAN Number: " + bank.IBAN });
                        }

                        Bank newBank = new Bank();

                        newBank.CompanyID = updateCompId; //Db generated Identity column value
                        newBank.AccountHolderName = bank.AccountHolderName;
                        newBank.BankAccount = bank.BankAccount;
                        newBank.Country = bank.Country;
                        newBank.BankKey = bank.BankKey;
                        newBank.BankName = bank.BankName;
                        newBank.Currency = bank.Currency;
                        newBank.IBAN = bank.IBAN;
                        newBank.SwiftCode = bank.SwiftCode;


                        await _context.Banks.AddAsync(newBank);

                        emailBodyBuilder.AppendLine("================================================================");
                        emailBodyBuilder.AppendLine("Vendor Bank Details: ");
                        //emailBodyBuilder.AppendLine(JsonConvert.SerializeObject(newBank));

                    }
                    //await _context.SaveChangesAsync();
                    await AtoVenDbContextTransaction.CommitAsync();
                }



                //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                ////////////////////////////////////////////////////////////////////
                //// ***************   All Duplications Check FLOW   ********///////
                ////////////////////////////////////////////////////////////////////
                /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//

                DuplicatesValidation duplicate = new DuplicatesValidation(_context, _schwarzContext);
                bool areDuplicatesFound = await duplicate.IsDuplicate(updateCompany);

                emailBodyBuilder.AppendLine("///////////////////////////////////////////");

                List<ApprovalFlow> listofApprovalFlows = _context.ApprovalFlows.Where(a => a.CompanyID == updateCompany.Id).ToList();

                //check Logged in User Role
                var uName = User.Identity.Name;
                ApplicationUser applicationUser = await _userManager.FindByNameAsync(uName);

                bool isVendorRole = false;
                var Roles = await _userManager.GetRolesAsync(applicationUser);

                foreach (string role in Roles)
                {
                    if (role == "Vendor")
                    {
                        isVendorRole = true;
                        break;
                    }
                }



                //only If the updating record is already approved and NO APPROVAL FLOW is found!
                if (!isApprovalFlowAvailable && isCompanyAlreadyApproved)
                {
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                    ////////////////////////////////////////////////////////////////////
                    //// ***************    Add Approval FLOW   *****************///////
                    ////////////////////////////////////////////////////////////////////
                    /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//

                    //find out the no of levels of approvals max = 2
                    int maxApprovalLevel = _context.Users.Max(u => u.ApproverLevel);
                    //approval Leverl of the approver
                    int currentApproverLevel = applicationUser.ApproverLevel;


                    for (int level = 1; level <= maxApprovalLevel; level++)
                    {
                        List<ApplicationUser> listApprovers = _context.Users.Where(u => u.ApproverLevel == level).ToList();

                        foreach (ApplicationUser approver in listApprovers)
                        {
                            ApprovalFlow newApprovalFlow = new ApprovalFlow();

                            newApprovalFlow.CompanyID = companyPutDTO.Id;
                            newApprovalFlow.RecordDate = curDateTime;
                            newApprovalFlow.ApproverEmail = approver.Email;
                            newApprovalFlow.ApproverLevel = approver.ApproverLevel;
                            newApprovalFlow.ApprovalStatus = currentApproverLevel >= approver.ApproverLevel ? (int)ApprovalStatusType.Approved : (int)ApprovalStatusType.Pending;

                            if (currentApproverLevel >= approver.ApproverLevel)
                            {
                                newApprovalFlow.LevelApprovedDate = curDateTime;

                            }
                            newApprovalFlow.IsDuplicateEntry = areDuplicatesFound;

                            _context.ApprovalFlows.Add(newApprovalFlow);
                            await _context.SaveChangesAsync();


                            if (level == currentApproverLevel)
                            {
                                companyPutDTO.ApprovalFlowID = newApprovalFlow.Id;
                                listofApprovalFlows = _context.ApprovalFlows.Where(a => a.CompanyID == updateCompany.Id).ToList();
                            }


                            //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                            ////////////////////////////////////////////////////////////////////
                            //// *************** Send Email to Approver *****************///////
                            ////////////////////////////////////////////////////////////////////
                            /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                            ///
                            if (level == 1 && currentApproverLevel <= 1) //only first approver will receive email
                            {
                                await SendEmailInHtml(
                                    approver.Email,
                                    "New Vendor " + companyPutDTO.CompanyName + " Approval request",
                                    emailBodyBuilder.ToString());
                            }
                            ////////////////////////////////////////////////////////////////////
                            /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                        }



                    }
                }


                //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                ////////////////////////////////////////////////////////////////////
                //// ***************    Update Approval FLOW   ***************///////
                ////////////////////////////////////////////////////////////////////
                /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//




                int i = 1;
                foreach (ApprovalFlow approvalFlowItem in listofApprovalFlows)
                {
                    ApprovalFlow updateApprovalFlow = await _context.ApprovalFlows.FindAsync(approvalFlowItem.Id);

                    updateApprovalFlow.RecordDate = curDateTime;
                    updateApprovalFlow.IsDuplicateEntry = areDuplicatesFound;

                    if (isVendorRole) //vendor is updating the record
                    {
                        updateApprovalFlow.ApprovalStatus = (int)ApprovalStatusType.Pending; //action is by Vendor
                        updateApprovalFlow.LevelApprovedDate = null;
                        _context.ApprovalFlows.Update(updateApprovalFlow);
                        await _context.SaveChangesAsync();

                        //Send email to the FIRST Approver
                        if (i == 1)
                        {
                            ApprovalFlow firstApproval = _context.ApprovalFlows.Where(a => a.CompanyID == updateCompId && a.ApproverLevel == 1).FirstOrDefault();
                            var approverMailAddress = firstApproval.ApproverEmail;
                            string subject = "Vendor Details Updated by " + updateCompany.CompanyName;
                            string content = emailBodyBuilder.ToString();
                            var messagemail = new Message(new string[] { approverMailAddress }, subject, content);
                            await _emailSender.SendEmailAsync(messagemail);
                        }

                        i += 1;
                    }
                    else //APPROVER IS updating the record
                    {
                        if (approvalFlowItem.Id == companyPutDTO.ApprovalFlowID)
                        {
                            updateApprovalFlow.ApprovalStatus = (int)ApprovalStatusType.Approved;//action is by Approver
                            updateApprovalFlow.LevelApprovedDate = curDateTime;
                            _context.ApprovalFlows.Update(updateApprovalFlow);
                            await _context.SaveChangesAsync();

                            int compId = updateApprovalFlow.CompanyID;
                            int apprLevel = updateApprovalFlow.ApproverLevel;

                            int nxtApprLevel = apprLevel + 1;
                            Company company = await _context.Companies.FindAsync(compId);
                            List<Contact> contacts = await _context.Contacts.Where(c => c.CompanyID == compId).ToListAsync();
                            List<Bank> banks = await _context.Banks.Where(b => b.CompanyID == compId).ToListAsync();

                            ApprovalFlow nextApproval = _context.ApprovalFlows.Where(a => a.CompanyID == compId && a.ApproverLevel == nxtApprLevel && a.ApprovalStatus == (int)ApprovalStatusType.Pending).FirstOrDefault();


                            if (nextApproval != null)
                            {
                                //If next approver is available Send email 
                                emailBodyBuilder = new StringBuilder();

                                emailBodyBuilder.AppendLine("New Vendor Registration");
                                emailBodyBuilder.AppendLine("===========================");
                                emailBodyBuilder.AppendLine("Company Name: " + company.CompanyName);
                                emailBodyBuilder.AppendLine("Company Registration No:" + company.CommercialRegistrationNo);
                                emailBodyBuilder.AppendLine("Company Location:" + company.City + " " + company.Country);
                                emailBodyBuilder.AppendLine("New Vendor Registration");

                                var approverMailAddress = nextApproval.ApproverEmail;
                                string subject = "New Vendor Registration" + company.CompanyName;
                                string content = emailBodyBuilder.ToString();
                                var messagemail = new Message(new string[] { approverMailAddress }, subject, content);
                                await _emailSender.SendEmailAsync(messagemail);

                            }
                            else
                            {


                                //else => THIS IS FINAL APPROVER
                                // 1. change the status as approved in Local DB
                                // 2. create and Save records to SchwarzDB
                                // 3. Send UserID and Password to Vendor
                                //using (TransactionScope scope = new TransactionScope())
                                //{
                                using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
                                {
                                    //assign values
                                    updateCompany = await _context.Companies.FindAsync(compId);

                                    updateCompany.IsApproved = true;
                                    updateCompany.ApprovedDate = curDateTime;

                                    _context.Companies.Update(updateCompany);
                                    await _context.SaveChangesAsync();
                                    await AtoVenDbContextTransaction.CommitAsync();

                                }

                                //ADD NEW RECORD TO SCHWARZ database



                                Company schwarzCompany = await _schwarzContext.Companies.FindAsync(company.Id);


                                //if Id already exists then update the database and not dont add New

                                if (schwarzCompany != null)
                                {
                                    using (var schwarzDbContextTransaction = _schwarzContext.Database.BeginTransaction())
                                    {
                                        //isCompanyAlreadyApproved

                                        //if Company is isCompanyAlreadyApproved and re-approved by all levels then
                                        // update the data and dont delete it.


                                        if (isCompanyAlreadyApproved)
                                        {// update the exisitng company record in Schwarz database 

                                            //assign values
                                            updateCompany = await _schwarzContext.Companies.FindAsync(companyPutDTO.Id);

                                            updateCompany.AccountGroup = companyPutDTO.AccountGroup;
                                            updateCompany.Building = companyPutDTO.Building;
                                            updateCompany.City = companyPutDTO.City;
                                            updateCompany.CommercialRegistrationNo = companyPutDTO.CommercialRegistrationNo;
                                            updateCompany.CompanyName = companyPutDTO.CompanyName;
                                            updateCompany.Country = companyPutDTO.Country;
                                            updateCompany.District = companyPutDTO.District;
                                            updateCompany.FaxNumber = companyPutDTO.FaxNumber;
                                            updateCompany.Floor = companyPutDTO.Floor;
                                            updateCompany.HouseNo = companyPutDTO.HouseNo;
                                            updateCompany.Language = companyPutDTO.Language;
                                            updateCompany.MobileNo = companyPutDTO.MobileNo;
                                            updateCompany.Notes = companyPutDTO.Notes;
                                            updateCompany.PhoneNo = companyPutDTO.PhoneNo;
                                            updateCompany.POBox = companyPutDTO.POBox;
                                            updateCompany.PostalCode = companyPutDTO.PostalCode;
                                            updateCompany.Region = companyPutDTO.Region;
                                            updateCompany.Room = companyPutDTO.Room;
                                            updateCompany.Street = companyPutDTO.Street;
                                            updateCompany.VatNo = companyPutDTO.VatNo;
                                            updateCompany.VendorType = companyPutDTO.VendorType;
                                            updateCompany.Website = companyPutDTO.Website;

                                            updateCompany.IsVendorInitiated = companyPutDTO.IsVendorInitiated ?? false;
                                            updateCompany.RecordDate = curDateTime;
                                            updateCompany.IsApproved = true; //currentCompanyDetail.IsApproved ? currentCompanyDetail.IsApproved : false;//
                                            updateCompany.ApprovedDate = curDateTime;//currentCompanyDetail.IsApproved ? currentCompanyDetail.ApprovedDate : null;

                                            _schwarzContext.Companies.Update(updateCompany);
                                            await _schwarzContext.SaveChangesAsync();

                                            //Get the DB Generated Identity Column Value after save.
                                            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                                            updateCompId = updateCompany.Id;
                                            ///>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                            //remove all existing Contacts related to the company
                                            _schwarzContext.Contacts.RemoveRange(_schwarzContext.Contacts.Where(c => c.CompanyID == updateCompId));
                                            //await _schwarzContext.SaveChangesAsync();
                                            foreach (ContactPutDTO contact in companyPutDTO.ListOfCompanyContacts)
                                            {
                                                Contact newContact = new();

                                                newContact.CompanyID = updateCompId; //Db generated Identity column value
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

                                                _schwarzContext.Contacts.Add(newContact);
                                                await _schwarzContext.SaveChangesAsync();


                                                emailBodyBuilder.AppendLine("================================================================");
                                                emailBodyBuilder.AppendLine("Vendor Contact Details: ");

                                            }

                                            //remove all existing Contacts related to the company
                                            _context.Banks.RemoveRange(_context.Banks.Where(c => c.CompanyID == updateCompId));
                                            await _schwarzContext.SaveChangesAsync();
                                            foreach (BankPutDTO bank in companyPutDTO.ListOfCompanyBanks)
                                            {   ////////////////// UPDATE TO EXISTING RECORD ///////////////////////
                                                ////////////////////////////////////////////////////////////////////
                                                //// *************** IBAN Number Validation *****************///////
                                                ////////////////////////////////////////////////////////////////////
                                                IBANValidation ibanvalidation = new IBANValidation();
                                                if (ibanvalidation.ValidateIBAN(bank.IBAN) != "Valid IBAN Number")
                                                {
                                                    return Ok(new { Status = "Failure", Message = "Invalid IBAN Number: " + bank.IBAN });
                                                }

                                                Bank newBank = new Bank();

                                                newBank.CompanyID = updateCompId; //Db generated Identity column value
                                                newBank.AccountHolderName = bank.AccountHolderName;
                                                newBank.BankAccount = bank.BankAccount;
                                                newBank.Country = bank.Country;
                                                newBank.BankKey = bank.BankKey;
                                                newBank.BankName = bank.BankName;
                                                newBank.Currency = bank.Currency;
                                                newBank.IBAN = bank.IBAN;
                                                newBank.SwiftCode = bank.SwiftCode;


                                                await _schwarzContext.Banks.AddAsync(newBank);

                                                emailBodyBuilder.AppendLine("================================================================");
                                                emailBodyBuilder.AppendLine("Vendor Bank Details: ");
                                                //emailBodyBuilder.AppendLine(JsonConvert.SerializeObject(newBank));

                                            }
                                            await _schwarzContext.SaveChangesAsync();



                                        }
                                        else
                                        {
                                            //Remove the existing Data for the {Id} in Schwarz Table
                                            _schwarzContext.Companies.Remove(schwarzCompany);
                                            _schwarzContext.Banks.RemoveRange(_schwarzContext.Banks.Where(c => c.CompanyID == company.Id));
                                            _schwarzContext.Contacts.RemoveRange(_schwarzContext.Contacts.Where(c => c.CompanyID == company.Id));
                                        }
                                        await schwarzDbContextTransaction.CommitAsync(); //ADDED HERE
                                    }
                                }
                                else
                                {
                                    using (var schwarzDbContextTransaction = _schwarzContext.Database.BeginTransaction())
                                    {

                                        Company newCompany = new Company();

                                        newCompany.AccountGroup = companyPutDTO.AccountGroup;
                                        newCompany.Building = companyPutDTO.Building;
                                        newCompany.City = companyPutDTO.City;
                                        newCompany.CommercialRegistrationNo = companyPutDTO.CommercialRegistrationNo;
                                        newCompany.CompanyName = companyPutDTO.CompanyName;
                                        newCompany.Email = _context.Companies.Find(companyPutDTO.Id).Email; //get email from DB to prevent Email tampering
                                        newCompany.Country = companyPutDTO.Country;
                                        newCompany.District = companyPutDTO.District;
                                        newCompany.FaxNumber = companyPutDTO.FaxNumber;
                                        newCompany.Floor = companyPutDTO.Floor;
                                        newCompany.HouseNo = companyPutDTO.HouseNo;
                                        newCompany.Language = companyPutDTO.Language;
                                        newCompany.MobileNo = companyPutDTO.MobileNo;
                                        newCompany.Notes = companyPutDTO.Notes;
                                        newCompany.PhoneNo = companyPutDTO.PhoneNo;
                                        newCompany.POBox = companyPutDTO.POBox;
                                        newCompany.PostalCode = companyPutDTO.PostalCode;
                                        newCompany.Region = companyPutDTO.Region;
                                        newCompany.Room = companyPutDTO.Room;
                                        newCompany.Street = companyPutDTO.Street;
                                        newCompany.VatNo = companyPutDTO.VatNo;
                                        newCompany.VendorType = companyPutDTO.VendorType;
                                        newCompany.Website = companyPutDTO.Website;

                                        newCompany.IsVendorInitiated = companyPutDTO.IsVendorInitiated ?? false;
                                        newCompany.RecordDate = company.RecordDate;
                                        newCompany.IsApproved = true;
                                        newCompany.ApprovedDate = curDateTime;

                                        _schwarzContext.Companies.Add(newCompany);
                                        await _schwarzContext.SaveChangesAsync();

                                        emailBodyBuilder.AppendLine("==================================================================================================");
                                        emailBodyBuilder.AppendLine("Vendor Company Details: " + newCompany.CompanyName);
                                        emailBodyBuilder.AppendLine("                        " + newCompany.City + ", " + newCompany.Country + ", " + newCompany.PostalCode);
                                        emailBodyBuilder.AppendLine("                        " + newCompany.MobileNo + ", " + newCompany.PhoneNo);
                                        emailBodyBuilder.AppendLine("Registration No: " + newCompany.CommercialRegistrationNo);
                                        emailBodyBuilder.AppendLine("==================================================================================================");


                                        foreach (ContactPutDTO contact in companyPutDTO.ListOfCompanyContacts)
                                        {
                                            Contact newContact = new();
                                            newContact.CompanyID = newCompany.Id; //Db generated Identity column value
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

                                            await _schwarzContext.Contacts.AddAsync(newContact);
                                            await _schwarzContext.SaveChangesAsync();

                                            emailBodyBuilder.AppendLine("================================================================");
                                            emailBodyBuilder.AppendLine("Vendor Contact Details: ");

                                        }

                                        foreach (BankPutDTO bank in companyPutDTO.ListOfCompanyBanks)
                                        {
                                            Bank newBank = new Bank();

                                            newBank.CompanyID = newCompany.Id; //Db generated Identity column value
                                            newBank.AccountHolderName = bank.AccountHolderName;
                                            newBank.BankAccount = bank.BankAccount;
                                            newBank.Country = bank.Country;
                                            newBank.BankKey = bank.BankKey;
                                            newBank.BankName = bank.BankName;
                                            newBank.Currency = bank.Currency;
                                            newBank.IBAN = bank.IBAN;
                                            newBank.SwiftCode = bank.SwiftCode;


                                            await _schwarzContext.Banks.AddAsync(newBank);
                                            await _schwarzContext.SaveChangesAsync();
                                            emailBodyBuilder.AppendLine("================================================================");
                                            emailBodyBuilder.AppendLine("Vendor Bank Details: ");
                                        }
                                        //HERE IT IS 
                                        await schwarzDbContextTransaction.CommitAsync();

                                    }

                                }
                                if (!isCompanyAlreadyApproved)
                                {
                                    //3 : Create user ID and password and send the mail 
                                    newCompanyPassword = GenerateRandomPassword();
                                    var NewUser = new ApplicationUser
                                    {
                                        UserName = company.Email,
                                        Email = company.Email,
                                        ApproverLevel = 0,
                                        PasswordHash = newCompanyPassword

                                    };

                                    using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
                                    {

                                        IdentityResult UserAddResult = await _userManager.CreateAsync(NewUser, newCompanyPassword);

                                        var user = await _userManager.FindByEmailAsync(NewUser.Email);

                                        IdentityResult RoleAddresult = await _userManager.AddToRoleAsync(NewUser, "Vendor");

                                        await AtoVenDbContextTransaction.CommitAsync();
                                    }

                                    //    scope.Complete();
                                    //}

                                    // 4: Send Email with User-Id and Password to Vendor
                                    //If next approver is available Send email 
                                    emailBodyBuilder = new StringBuilder();

                                    emailBodyBuilder.AppendLine("Congratulations - You are now approved Vendor");
                                    emailBodyBuilder.AppendLine("==================================================");
                                    emailBodyBuilder.AppendLine("Company Name: " + company.CompanyName);
                                    emailBodyBuilder.AppendLine("Company Registration No:" + company.CommercialRegistrationNo);
                                    emailBodyBuilder.AppendLine("Company Location: " + company.City + ", " + company.Country);
                                    emailBodyBuilder.AppendLine("                                                             ");
                                    emailBodyBuilder.AppendLine("==================================================");
                                    emailBodyBuilder.AppendLine("Your User Id: " + company.Email);
                                    emailBodyBuilder.AppendLine("Your Password: " + newCompanyPassword);
                                    emailBodyBuilder.AppendLine("==================================================");

                                    var VendorMailAddress = company.Email;
                                    string VendorSubject = "Congratulations Your " + company.CompanyName + " is now a registered Vendor!";
                                    string VendorContent = emailBodyBuilder.ToString();
                                    var VendorMmessagemail = new Message(new string[] { VendorMailAddress }, VendorSubject, VendorContent);
                                    await _emailSender.SendEmailAsync(VendorMmessagemail);
                                }
                                else
                                {
                                    //already has user Id so dont sent user-id and password
                                    emailBodyBuilder.AppendLine("FYI...Your Company Details in our Database has been Updated after scrutity");
                                    emailBodyBuilder.AppendLine("==================================================");
                                    emailBodyBuilder.AppendLine("Company Name: " + company.CompanyName);
                                    emailBodyBuilder.AppendLine("Company Registration No:" + company.CommercialRegistrationNo);
                                    emailBodyBuilder.AppendLine("Company Location: " + company.City + ", " + company.Country);
                                    emailBodyBuilder.AppendLine("Your User-Id and Password remains the same.");
                                    emailBodyBuilder.AppendLine("==================================================");

                                    var VendorMailAddress = company.Email;
                                    string VendorSubject = ".Your Company" + company.CompanyName + " Details in our Database has been Updated";
                                    string VendorContent = emailBodyBuilder.ToString();
                                    var VendorMmessagemail = new Message(new string[] { VendorMailAddress }, VendorSubject, VendorContent);
                                    await _emailSender.SendEmailAsync(VendorMmessagemail);
                                }



                                //_context.ApprovalFlows.Update(updateApprovalFlow);
                                //await _context.SaveChangesAsync();




                                //BEFORE THIS LINE
                            }
                            //Email Sent
                        }

                    }


                    //_context.ApprovalFlows.Update(updateApprovalFlow);

                    //email was here
                }

                //await _context.SaveChangesAsync();
            }




            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return Ok(new { Status = "Failure", Message = "Company Id Invalid!" });
                }
                else
                {
                    return Ok(new { Status = "Failure", Message = "Check DbContext Concurrency Issue!" });
                }
            }

            return Ok(new { Status = "Success", Message = "Vendor Details Update request under Process!" });
        }






        [HttpPost]
        [ActionName("RegisterCompany")]
        //[Authorize(Roles = "Admin, AtoVenAdmin, Approver, Vendor")]
        [AllowAnonymous]
        public async Task<ActionResult<Company>> PostCompany(CompanyPostDTO company)
        {
            if (_context.Users.Max(u => u.ApproverLevel) < 1)
            {
                return Ok(new { Status = "Failure", Message = "No Approvers Found!" });
            }
            if (_context.Companies.Where(c => c.Email == company.Email).Any())
            {
                return Ok(new { Status = "Failure", Message = "Company Email ID must be unique!" });
            }

            emailBodyBuilder.AppendLine("===========================================================");
            emailBodyBuilder.Append(Environment.NewLine);
            emailBodyBuilder.AppendLine("New Vendor " + company.CompanyName + " Approval request");
            emailBodyBuilder.Append(Environment.NewLine);
            emailBodyBuilder.AppendLine("===========================================================");
            emailBodyBuilder.Append(Environment.NewLine);

            ////////////////////////////////////////////////////////////////////
            //// ***************   VAT Validation   *********************///////
            ////////////////////////////////////////////////////////////////////

            VATValidation vatvalidation = new VATValidation();
            if (vatvalidation.ValidateVAT(company.VatNo) != "Valid VAT Number")
            {
                return Ok(new { Status = "Failure", Message = "Invalid VAT Number: " + company.VatNo });

            }

            emailBodyBuilder.AppendLine("VAT Number: Validated");
            emailBodyBuilder.Append(Environment.NewLine);

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
                return Ok(new { Status = "Failure", Message = "Invalid Street Address" });
            }
            //
            emailBodyBuilder.AppendLine("Street Address: Validated");
            emailBodyBuilder.Append(Environment.NewLine);


            int newCompId = 0;
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

                newCompany.IsVendorInitiated = company.IsVendorInitiated ?? false;
                newCompany.RecordDate = DateTime.Now;
                newCompany.IsApproved = false;
                newCompany.ApprovedDate = null;

                _context.Companies.Add(newCompany);
                await _context.SaveChangesAsync();

                emailBodyBuilder.AppendLine("==================================================================================================");
                emailBodyBuilder.Append(Environment.NewLine);
                emailBodyBuilder.AppendLine("Vendor Company Details: " + newCompany.CompanyName);
                emailBodyBuilder.AppendLine("                        " + newCompany.City + ", " + newCompany.Country + ", " + newCompany.PostalCode);
                emailBodyBuilder.AppendLine("                        " + newCompany.MobileNo + ", " + newCompany.PhoneNo);
                emailBodyBuilder.Append(Environment.NewLine);
                emailBodyBuilder.AppendLine("Registration No: " + newCompany.CommercialRegistrationNo);
                emailBodyBuilder.Append(Environment.NewLine);
                emailBodyBuilder.AppendLine("==================================================================================================");
                emailBodyBuilder.Append(Environment.NewLine);



                //Get the DB Generated Identity Column Value after save.
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                newCompId = newCompany.Id;
                ///>>>>>>>>>>>>>>>>>>>>>>>>>>>

                foreach (ContactPostDTO contact in company.ListOfCompanyContacts)
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


                    emailBodyBuilder.AppendLine("================================================================");
                    emailBodyBuilder.Append(Environment.NewLine);
                    emailBodyBuilder.AppendLine("Vendor Contact Details: ");
                    emailBodyBuilder.Append(Environment.NewLine);
                }


                foreach (BankPostDTO bank in company.ListOfCompanyBanks)
                {
                    ////////////////////////////////////////////////////////////////////
                    //// *************** IBAN Number Validation *****************///////
                    ////////////////////////////////////////////////////////////////////
                    IBANValidation ibanvalidation = new IBANValidation();
                    if (ibanvalidation.ValidateIBAN(bank.IBAN) != "Valid IBAN Number")
                    {
                        return Ok(new { Status = "Failure", Message = "Invalid IBAN Number: " + bank.IBAN });
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
                    emailBodyBuilder.AppendLine("================================================================");
                    emailBodyBuilder.Append(Environment.NewLine);
                    emailBodyBuilder.AppendLine("Vendor Bank Details: ");
                }

                await AtoVenDbContextTransaction.CommitAsync();
            }



            //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
            ////////////////////////////////////////////////////////////////////
            //// ***************   All Duplications Check FLOW   ********///////
            ////////////////////////////////////////////////////////////////////
            /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//

            DuplicatesValidation duplicate = new DuplicatesValidation(_context, _schwarzContext);
            bool areDuplicatesFound = await duplicate.IsDuplicate(newCompany);



            //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
            ////////////////////////////////////////////////////////////////////
            //// ***************    Add Approval FLOW   *****************///////
            ////////////////////////////////////////////////////////////////////
            /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//

            //find out the no of levels of approvals max = 2
            int maxApprovalLevel = _context.Users.Max(u => u.ApproverLevel);

            for (int i = 1; i <= maxApprovalLevel; i++)
            {
                List<ApplicationUser> listApprovers = _context.Users.Where(u => u.ApproverLevel == i).ToList();

                foreach (ApplicationUser approver in listApprovers)
                {
                    ApprovalFlow newApprovalFlow = new ApprovalFlow();

                    newApprovalFlow.CompanyID = newCompany.Id;
                    newApprovalFlow.RecordDate = newCompany.RecordDate;
                    newApprovalFlow.ApproverEmail = approver.Email;
                    newApprovalFlow.ApproverLevel = approver.ApproverLevel;
                    newApprovalFlow.ApprovalStatus = (int)ApprovalStatusType.Pending;
                    newApprovalFlow.IsDuplicateEntry = areDuplicatesFound;

                    _context.ApprovalFlows.Add(newApprovalFlow);

                    //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                    ////////////////////////////////////////////////////////////////////
                    //// *************** Send Email to Approver *****************///////
                    ////////////////////////////////////////////////////////////////////
                    /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                    ///
                    if (i == 1) //only first approver will receive email
                    {
                        await SendEmailInHtml(
                            approver.Email,
                            "New Vendor " + newCompany.CompanyName + " Approval request",
                            emailBodyBuilder.ToString());
                    }
                    ////////////////////////////////////////////////////////////////////
                    /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                }

                await _context.SaveChangesAsync();

            }


            await SendEmailInHtml(newCompany.Email,
                                              "Your request submitted for Vendor Registration !",
                                              "Your request for " + newCompany.CompanyName + " Vendor Registration is now Initiated!");

            ////////////////////////////////////////
            ///



            return Ok(new { Status = "Success", Message = "New Vendor request Initiated!" });

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


        [HttpGet]
        [ActionName("GetCompanyForVendor")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompanyForVendor()
        {
            var uName = User.Identity.Name;
            ApplicationUser applicationUser = await _userManager.FindByNameAsync(uName);

            Company company = _context.Companies.Where(c => c.Email == applicationUser.Email).FirstOrDefault();


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


            companyDTO.IsVendorInitiated = company.IsVendorInitiated;
            companyDTO.RecordDate = company.RecordDate;
            companyDTO.IsApproved = company.IsApproved;
            companyDTO.ApprovedDate = company.ApprovedDate ?? null;

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
            var ListContacts = _context.Contacts.Where(b => b.CompanyID == companyDTO.Id).ToList();

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
                contactDTO.PhoneNo = contact.PhoneNo;
                contactDTO.FaxNo = contact.FaxNo;
                contactDTO.Email = contact.Email;
                contactDTO.Language = contact.Language;
                contactDTO.Country = contact.Country;

                ListContactDTOs.Add(contactDTO);

            }

            return Ok(companyDTO);
        }
    }


    //////////[HttpPost]
    //////////[ActionName("PostDocuments")]
    //////////public async Task<ActionResult<List<FileDocumentDTO>>> PostFiles([FromForm] IFormFileCollection Documents)
    //////////{
    //////////    //StringBuilder StrBuilderUploadedDocuments = new();

    //////////    List<FileDocumentDTO> fileDocumentDTOs = new();

    //////////    foreach (IFormFile document in Documents)
    //////////    {
    //////////        //Store the file to the contentrootpath/images =>
    //////////        //for docker it is /app/Images configured with volume mount in docker-compose

    //////////        string uploadsFolder = Path.Combine(hostingEnvironment.ContentRootPath, "documents");
    //////////        string uniqueFileName = Guid.NewGuid().ToString() + "_" + document.FileName;
    //////////        string filePath = Path.Combine(uploadsFolder, uniqueFileName);


    //////////        try
    //////////        {
    //////////            using var stream = new FileStream(filePath, FileMode.Create);
    //////////            await document.CopyToAsync(stream);
    //////////            stream.Flush();


    //////////            // Save it to the acutal FileDocuments table
    //////////            FileDocument fileDocument = new();
    //////////            fileDocument.ActualFileName = document.FileName;
    //////////            fileDocument.UniqueFileName = uniqueFileName;
    //////////            _context.FileDocuments.Add(fileDocument);
    //////////            await _context.SaveChangesAsync();
    //////////            //

    //////////            // Populating the List of Document Id for FrontEnd consumption
    //////////            FileDocumentDTO fileDocumentDTO = new();
    //////////            fileDocumentDTO.Id = fileDocument.Id;
    //////////            fileDocumentDTO.ActualFileName = document.FileName;
    //////////            fileDocumentDTOs.Add(fileDocumentDTO);

    //////////            //StrBuilderUploadedDocuments.Append(uniqueFileName + "^");
    //////////            //
    //////////        }
    //////////        catch (Exception ex)
    //////////        {
    //////////            return Conflict(new { Status = "Failure", Message = "File not uploaded.. Please retry!" + ex.ToString() });

    //////////        }




    //////////    }

    //////////    return Ok(fileDocumentDTOs);
    //////////}

    //////////[HttpGet("{id}")]
    //////////[ActionName("GetDocumentsByCompanyId")]
    ////////////<List<FileContentResult>
    //////////public async Task<ActionResult> GetDocumentsByCompanyId(int id)
    //////////{
    //////////    List<int> documentIds = _context.Companies.Find(id).DocumentIDs.Split(",").Select(Int32.Parse).ToList();
    //////////    string documentsFolder = Path.Combine(hostingEnvironment.ContentRootPath, "documents");

    //////////    List<string> docUrls = new();

    //////////    var provider = new FileExtensionContentTypeProvider();
    //////////    await Task.Run(() =>
    //////////    {
    //////////        foreach (int docid in documentIds)
    //////////        {
    //////////            var fd = _context.FileDocuments.Find(docid);
    //////////            string uniqueFileName = fd.UniqueFileName;
    //////////            string actualFileName = fd.ActualFileName;

    //////////            string filePath = Path.Combine(documentsFolder, uniqueFileName);

    //////////            string docUrl = Directory.EnumerateFiles(documentsFolder).Select(f => filePath).FirstOrDefault().ToString();
    //////////            docUrls.Add(docUrl);


    //////////        }
    //////////    });
    //////////    return Ok(docUrls);
    //////////}


    //////////[HttpGet("{id}")]
    //////////[ActionName("GetDocumentByDocId")]
    //////////public async Task<ActionResult> GetDocumentByDocId(int id)
    //////////{
    //////////    string documentsFolder = Path.Combine(hostingEnvironment.ContentRootPath, "documents");
    //////////    //var content = new MultipartContent();

    //////////    var provider = new FileExtensionContentTypeProvider();

    //////////    var fd = _context.FileDocuments.Find(id);
    //////////    string uniqueFileName = fd.UniqueFileName;
    //////////    //string actualFileName = fd.ActualFileName;

    //////////    string filePath = Path.Combine(documentsFolder, uniqueFileName);
    //////////    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
    //////////    if (!provider.TryGetContentType(filePath, out var contentType))
    //////////    {
    //////////        contentType = "application/octet-stream";
    //////////    }

    //////////    //FileContentResult thisfile = File(bytes, contentType, Path.GetFileName(filePath));

    //////////    return File(bytes, contentType, Path.GetFileName(filePath));
    //////////}

}
