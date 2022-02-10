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

namespace AtoVen.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin, AtoVenAdmin, Approver, Vendor")]
    public class CompaniesController : ControllerBase
    {
        private readonly AtoVenDbContext _context;
        private readonly SchwarzDbContext _SchwarzContext;
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
            _SchwarzContext = schwarzContext;
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

                ListCompanyDTOs.Add(companyDTO);
            }

            _logger.LogInformation("Company Added: " + DateTime.Now);

            return Ok(ListCompanyDTOs);

        }


        [HttpGet]
        [ActionName("GetCompaniesApproved")]
        //[Authorize(Roles = "Admin, AtoVenAdmin, Approver")]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetCompaniesApproved()
        {
            List<CompanyDTO> ListCompanyDTOs = new();

            var ListCompanies = await _context.Companies.Where(c=> c.IsApproved== true).ToListAsync();

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

                ListCompanyDTOs.Add(companyDTO);
            }

            _logger.LogInformation("Company Added: " + DateTime.Now);

            return Ok(ListCompanyDTOs);

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

            return Ok(companyDTO);
        }


        [HttpGet("{id}")]
        [ActionName("GetCompanyDuplicatesByCompId")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanyDuplicatesByCompId(int id)
        {
            Company company = await _context.Companies.FindAsync(id);
            DuplicatesValidation duplicate = new DuplicatesValidation(_context, _SchwarzContext);
            List<Company> ListDuplicateCompanies = duplicate.CheckDuplicates(company).ToList();

            return Ok(ListDuplicateCompanies);
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ActionName("UpdateCompany")]
        //[Authorize(Roles = "Admin, AtoVenAdmin, Approver, Vendor")]
        public async Task<IActionResult> PutCompany(int id, CompanyPutDTO companyPutDTO)
        {

            emailBodyBuilder.AppendLine("===========================================================");
            emailBodyBuilder.AppendLine("Update Existing Vendor " + companyPutDTO.CompanyName + " Approval request");
            emailBodyBuilder.AppendLine("===========================================================");
            if (id != companyPutDTO.Id)
            {
                return Ok(new { Status = "Failure", Message = "Company Id Invalid!" });
            }

            //_context.Entry(companyPutDTO).State = EntityState.Modified;

            try
            {
                ////////////////// UPDATE TO EXISTING RECORD ///////////////////////
                ////////////////////////////////////////////////////////////////////
                //// ***************   VAT Validation   *********************///////
                ////////////////////////////////////////////////////////////////////

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
                AddressValidation addValidation = new AddressValidation();
                AddressValidationInputs addressValidationInputs = new AddressValidationInputs();
                addressValidationInputs.HouseNo = companyPutDTO.HouseNo;
                addressValidationInputs.Street = companyPutDTO.Street;
                addressValidationInputs.City = companyPutDTO.City;
                addressValidationInputs.Region = companyPutDTO.Region;
                addressValidationInputs.Country = companyPutDTO.Country;
                addressValidationInputs.Language = companyPutDTO.Language;
                addressValidationInputs.PostalCode = companyPutDTO.PostalCode;


                if (addValidation.ValidateStreetAddress(addressValidationInputs) != "")
                {
                    return Ok(new { Status = "Failure", Message = "Invalid Street Address"});
                }
                //
                emailBodyBuilder.AppendLine("Street Address: Validated");


                int updateCompId = 0;
                int[] arrBankIds;
                int[] arrContactIds;

                int totalBankCount = companyPutDTO.ListOfCompanyBanks.Count;
                int totalContactCount = companyPutDTO.ListOfCompanyContacts.Count;

                int intBankCount = 0;
                int intContactCount = 0;
                Company updateCompany;



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
                    updateCompany.RecordDate = DateTime.Now;
                    updateCompany.IsApproved = false;
                    updateCompany.ApprovedDate = null;

                    _context.Companies.Update(updateCompany);
                    await _context.SaveChangesAsync();

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

                    arrContactIds = new int[totalContactCount]; //Initialize the array with count
                    //remove all existing Contacts related to the company
                    _context.Contacts.RemoveRange(_context.Contacts.Where(c => c.CompanyID == updateCompId));
                    foreach (ContactPostDTO contact in companyPutDTO.ListOfCompanyContacts)
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
                        await _context.SaveChangesAsync();


                        emailBodyBuilder.AppendLine("================================================================");
                        emailBodyBuilder.AppendLine("Vendor Contact-" + intContactCount + 1 + " Details: ");
                        //emailBodyBuilder.AppendLine(JsonConvert.SerializeObject(newContact));
                        arrContactIds[intContactCount] = newContact.Id; //Assign new Contact ID to array
                        intContactCount += 1;
                    }


                    arrBankIds = new int[totalBankCount]; // Initialize the array with count

                    //remove all existing Contacts related to the company
                    _context.Banks.RemoveRange(_context.Banks.Where(c => c.CompanyID == updateCompId));

                    foreach (BankPostDTO bank in companyPutDTO.ListOfCompanyBanks)
                    {   ////////////////// UPDATE TO EXISTING RECORD ///////////////////////
                        ////////////////////////////////////////////////////////////////////
                        //// *************** IBAN Number Validation *****************///////
                        ////////////////////////////////////////////////////////////////////
                        IBANValidation ibanvalidation = new IBANValidation();
                        if (ibanvalidation.ValidateIBAN(bank.IBAN) != "Valid IBAN Number")
                        {
                            return Ok("Invalid IBAN Number: " + bank.IBAN);
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
                        await _context.SaveChangesAsync();
                        emailBodyBuilder.AppendLine("================================================================");
                        emailBodyBuilder.AppendLine("Vendor Bank-" + intBankCount + 1 + " Details: ");
                        //emailBodyBuilder.AppendLine(JsonConvert.SerializeObject(newBank));

                        arrBankIds[intBankCount] = newBank.Id; //Assign new bank ID to array
                        intBankCount += 1;
                    }

                    await AtoVenDbContextTransaction.CommitAsync();
                }
               


                //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                ////////////////////////////////////////////////////////////////////
                //// ***************   All Duplications Check FLOW   ********///////
                ////////////////////////////////////////////////////////////////////
                /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//

                DuplicatesValidation duplicate = new DuplicatesValidation(_context, _SchwarzContext);
                bool areDuplicatesFound = duplicate.CheckDuplicates(updateCompany).Count() > 0 ? true : false;

                emailBodyBuilder.AppendLine("///////////////////////////////////////////");

                var listofApprovalFlows = _context.ApprovalFlows.Where(a => a.CompanyID == updateCompany.Id).ToList();
                int i = 1;

                string jwtUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
               ApplicationUser applicationUser = await _userManager.FindByIdAsync(jwtUserId);

                //check Logged in User Role
                bool isVendorRole = false;
                var Roles = await _userManager.GetRolesAsync(applicationUser);

                foreach( string role in Roles)
                {
                    if (role == "Vendor")
                    {
                        isVendorRole = true;
                        break;
                    }
                }

                //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                ////////////////////////////////////////////////////////////////////
                //// ***************    Update Approval FLOW   ***************///////
                ////////////////////////////////////////////////////////////////////
                /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                foreach (ApprovalFlow approvalFlowItem in listofApprovalFlows)
                {
                    ApprovalFlow updateApprovalFlow = await _context.ApprovalFlows.FindAsync(approvalFlowItem.Id);

                    updateApprovalFlow.RecordDate = DateTime.Now;
                    updateApprovalFlow.IsDuplicateEntry = areDuplicatesFound;

                    if (isVendorRole)
                    {
                        updateApprovalFlow.ApprovalStatus = (int)ApprovalStatusType.Pending; //action is by Vendor
                    }
                    else
                    {
                        updateApprovalFlow.ApprovalStatus = (int)ApprovalStatusType.Approved;//action is by Approver
                    }


                    _context.ApprovalFlows.Update(updateApprovalFlow);

                    //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                    ////////////////////////////////////////////////////////////////////
                    //// *************** Send Email to Approver *****************///////
                    ////////////////////////////////////////////////////////////////////
                    /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                    ///
                    if (i == 1) //only first approver will receive email
                    {
                        await SendEmailInHtml(updateApprovalFlow.ApproverEmail,
                                                "New Vendor " + updateCompany.CompanyName + " Approval request",
                                                emailBodyBuilder.ToString());
                    }
                    ////////////////////////////////////////////////////////////////////
                    /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                }

                await _context.SaveChangesAsync();
            }




            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return Ok(new { Status = "Failure", Message = "Company Id Invalid!" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Status = "Success", Message = "New Vendor Request Initiated!" });
        }






        [HttpPost]
        [ActionName("RegisterCompany")]
        //[Authorize(Roles = "Admin, AtoVenAdmin, Approver, Vendor")]
        [AllowAnonymous]
        public async Task<ActionResult<Company>> PostCompany(CompanyPostDTO company)
        {
            if (_context.Users.Max(u => u.ApproverLevel) < 1)
            {
                return BadRequest();
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

                arrContactIds = new int[totalContactCount]; //Initialize the array with count
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
                    emailBodyBuilder.AppendLine("Vendor Contact-" + intContactCount + 1 + " Details: ");
                    emailBodyBuilder.Append(Environment.NewLine);
                    //emailBodyBuilder.AppendLine(JsonConvert.SerializeObject(newContact));
                    arrContactIds[intContactCount] = newContact.Id; //Assign new Contact ID to array
                    intContactCount += 1;
                }



                arrBankIds = new int[totalBankCount]; // Initialize the array with count

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
                    emailBodyBuilder.AppendLine("Vendor Bank-" + intBankCount + 1 + " Details: ");
                    //emailBodyBuilder.AppendLine(JsonConvert.SerializeObject(newBank));

                    arrBankIds[intBankCount] = newBank.Id; //Assign new bank ID to array
                    intBankCount += 1;
                }

                await AtoVenDbContextTransaction.CommitAsync();
            }


            //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
            ////////////////////////////////////////////////////////////////////
            //// ***************    Add Approval FLOW   *****************///////
            ////////////////////////////////////////////////////////////////////
            /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//


            //find out the no of levels of approvals max = 2

            int maxApprovalLevel = _context.Users.Max(u => u.ApproverLevel);



            //<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
            ////////////////////////////////////////////////////////////////////
            //// ***************   All Duplications Check FLOW   ********///////
            ////////////////////////////////////////////////////////////////////
            /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//

            DuplicatesValidation duplicate = new DuplicatesValidation(_context, _SchwarzContext);
            bool areDuplicatesFound = duplicate.CheckDuplicates(newCompany).Count() > 0 ? true : false;


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
                        await SendEmailInHtml(approver.Email,
                                                "New Vendor " + newCompany.CompanyName + " Approval request",
                                                emailBodyBuilder.ToString());
                    }
                    ////////////////////////////////////////////////////////////////////
                    /////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
                }

                await _context.SaveChangesAsync();

            }

            ////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//
            //////////////////////////////////////////////////////////////////////
            ////// *************** Register a userId for the Vendor *******///////
            //////////////////////////////////////////////////////////////////////
            ///////<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//


            //string rndPassword = GenerateRandomPassword(null);
            //var user = new ApplicationUser
            //{
            //    UserName = newCompany.Email,
            //    Email = newCompany.Email,
            //    NormalizedUserName = newCompany.CompanyName,
            //    ApproverLevel = 0,
            //    PasswordHash = rndPassword
            //};

            //var result = await _userManager.CreateAsync(user);

            //if (result.Succeeded)
            //{
            //    return CreatedAtAction("GetCompanyById", new { id = newCompId }, company);
            //}

            return Ok(new { Status = "Success", Message = "New Vendor request Initiated!" });

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
                return Ok(new { Status = "Failure", Message = "Company is Null!" });
            }

            using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
            {

                _context.Companies.Remove(company);
                _context.Contacts.RemoveRange(_context.Contacts.Where(c => c.CompanyID == company.Id));
                _context.Banks.RemoveRange(_context.Banks.Where(b => b.CompanyID == company.Id));
                await _context.SaveChangesAsync();

                await AtoVenDbContextTransaction.CommitAsync();
            }

            return Ok(new { Status = "Success", Message = "Company Details Deleted!" });
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
