using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtoVen.API.Entities;

namespace AtoVen.API.Repository
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContactsByCompany(string ContactName);
        Task<IEnumerable<Contact>> GetAllContactsByCompanyId(int ContactId);
        Task<Contact> GetContactById(int ContactId);

        Task<Contact> AddContact(Contact Contact);
        Task<Contact> UpdateContact(Contact Contact);
        Task<Contact> DeleteContact(int ContactId);

    }
}
