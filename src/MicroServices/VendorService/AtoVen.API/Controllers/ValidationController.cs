using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using ValidationLibrary;

namespace AtoVen.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        [HttpPost]
        [ActionName("VATValidator")]
        public async Task<ActionResult<string>> VATValidator(string VATNumber)
        {
            VATValidation validation = new VATValidation();
            return validation.ValidateVAT(VATNumber);
        }

        [HttpPost]
        [ActionName("IBANValidator")]
        public async Task<ActionResult<string>> IBANValidator(string IbanNumber)
        {
            IBANValidation validation = new IBANValidation();
            return validation.ValidateIBAN(IbanNumber);
        }

        [HttpPost]
        [ActionName("AddressValidator")]
        public async Task<ActionResult<string>> AddressValidator(AddressValidationInputs address)
        {
            AddressValidation addressvalidation = new AddressValidation();
            return addressvalidation.ValidateStreetAddress(address);
        }
    }
}
