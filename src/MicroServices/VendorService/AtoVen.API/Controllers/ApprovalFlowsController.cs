#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmailSendService;
using System.Text;
using Microsoft.AspNetCore.Identity;
using LinqKit;
using DataService.Entities;
using DataService.DataContext;
using DataService.AccountControl.Models;
using Microsoft.AspNetCore.Authorization;

namespace AtoVen.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin, AtoVenAdmin, Approver")]
    public class ApprovalFlowsController : ControllerBase
    {
        private readonly AtoVenDbContext _context;
        private readonly SchwarzDbContext _schwarzContext;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public StringBuilder emailBodyBuilder = new StringBuilder();

        public ApprovalFlowsController(AtoVenDbContext context,
                                        SchwarzDbContext schwarzContext,
                                        IEmailSender emailSender,
                                        RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _schwarzContext = schwarzContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/ApprovalFlows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalFlowDTO>>> GetApprovalFlows()
        {

            List<ApprovalFlowDTO> ListApprovalFlowDTOs = new();

            var ListApprovalFlows = await _context.ApprovalFlows.ToListAsync();

            foreach (ApprovalFlow approvalFlow in ListApprovalFlows)
            {
                ApprovalFlowDTO approvalFlowDTO = new ApprovalFlowDTO();

                approvalFlowDTO.Id = approvalFlow.Id;
                approvalFlowDTO.CompanyID = approvalFlow.CompanyID;
                approvalFlowDTO.CompanyName = _context.Companies.Find(approvalFlow.CompanyID).CompanyName;
                approvalFlowDTO.CompanyRegisterNo = _context.Companies.Find(approvalFlow.CompanyID).CommercialRegistrationNo;
                approvalFlowDTO.RecordDate = approvalFlow.RecordDate;
                approvalFlowDTO.ApprovalStatus = approvalFlow.ApprovalStatus;
                approvalFlowDTO.IsDuplicateEntry = approvalFlow.IsDuplicateEntry;

                ListApprovalFlowDTOs.Add(approvalFlowDTO);
            }
            return Ok(ListApprovalFlowDTOs);
        }


        // GET: api/ApprovalFlows
        [HttpGet]
        [ActionName("GetApprovalFlowByEmailId")]
        public async Task<ActionResult<IEnumerable<ApprovalFlowDTO>>> GetApprovalFlowByEmailId(string email)
        {
            List<ApprovalFlowDTO> ListApprovalFlowDTOs = new();

            var ListApprovalFlows = await _context.ApprovalFlows.Where(a => a.ApproverEmail == email && a.ApprovalStatus==(int)ApprovalStatusType.Pending).ToListAsync();

            ///
            List<ApprovalFlow> FilteredApprovalFlow = new List<ApprovalFlow>();



            foreach (ApprovalFlow approvalFlow in ListApprovalFlows)
            {
                int PreviousApprovalLevel = approvalFlow.ApproverLevel - 1;
                if(PreviousApprovalLevel !=0)
                {
                    ApprovalFlow tempApprFlow = _context.ApprovalFlows.Where(a => a.CompanyID == approvalFlow.CompanyID && a.ApproverLevel == PreviousApprovalLevel).FirstOrDefault();
                    if (tempApprFlow.ApprovalStatus == (int)ApprovalStatusType.Approved)
                    {
                        FilteredApprovalFlow.Add(approvalFlow);
                    }
                }
                else
                {
                    FilteredApprovalFlow.Add(approvalFlow);

                }
            }


            foreach (ApprovalFlow approvalFlow in FilteredApprovalFlow)
            {
                ApprovalFlowDTO approvalFlowDTO = new ApprovalFlowDTO();

                approvalFlowDTO.Id = approvalFlow.Id;
                approvalFlowDTO.CompanyID = approvalFlow.CompanyID;
                approvalFlowDTO.CompanyName = _context.Companies.Find(approvalFlow.CompanyID).CompanyName;
                approvalFlowDTO.CompanyRegisterNo = _context.Companies.Find(approvalFlow.CompanyID).CommercialRegistrationNo;
                approvalFlowDTO.RecordDate = approvalFlow.RecordDate;
                approvalFlowDTO.ApprovalStatus = approvalFlow.ApprovalStatus;
                approvalFlowDTO.IsDuplicateEntry = approvalFlow.IsDuplicateEntry;

                ListApprovalFlowDTOs.Add(approvalFlowDTO);
            }
            return Ok(ListApprovalFlowDTOs);
        }


        [HttpGet]
        [ActionName("GetApprovalFlowByEmailIdInPending")]
        public async Task<ActionResult<IEnumerable<ApprovalFlowDTO>>> GetApprovalFlowByEmailByPending(string email)
        {
            List<ApprovalFlowDTO> ListApprovalFlowDTOs = new();

            var ListApprovalFlows = await _context.ApprovalFlows.Where(a => a.ApproverEmail == email && a.ApprovalStatus == (int)ApprovalStatusType.Pending).ToListAsync();

            ///
            List<ApprovalFlow> FilteredApprovalFlow = new List<ApprovalFlow>();


            foreach (ApprovalFlow approvalFlow in ListApprovalFlows)
            {
                int PreviousApprovalLevel = approvalFlow.ApproverLevel - 1;
                if (PreviousApprovalLevel != 0)
                {
                    ApprovalFlow tempApprFlow = _context.ApprovalFlows.Where(a => a.CompanyID == approvalFlow.CompanyID && a.ApproverLevel == PreviousApprovalLevel).FirstOrDefault();
                    if (tempApprFlow.ApprovalStatus == (int)ApprovalStatusType.Approved)
                    {
                        FilteredApprovalFlow.Add(approvalFlow);
                    }
                }
                else
                {
                    FilteredApprovalFlow.Add(approvalFlow);

                }
            }


            foreach (ApprovalFlow approvalFlow in FilteredApprovalFlow)
            { 
                ApprovalFlowDTO approvalFlowDTO = new ApprovalFlowDTO();

                approvalFlowDTO.Id = approvalFlow.Id;
                approvalFlowDTO.CompanyID = approvalFlow.CompanyID;
                approvalFlowDTO.CompanyName = _context.Companies.Find(approvalFlow.CompanyID).CompanyName;
                approvalFlowDTO.CompanyRegisterNo = _context.Companies.Find(approvalFlow.CompanyID).CommercialRegistrationNo;
                approvalFlowDTO.RecordDate = approvalFlow.RecordDate;
                approvalFlowDTO.ApprovalStatus = approvalFlow.ApprovalStatus;
                approvalFlowDTO.IsDuplicateEntry = approvalFlow.IsDuplicateEntry;

                ListApprovalFlowDTOs.Add(approvalFlowDTO);
            }
            return Ok(ListApprovalFlowDTOs);
        }

        [HttpGet]
        [ActionName("GetApprovalFlowByEmailIdInApproved")]
        public async Task<ActionResult<IEnumerable<ApprovalFlowDTO>>> GetApprovalFlowByEmailByApproved(string email)
        {
            List<ApprovalFlowDTO> ListApprovalFlowDTOs = new();

            var ListApprovalFlows = await _context.ApprovalFlows.Where(a => a.ApproverEmail == email && a.ApprovalStatus == (int)ApprovalStatusType.Approved).ToListAsync();

            foreach (ApprovalFlow approvalFlow in ListApprovalFlows)
            {
                ApprovalFlowDTO approvalFlowDTO = new ApprovalFlowDTO();

                approvalFlowDTO.Id = approvalFlow.Id;
                approvalFlowDTO.CompanyID = approvalFlow.CompanyID;
                approvalFlowDTO.CompanyName = _context.Companies.Find(approvalFlow.CompanyID).CompanyName;
                approvalFlowDTO.CompanyRegisterNo = _context.Companies.Find(approvalFlow.CompanyID).CommercialRegistrationNo;
                approvalFlowDTO.RecordDate = approvalFlow.RecordDate;
                approvalFlowDTO.ApprovalStatus = approvalFlow.ApprovalStatus;
                approvalFlowDTO.IsDuplicateEntry = approvalFlow.IsDuplicateEntry;

                ListApprovalFlowDTOs.Add(approvalFlowDTO);
            }
            return Ok(ListApprovalFlowDTOs);
        }

 
        // GET: api/ApprovalFlows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalFlow>> GetApprovalFlow(int id)
        {
            var approvalFlow = await _context.ApprovalFlows.FindAsync(id);

            if (approvalFlow == null)
            {
                return NotFound();
            }

            ApprovalFlowDTO approvalFlowDTO = new ApprovalFlowDTO();

            approvalFlowDTO.Id = approvalFlow.Id;
            approvalFlowDTO.CompanyID = approvalFlow.CompanyID;
            approvalFlowDTO.CompanyName = _context.Companies.Find(approvalFlow.CompanyID).CompanyName;
            approvalFlowDTO.CompanyRegisterNo = _context.Companies.Find(approvalFlow.CompanyID).CommercialRegistrationNo;
            approvalFlowDTO.RecordDate = approvalFlow.RecordDate;
            approvalFlowDTO.ApprovalStatus = approvalFlow.ApprovalStatus;
            approvalFlowDTO.IsDuplicateEntry = approvalFlow.IsDuplicateEntry;

            return Ok(approvalFlow);
        }

        // PUT: api/ApprovalFlows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprovalFlow(int id, ApprovalFlowPutDTO approvalFlow)
        {

           // ApprovalFlow updateApprovalFlow = await _context.ApprovalFlows.FindAsync(approvalFlow.Id);

           // updateApprovalFlow.RecordDate = DateTime.Now;
           // updateApprovalFlow.ApprovalStatus = (int)ApprovalStatusType.Pending;
           // _context.ApprovalFlows.Update(updateApprovalFlow);
           // await _context.SaveChangesAsync();
           
           //ApprovalFlow NextApprovalFlow =  _context.ApprovalFlows.Where(a => a.CompanyID == updateApprovalFlow.CompanyID && a.ApproverLevel == updateApprovalFlow.ApproverLevel + 1).FirstOrDefault();
            

           // await SendEmailInHtml(NextApprovalFlow.ApproverEmail,
           //                                    "New Vendor " + _context.Companies.Find(updateApprovalFlow.CompanyID).CompanyName + " Approval request",
           //                                    emailBodyBuilder.ToString());


            if (id != approvalFlow.Id)
            {
                return BadRequest();
            }

            Company newCompany;
            string newCompanyPassword = "";

            //_context.Entry(approvalFlow).State = EntityState.Modified;

            try
            {

                var updateApprovalFlow = await _context.ApprovalFlows.FindAsync(approvalFlow.Id);
                updateApprovalFlow.ApprovalStatus = approvalFlow.ApprovalStatus;

                // If approved, Update Approval flow table and check for the next approver
                if (approvalFlow.ApprovalStatus == (int)ApprovalStatusType.Approved)
                {
                    //update ApprovalFlow table data
                    updateApprovalFlow.LevelApprovedDate = DateTime.Now;
                    _context.ApprovalFlows.Update(updateApprovalFlow);
                    await _context.SaveChangesAsync();

                    int compId = updateApprovalFlow.CompanyID;
                    int apprLevel = updateApprovalFlow.ApproverLevel;
                    int nxtApprLevel = apprLevel + 1;
                    Company company = await _context.Companies.FindAsync(compId);
                    List<Contact> contacts = await _context.Contacts.Where(c => c.CompanyID == compId).ToListAsync();
                    List<Bank> banks = await _context.Banks.Where(b => b.CompanyID == compId).ToListAsync();

                    ApprovalFlow nextApproval = _context.ApprovalFlows.Where(a => a.CompanyID == compId && a.ApproverLevel == nxtApprLevel && a.ApprovalStatus==(int)ApprovalStatusType.Pending).FirstOrDefault();


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

                        using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
                        {
                            //assign values
                            Company updateCompany = await _context.Companies.FindAsync(compId);

                            updateCompany.IsApproved = true;
                            updateCompany.ApprovedDate = DateTime.Now;

                            _context.Companies.Update(updateCompany);
                            await _context.SaveChangesAsync();
                            await AtoVenDbContextTransaction.CommitAsync();

                        }
                        using (var schwarzDbContextTransaction = _schwarzContext.Database.BeginTransaction())
                        {

                            //Create and Save Records to SchwarzDB

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

                            newCompany.IsVendorInitiated = company.IsVendorInitiated;
                            newCompany.RecordDate = company.RecordDate;
                            newCompany.IsApproved = true;
                            newCompany.ApprovedDate = DateTime.Now;

                            _schwarzContext.Companies.Add(newCompany);
                            await _schwarzContext.SaveChangesAsync();

                            //Get the DB Generated Identity Column Value after save.
                            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                            int newCompId = newCompany.Id;
                            ///>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            foreach (Contact contact in contacts)
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

                                _schwarzContext.Contacts.Add(newContact);
                                await _schwarzContext.SaveChangesAsync();

                            }


                            foreach (Bank bank in banks)
                            {
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

                                _schwarzContext.Banks.Add(newBank);
                                await _schwarzContext.SaveChangesAsync();

                            }

                            await schwarzDbContextTransaction.CommitAsync();
                        }

                        //3 : Create user ID and password and send the mail 
                        newCompanyPassword = GenerateRandomPassword();
                        var NewUser = new ApplicationUser
                        {
                            UserName = newCompany.CompanyName,
                            Email = newCompany.Email,
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

                        //Email Sent
                    }



                }


                // If Rejected, Update the Approval Flow table and Remove all the Details of the company
                if (approvalFlow.ApprovalStatus == (int)ApprovalStatusType.Rejected)
                {
                    //update ApprovalFlow table data - No "Approved-date" entered
                    _context.ApprovalFlows.Update(updateApprovalFlow);
                    await _context.SaveChangesAsync();

                    var company = await _context.Companies.FindAsync(updateApprovalFlow.CompanyID);

                    if (company != null)
                    {
                        using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
                        {
                            _context.Companies.Remove(company);
                            _context.Contacts.RemoveRange(_context.Contacts.Where(c => c.CompanyID == company.Id));
                            _context.Banks.RemoveRange(_context.Banks.Where(b => b.CompanyID == company.Id));
                            await _context.SaveChangesAsync();

                            await AtoVenDbContextTransaction.CommitAsync();
                        }
                    }
                }




            }



            catch (DbUpdateConcurrencyException)
            {
                if (!ApprovalFlowExists(id))
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

        private bool ApprovalFlowExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }



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
