using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            const string VATAPIValidatorKey = "579db579789b253ddbc7708f84990f18";
            //'sample = GB429214460'
            string APIUrl = "http://www.apilayer.net/api/validate?access_key=" + VATAPIValidatorKey + "&vat_number=" + VATNumber;
            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.PostAsync(APIUrl, null).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;
            //VATAPIValidationResult VATresult = JsonSerializer.Deserialize<VATAPIValidationResult>(resultContent);
            return resultContent;

        }

        [HttpPost]
        [ActionName("IBANValidator")]
        public async Task<ActionResult<string>> IBANValidator(string IbanNumber)
        {
            //https://api.ibanapi.com/v1/validate/EE471000001020145685?api_key=API_KEY
            const string IBANAPIValidatorKey = "a1cd8ac82a2940d9bb3f9fa20d84261ddd6cd538";
            //ICICIBank IBAN number = DE56501201000000484637
            string APIUrl =  "https://api.ibanapi.com/v1/validate/" + IbanNumber + "?api_key=" + IBANAPIValidatorKey;

            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.PostAsync(APIUrl, null).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;
            return resultContent;

        }

        [HttpPost]
        [ActionName("AddressValidator")]
        public async Task<ActionResult<string>> AddressValidator(AddressValidationInputs addressValidationInputs)
        {
            const string APIURL = "https://api.address-validator.net/api/verify";
            string APIKey = "av-46947951bfd581f1e1d6c206b2a8f9e0";

            HttpClient client = new HttpClient();

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("StreetAddress", addressValidationInputs.HouseNo + " " + addressValidationInputs.Street));
            postData.Add(new KeyValuePair<string, string>("City", addressValidationInputs.City));
            postData.Add(new KeyValuePair<string, string>("PostalCode", addressValidationInputs.PostalCode));
            postData.Add(new KeyValuePair<string, string>("State", addressValidationInputs.Region));
            postData.Add(new KeyValuePair<string, string>("CountryCode", addressValidationInputs.Country));
            postData.Add(new KeyValuePair<string, string>("Locale", addressValidationInputs.Language));
            postData.Add(new KeyValuePair<string, string>("APIKey", APIKey));

            HttpContent content = new FormUrlEncodedContent(postData);

            HttpResponseMessage result = client.PostAsync(APIURL, content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;
            //AddressAPIValidationResult res = JsonSerializer.Deserialize<AddressAPIValidationResult>(resultContent);
            //string formattedaddress = "";

            //if (res.Status.Equals("VALID"))
            //{
            //    formattedaddress = res.FormattedAddress;
            //}
            //else
            //{
            //    formattedaddress = "Address is not Valid";
            //}
            return resultContent;
        }
    }
}
