using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DataService.DataContext;
using DataService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataService.DataContext
{
    public class DuplicatesValidation
    {

        private readonly AtoVenDbContext _context;
        private readonly SchwarzDbContext _schwarzContext;

        public DuplicatesValidation(AtoVenDbContext context,
                                    SchwarzDbContext schwarzContext)
        {
            _context = context;
            _schwarzContext = schwarzContext;
        }


        //1. establish dbcontext
        //    2. 


        public async Task<bool> IsDuplicate(Company company)
        {
            //Checks both Local DB and Schwarz Database

            List<Company> ListOfCompanies = new();

            //Checks both Local DB and Schwarz Database
            //using (TransactionScope scope = new TransactionScope())
            //{
            List<Bank> listofBanks = new();
            string[] words = company.CompanyName.Split(' ');


            //using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
            //{
            ///////////////////////////////////////
            ///  IBAN number Duplicate Search  ////
            ///////////////////////////////////////

            listofBanks = _context.Banks.Where(b => b.CompanyID == company.Id).ToList();


            foreach (Bank bank in listofBanks)
            {
                List<int> compIDs = _context.Banks.Where(b => b.IBAN == bank.IBAN).Select(s => s.CompanyID).ToList();
                ListOfCompanies.AddRange(_context.Companies.Where(c => compIDs.Contains(c.Id)).ToList());
            }

            ///////////////////////////////////////
            /// VAT number Duplicate Search ////
            ///////////////////////////////////////

            ListOfCompanies.AddRange(_context.Companies.Where(c => c.VatNo.Contains(company.VatNo)).ToList());



            ///////////////////////////////////////
            /// Company Name - Duplicate Search ///
            ///////////////////////////////////////

            /// Break Word Search

            foreach (string word in words)
            {
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.CompanyName.Contains(word)).ToList());
            }
            //-- First 5 letters search count
            if (company.CompanyName.Length < 5)
            {
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.CompanyName.Contains(company.CompanyName)).ToList());
            }
            else
            {
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.CompanyName.Contains(company.CompanyName.Substring(0, 5))).ToList());
            }


            ///////////////////////////////////////
            /// Mobile number Duplicate Search ////
            ///////////////////////////////////////

            if (company.MobileNo != null)
            {
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.MobileNo.Contains(company.MobileNo)).ToList());
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.MobileNo.StartsWith(company.MobileNo.Substring(0, 5))).ToList());
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.MobileNo.EndsWith(company.MobileNo.Substring(company.MobileNo.Length - 5))).ToList());
            }

            ///////////////////////////////////////
            /// Phone number Duplicate Search  ////
            ///////////////////////////////////////
            if (company.PhoneNo != null)
            {
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.PhoneNo.Contains(company.PhoneNo)).ToList());
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.PhoneNo.StartsWith(company.PhoneNo.Substring(0, 5))).ToList());
                ListOfCompanies.AddRange(_context.Companies.Where(c => c.PhoneNo.EndsWith(company.PhoneNo.Substring(company.PhoneNo.Length - 5))).ToList());
            }
            ///////////////////////////////////////
            ///     Website Duplicate Search   ////
            ///////////////////////////////////////

            //ListOfCompanies.AddRange(_context.Companies.Where(c => c.Website.Contains(new Uri(company.Website).Host)).ToList());

            ///////////////////////////////////////
            ///     Email Duplicate Search   ////
            ///////////////////////////////////////

            ListOfCompanies.AddRange(_context.Companies.Where(c => c.Email.Contains(company.Email)).ToList());

            ///////////////////////////////////////
            /// Registration No Duplicate Search //
            ///////////////////////////////////////

            ListOfCompanies.AddRange(_context.Companies.Where(c => c.CommercialRegistrationNo.Contains(company.CommercialRegistrationNo)).ToList());

            ListOfCompanies = ListOfCompanies.Distinct().ToList();
            //    await AtoVenDbContextTransaction.CommitAsync();
            //}



            //DONT REMOVE THE BELOW CODE
            ListOfCompanies = ListOfCompanies.Distinct().ToList();

            ListOfCompanies.Remove(company);

            if (ListOfCompanies.Count > 0)
            {
                return true;
            }

            return false;
        }




        public async Task<IEnumerable<Company>> CheckDuplicates(Company company)
        {

            List<Company> ListOfCompanies = new();

            //Checks both Local DB and Schwarz Database
            //using (TransactionScope scope = new TransactionScope())
            //{
            List<Bank> listofBanks = new();
            string[] words = company.CompanyName.Split(' ');


            using (var AtoVenDbContextTransaction = _context.Database.BeginTransaction())
            {
                ///////////////////////////////////////
                ///  IBAN number Duplicate Search  ////
                ///////////////////////////////////////

                listofBanks = _context.Banks.Where(b => b.CompanyID == company.Id).ToList();


                foreach (Bank bank in listofBanks)
                {
                    List<int> compIDs = _context.Banks.Where(b => b.IBAN == bank.IBAN).Select(s => s.CompanyID).ToList();
                    ListOfCompanies.AddRange(_context.Companies.Where(c => compIDs.Contains(c.Id)).ToList());
                }

                ///////////////////////////////////////
                /// VAT number Duplicate Search ////
                ///////////////////////////////////////

                ListOfCompanies.AddRange(_context.Companies.Where(c => c.VatNo.Contains(company.VatNo)).ToList());



                ///////////////////////////////////////
                /// Company Name - Duplicate Search ///
                ///////////////////////////////////////

                /// Break Word Search

                foreach (string word in words)
                {
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.CompanyName.Contains(word)).ToList());
                }
                //-- First 5 letters search count
                if (company.CompanyName.Length < 5)
                {
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.CompanyName.Contains(company.CompanyName)).ToList());
                }
                else
                {
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.CompanyName.Contains(company.CompanyName.Substring(0, 5))).ToList());
                }


                ///////////////////////////////////////
                /// Mobile number Duplicate Search ////
                ///////////////////////////////////////

                if (company.MobileNo != null)
                {
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.MobileNo.Contains(company.MobileNo)).ToList());
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.MobileNo.StartsWith(company.MobileNo.Substring(0, 5))).ToList());
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.MobileNo.EndsWith(company.MobileNo.Substring(company.MobileNo.Length - 5))).ToList());
                }

                ///////////////////////////////////////
                /// Phone number Duplicate Search  ////
                ///////////////////////////////////////
                if (company.PhoneNo != null)
                {
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.PhoneNo.Contains(company.PhoneNo)).ToList());
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.PhoneNo.StartsWith(company.PhoneNo.Substring(0, 5))).ToList());
                    ListOfCompanies.AddRange(_context.Companies.Where(c => c.PhoneNo.EndsWith(company.PhoneNo.Substring(company.PhoneNo.Length - 5))).ToList());
                }
                ///////////////////////////////////////
                ///     Website Duplicate Search   ////
                ///////////////////////////////////////

                //ListOfCompanies.AddRange(_context.Companies.Where(c => c.Website.Contains(new Uri(company.Website).Host)).ToList());

                ///////////////////////////////////////
                ///     Email Duplicate Search   ////
                ///////////////////////////////////////

                ListOfCompanies.AddRange(_context.Companies.Where(c => c.Email.Contains(company.Email)).ToList());

                ///////////////////////////////////////
                /// Registration No Duplicate Search //
                ///////////////////////////////////////

                ListOfCompanies.AddRange(_context.Companies.Where(c => c.CommercialRegistrationNo.Contains(company.CommercialRegistrationNo)).ToList());

                ListOfCompanies = ListOfCompanies.Distinct().ToList();
                await AtoVenDbContextTransaction.CommitAsync();
            }


            // ListOfCompanies.Remove(company);

            List<Company> distinctCompanies = ListOfCompanies;

            return distinctCompanies.Distinct();
        }
    }
}
