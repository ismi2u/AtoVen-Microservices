using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtoVen.API.Repository;
using AtoVen.API.Entities;
using AtoVen.API.Data;

namespace AtoVen.API.Repository
{
    public class CompanyRepository : ICompanyRepository
    {

        private readonly AtoVenDbContext _context;
        public CompanyRepository(AtoVenDbContext context)
        {
            _context = context;
        }

        public async Task<Company> AddCompany(Company Company)
        {
            await _context.Companies.AddAsync(Company);
            _context.SaveChanges();
            return Company;
        }

        public async Task<Company> DeleteCompany(int CompanyId)
        {
            Company company = _context.Companies.Find(CompanyId);
            if (CompanyId != 0)
            {
                _context.Companies.Remove(company);
               await _context.SaveChangesAsync();
            }
            return company;
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            List<Company> listCompanies = _context.Companies.ToList();
            return listCompanies;
        }

        public async Task<Company> GetCompanyById(int CompanyId)
        {
            return await _context.Companies.FindAsync(CompanyId);
        }

        public Task<Company> UpdateCompany(Company Company)
        {
            throw new NotImplementedException();
        }
    }
}
