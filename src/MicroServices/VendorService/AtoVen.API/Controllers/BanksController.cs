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

namespace AtoVen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly AtovenDbContext _context;

        public BanksController(AtovenDbContext context)
        {
            _context = context;
        }

        // GET: api/Banks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankDTO>>> GetBanks()
        {
            List<BankDTO> ListBankDTOs = new();

            var ListBanks = await _context.Banks.ToListAsync();

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

            return ListBankDTOs;
        }

        // GET: api/Banks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankDTO>> GetBank(int id)
        {

            var bank = await _context.Banks.FindAsync(id);

            if (bank == null)
            {
                return NotFound();
            }

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

            return bankDTO;

        }

        // PUT: api/Banks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBank(int id, BankDTO bank)
        {
            if (id != bank.Id)
            {
                return BadRequest();
            }

            _context.Entry(bank).State = EntityState.Modified;

            try
            {
                var updatebank = await _context.Banks.FindAsync(bank.Id);

                updatebank.Id = bank.Id;
                updatebank.Country = bank.Country;
                updatebank.CompanyID = bank.CompanyID;
                updatebank.BankKey = bank.BankKey;
                updatebank.BankName = bank.BankName;
                updatebank.SwiftCode = bank.SwiftCode;
                updatebank.BankAccount = bank.BankAccount;
                updatebank.AccountHolderName = bank.AccountHolderName;
                updatebank.IBAN = bank.IBAN;
                updatebank.Currency = bank.Currency;

                _context.Banks.Update(updatebank);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankExists(id))
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

        // POST: api/Banks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bank>> PostBank(BankDTO bank)
        {
            Bank newBank = new Bank();

            newBank.Country = bank.Country;
            newBank.CompanyID = bank.CompanyID;
            newBank.BankKey = bank.BankKey;
            newBank.BankName = bank.BankName;
            newBank.SwiftCode = bank.SwiftCode;
            newBank.BankAccount = bank.BankAccount;
            newBank.AccountHolderName = bank.AccountHolderName;
            newBank.IBAN = bank.IBAN;
            newBank.Currency = bank.Currency;

            _context.Banks.Add(newBank);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBank", new { id = bank.Id }, newBank);
        }

        // DELETE: api/Banks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBank(int id)
        {
            var bank = await _context.Banks.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }

            _context.Banks.Remove(bank);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankExists(int id)
        {
            return _context.Banks.Any(e => e.Id == id);
        }
    }
}
