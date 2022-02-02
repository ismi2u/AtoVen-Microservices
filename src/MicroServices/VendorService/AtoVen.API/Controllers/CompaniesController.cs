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
//using EmailSender;

namespace AtoVen.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Route("api/[controller]")]
    //[Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, User")]
    public class CompaniesController : ControllerBase
    {
        private readonly AtoVenDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public CompaniesController(AtoVenDbContext context, IEmailSender emailSender,
                                UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
        [ActionName("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<Company>> AccountLogin(Login login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);

            if (result.Succeeded)
            {
                var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey12323232"));

                var signingcredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

                var modeluser = await _userManager.FindByEmailAsync(login.Email);
                var userroles = await _userManager.GetRolesAsync(modeluser);


                //add claims
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, modeluser.UserName),
                 new Claim(ClaimTypes.Email, login.Email)


                };
                //add all roles belonging to the user
                foreach (var role in userroles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var tokenOptions = new JwtSecurityToken(

                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddHours(5),
                     signingCredentials: signingcredentials
                    );


                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);


                return Ok(new { Token = tokenString, Role = userroles, Email = login.Email });

            }
            return Unauthorized(new { Status = "Failure", Message = "Incorrect User-Id/Password!" });
        }

        [HttpPost]
        [ActionName("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok(new { Status = "Success", Message = "Logged Out Successfully!" });
        }

        // POST: api/Companies/RegisterVendor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ActionName("RegisterVendor")]
        public async Task<ActionResult<Company>> PostCompany(CompanyDTO company)
        {
            int newCompId = 0;
            Company newCompany;

            const string IBANAPIValidatorKey = "12343434455";
            const string AddressAPIValidatorKey = "av-46947951bfd581f1e1d6c206b2a8f9e0";
            const string VATAPIValidatorKey = "579db579789b253ddbc7708f84990f18";

            string[] ArrBankIBAN = null;

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
                newCompany.Password = GenerateRandomPassword(null);
                newCompany.IsVendorInitiated = true;
                newCompany.RecordDate = DateTime.Now;

                newCompany.ApproverID = 1;
                newCompany.ApproverRoleID = 1;
                newCompany.IsApproved = false;
                newCompany.ApprovedDate = null;


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
                    newContact.MobileNo = contact.MobileNo;
                    newContact.Department = contact.Department;
                    newContact.FirstName = contact.FirstName;
                    newContact.Address = contact.Address;
                    newContact.LastName = contact.LastName;
                    newContact.Designation = contact.Designation;
                    newContact.Country = contact.Country;

                    _context.Contacts.Add(newContact);
                    await _context.SaveChangesAsync();
                }

                int intBankCount = 0;
                ArrBankIBAN = new string[company.ListOfCompanyBanks.Count];
                foreach (BankDTO bank in company.ListOfCompanyBanks)
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

                    ArrBankIBAN[intBankCount] = bank.IBAN;
                    intBankCount += 1;

                    _context.Banks.Add(newBank);
                    await _context.SaveChangesAsync();
                }

                await AtoVenDbContextTransaction.CommitAsync();
            }
            ///Register a userId for the Vendor
            ///
            var user = new IdentityUser
            {
                UserName = newCompany.Email,
                Email = newCompany.Email,
                PasswordHash = newCompany.Password,
                NormalizedUserName = newCompany.CompanyName

            };

            var result = await _userManager.CreateAsync(user, newCompany.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }


            //IBAN Validation // address multiple IBAN validation is pending
            IBANValidationResult iBANValidationResult = (IBANValidationResult)ValidateIBAN(IBANAPIValidatorKey, ArrBankIBAN[0]);

            //ADDRESS Validation //
            AddressValidationResult addressResult = (AddressValidationResult)ValidateAddress(
                AddressAPIValidatorKey,
                newCompany.Street,
                newCompany.City,
                newCompany.PostalCode,
                 newCompany.Region,
                 newCompany.Country,
                 newCompany.Language);

            //VAT Validation
            APIValidationResult vatResult = (APIValidationResult)ValidateVAT(VATAPIValidatorKey, newCompany.VatNo);


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


        /////// 2. Validate IBAN Number /////
        /// <summary>
        /// https://api.iban.com/clients/api/v4/iban/?api_key=[YOUR_API_KEY]&format=json&iban=DE02100500000024290661
        /// </summary>
        /// <param name="IBANAPIKey"></param>
        /// <param name="IBANNumber"></param>
        /// <returns></returns>
        public object ValidateIBAN(string IBANAPIKey = "[YOUR_API_KEY]", string IBANNumber = "DE02100500000024290661")
        {

            HttpClient IbanClient = new HttpClient();

            string uri = "https://api.iban.com/clients/api/v4/iban/?api_key=" + IBANAPIKey + "&format=json&iban=" + IBANNumber;
            var response = IbanClient.PostAsync(uri, null);

            IBANValidationResult ibanValidationResult = new IBANValidationResult() { Status = "Success", ResultData = response.ToString() };

            return ibanValidationResult;

            /*var request = (HttpWebRequest)WebRequest.Create("https://api.iban.com/clients/api/v4/iban/");
            var postData = "api_key=[YOUR_API_KEY]";
            postData += "&format=json";
            postData += "&iban=DE02100500000024290661";
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();*/

        }

        /// <summary>
        ///         //// 3. Address validator 
        /// API key : av-46947951bfd581f1e1d6c206b2a8f9e0
        /// https://www.address-validator.net/api.html
        /// /// </summary>
        /// <param name="AddressValidatorAPIKey"></param>
        /// <param name="vStreetAddress"></param>
        /// <param name="vCity"></param>
        /// <param name="vPostalCode"></param>
        /// <param name="vState"></param>
        /// <param name="vCountryCode"></param>
        /// <param name="vLocale"></param>
        /// <returns></returns>
        public object ValidateAddress(string AddressValidatorAPIKey, string vStreetAddress,
                                        string vCity, string vPostalCode,
                                        string vState, string vCountryCode,
                                        string vLocale)
        {
            const String APIURL = "https://api.address-validator.net/api/verify";
            HttpClient client = new HttpClient();
            String StreetAddress = vStreetAddress;
            String City = vCity;
            String PostalCode = vPostalCode;
            String State = vState;
            String CountryCode = vCountryCode;
            String Locale = "en";
            String APIKey = AddressValidatorAPIKey;

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("StreetAddress", StreetAddress));
            postData.Add(new KeyValuePair<string, string>("City", City));
            postData.Add(new KeyValuePair<string, string>("PostalCode", PostalCode));
            postData.Add(new KeyValuePair<string, string>("State", State));
            postData.Add(new KeyValuePair<string, string>("CountryCode", CountryCode));
            postData.Add(new KeyValuePair<string, string>("Locale", Locale));
            postData.Add(new KeyValuePair<string, string>("APIKey", APIKey));

            HttpContent content = new FormUrlEncodedContent(postData);

            HttpResponseMessage result = client.PostAsync(APIURL, content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;
            APIValidationResult res = JsonSerializer.Deserialize<APIValidationResult>(resultContent);
            string formattedaddress = "";

            if (res.status.Equals("VALID"))
            {
                formattedaddress = res.formattedaddress;
            }

            AddressValidationResult addressValidationResult = new AddressValidationResult() { Status = res.status, ResultData = formattedaddress };

            return addressValidationResult;

        }


        /// <summary>
        ///   ///4. Validate VAT
        /// API Key : 579db579789b253ddbc7708f84990f18
        /// http://www.apilayer.net/api/validate?access_key=579db579789b253ddbc7708f84990f18
        // http://apilayer.net/api/validate/?access_key=YOUR_ACCESS_KEY&vat_number=LU26375245
        /// </summary>
        /// <param name="VATAPIValidatorKey"></param>
        /// <param name="VATNumber"></param>
        /// <returns>Oject</returns>
        public object ValidateVAT(string VATAPIValidatorKey, string VATNumber)
        {
            string APIURL = "http://www.apilayer.net/api/validate?access_key=" + VATAPIValidatorKey + "&vat_number=" + VATNumber;
            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.PostAsync(APIURL, null).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;
            APIValidationResult VATresult = JsonSerializer.Deserialize<APIValidationResult>(resultContent);
            return VATresult;
        }


        /// <summary>
        /// Generate Random Password satisfying all the Password Requirement chars
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static string GenerateRandomPassword(PasswordOptions opts = null)
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
