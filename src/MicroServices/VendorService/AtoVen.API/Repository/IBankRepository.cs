using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtoVen.API.Entities;

namespace AtoVen.API.Repository  
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> GetAllBanksByCompany(string CompanyName);
        Task<IEnumerable<Bank>> GetAllBanksByCompanyId(int CompanyId);

        Task<Bank> GetBankById(int Id);
        Task<IEnumerable<Bank>> GetAllBanks (int Id);
        Task<Bank> AddBank(Bank Bank);
        Task<Bank> UpdateBank(Bank Bank);
        Task<Bank> DeleteBank(int BankId);


    }
}
