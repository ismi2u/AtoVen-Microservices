using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataService.DataContext;
using DataService.Entities;

namespace DataService.DataContext
{
    public class DuplicatesValidation 
    {

        private readonly AtoVenDbContext _context;

        public DuplicatesValidation(AtoVenDbContext context)
        {
            _context = context;
        }


        //1. establish dbcontext
        //    2. 

        public string CheckDuplicates(Company company)
        {
            //ar customers = context.Customers.Where(c => c.Name.StartsWith("a")); // Version A
            //var customers = context.Customers.Where(c => EF.Functions.Like(c.Name, "a%")); // Version B


            string searchText = company.CompanyName;

            string phrase = searchText;
            string[] words = phrase.Split(' ');

            //var predicate = PredicateBuilder.New<Company>();

            List<Company> ListOfCompanies = new();

            ////Break Word Search
            foreach (string word in words)
            {
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.CompanyName.Contains(word)).ToList());
            }

            //Contains matching words search count
            int containsSimilarWordCount = ListOfCompanies.Count();
            //First 3 letters search count
            int first3LettersMatchCount = _context.Companies.Where(c => c.CompanyName.StartsWith(company.CompanyName.Substring(0, 3))).ToList().Count();
            //Last 3 letters search count
            int last3LettersMatchCount = _context.Companies.Where(c => c.CompanyName.StartsWith(company.CompanyName.Substring(company.CompanyName.Length, -3))).ToList().Count();

            //Mobile number match count
            int mobileNoMatchCount = _context.Companies.Where(c => c.MobileNo.Contains(company.MobileNo)).ToList().Count();

            int first5MobileNoCharsMatchCount = _context.Companies.Where(c => c.CompanyName.StartsWith(company.MobileNo.Substring(0, 5).ToString())).ToList().Count();
            //Last 3 letters search count
            int last5MobileNoCharsMatchCount = _context.Companies.Where(c => c.CompanyName.StartsWith(company.MobileNo.Substring(company.MobileNo.Length, -5))).ToList().Count();


            //phone number match count
            int phoneNoMatchCount = _context.Companies.Where(c => c.PhoneNo.Contains(company.PhoneNo)).ToList().Count();

            int first5PhoneNoCharsMatchCount = _context.Companies.Where(c => c.CompanyName.StartsWith(company.PhoneNo.Substring(0, 5))).ToList().Count();
            //Last 3 letters search count
            int last5PhoneNoCharsMatchCount = _context.Companies.Where(c => c.CompanyName.StartsWith(company.PhoneNo.Substring(company.PhoneNo.Length, -5))).ToList().Count();


            int websiteMatchCount = _context.Companies.Where(c => c.Website.Contains(company.Website)).ToList().Count();

            int first5websiteCharsMatchCount = _context.Companies.Where(c => c.Website.StartsWith(company.Website.Substring(0, 5))).ToList().Count();
            //Last 3 letters search count
            int last5websiteCharsMatchCount = _context.Companies.Where(c => c.Website.StartsWith(company.Website.Substring(company.Website.Length, -5))).ToList().Count();




            int RegistratioNoMatchCount = _context.Companies.Where(c => c.CommercialRegistrationNo.Contains(company.CommercialRegistrationNo)).ToList().Count();

            int first5RegistratioNoCharsMatchCount = _context.Companies.Where(c => c.CommercialRegistrationNo.StartsWith(company.CommercialRegistrationNo.Substring(0, 5))).ToList().Count();
            //Last 3 letters search count
            int last5RegistratioNoCharsMatchCount = _context.Companies.Where(c => c.CommercialRegistrationNo.StartsWith(company.CommercialRegistrationNo.Substring(company.CommercialRegistrationNo.Length, -5))).ToList().Count();



            return "";
        }
    }
}
