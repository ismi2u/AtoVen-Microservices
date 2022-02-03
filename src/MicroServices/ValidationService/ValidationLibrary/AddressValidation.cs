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
        private string _HouseNo { get; set; }
        private string _Street { get; set; }
        private string _City { get; set; }
        private string _Region { get; set; }
        private string _PostalCode { get; set; }
        private string _Country { get; set; }
        private string _CountryCode { get; set; }
        private string _Language { get; set; }
        private string _Locale { get; set; }
        private string _AddressValidatorAPIKey { get; set; }

        public AddressValidation(string HouseNo, string Street, string City,
                            string Region, string PostalCode, string Country,
                            string Language, string AddressValidatorAPIKey)
        {
            _HouseNo = HouseNo;
            _Street = Street;
            _City = City;
            _Region = Region;
            _PostalCode = PostalCode;
            _Country = Country;
            _Language = Language;
            _AddressValidatorAPIKey = AddressValidatorAPIKey;
        }

        public string GetStreetAddress()
        {
            string StreetAddress = _HouseNo + " " + _Street;

            return StreetAddress;
        }

        public string GetCity()
        {
            return _City;
        }

        public string GetRegion()
        {
            return _Region;//region is state
        }
        public string GetPostalCode()
        {
            return _PostalCode;
        }
        public string GetCountry()
        {
            return _Country;
        }

        public string GetCountryCode()
        {
            return _Country; //Convert the country  to country code
        }
        public string GetLanguage()
        {
            return _Language;
        }

        public string GetLocale()
        {
            return _Language; //convet language to locale
        }

        public string GetAddressValidatorAPIKey()
        {
            return _AddressValidatorAPIKey;
        }
        public string ValidateStreetAddress()
        {
            const string APIURL = "https://api.address-validator.net/api/verify";
            HttpClient client = new HttpClient();

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("StreetAddress", GetStreetAddress()));
            postData.Add(new KeyValuePair<string, string>("City", GetCity()));
            postData.Add(new KeyValuePair<string, string>("PostalCode", GetPostalCode()));
            postData.Add(new KeyValuePair<string, string>("State", GetRegion()));
            postData.Add(new KeyValuePair<string, string>("CountryCode", GetCountryCode()));
            postData.Add(new KeyValuePair<string, string>("Locale", GetLocale()));
            postData.Add(new KeyValuePair<string, string>("APIKey", GetAddressValidatorAPIKey()));

            HttpContent content = new FormUrlEncodedContent(postData);

            HttpResponseMessage result = client.PostAsync(APIURL, content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;
            AddressAPIValidationResult res = JsonSerializer.Deserialize<AddressAPIValidationResult>(resultContent);
            string formattedaddress = "";

            if (res.Status.Equals("VALID"))
            {
                formattedaddress = res.FormattedAddress;
            }
            else
            {
                formattedaddress = "Address is not Valid";
            }
            return formattedaddress;
        }
    }


    public class AddressAPIValidationResult
    {
        public string Status { get; set; }
        public string FormattedAddress { get; set; }
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
