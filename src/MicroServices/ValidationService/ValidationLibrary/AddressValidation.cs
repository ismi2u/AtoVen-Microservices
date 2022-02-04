using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ValidationLibrary
{
    public class AddressValidation
    {
        private readonly string AddressValidatorAPIKey = "av-46947951bfd581f1e1d6c206b2a8f9e0";
        private readonly string APIURL = "https://api.address-validator.net/api/verify";
        public string ValidateStreetAddress(AddressValidationInputs address)
        {
            
            HttpClient client = new HttpClient();

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("StreetAddress", address.HouseNo + " " + address.Street));
            postData.Add(new KeyValuePair<string, string>("City", address.City));
            postData.Add(new KeyValuePair<string, string>("PostalCode", address.PostalCode));
            postData.Add(new KeyValuePair<string, string>("State", address.Region));
            postData.Add(new KeyValuePair<string, string>("CountryCode", address.Country));
            postData.Add(new KeyValuePair<string, string>("Locale", address.Language));
            postData.Add(new KeyValuePair<string, string>("APIKey", AddressValidatorAPIKey));

            HttpContent content = new FormUrlEncodedContent(postData);

            HttpResponseMessage result = client.PostAsync(APIURL, content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;
            var res = JsonSerializer.Deserialize<object>(resultContent);
            string formattedaddress = "";

            //if (res.Status.Equals("VALID"))
            //{
            //    formattedaddress = res.FormattedAddress;
            //}
            //else
            //{
            //    formattedaddress = "Address is not Valid";
            //}
            //return formattedaddress;

            return "";
        }
    }


    public class AddressValidationInputs
    {
        public string HouseNo { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }

    }

}
