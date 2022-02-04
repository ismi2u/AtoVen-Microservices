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
    public class ContactRepository : IContactRepository
    {
        private readonly AtovenDbContext _context;
        public ContactRepository(AtovenDbContext context)
        {
            _context = context;
        }

        public Task<Contact> AddContact(Contact Contact)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> DeleteContact(int ContactId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contact>> GetAllContactsByCompany(string ContactName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contact>> GetAllContactsByCompanyId(int ContactId)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> GetContactById(int ContactId)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> UpdateContact(Contact Contact)
        {
            throw new NotImplementedException();
        }
    }
}
