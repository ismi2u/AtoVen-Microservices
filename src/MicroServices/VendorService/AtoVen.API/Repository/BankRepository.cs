using Microsoft.EntityFrameworkCore;
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
    public class BankRepository : IBankRepository
    {
        private readonly AtovenDbContext _context;
        public BankRepository(AtovenDbContext context)
        {
             _context = context;
        }

        public Task<Bank> AddBank(Bank Bank)
        {
            throw new NotImplementedException();
        }

        public Task<Bank> DeleteBank(int BankId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Bank>> GetAllBanks(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Bank>> GetAllBanksByCompany(string CompanyName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Bank>> GetAllBanksByCompanyId(int CompanyId)
        {
            throw new NotImplementedException();
        }

        public Task<Bank> GetBankById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Bank> UpdateBank(Bank Bank)
        {
            throw new NotImplementedException();
        }
    }
}
