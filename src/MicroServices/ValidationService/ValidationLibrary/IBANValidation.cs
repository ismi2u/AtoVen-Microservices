using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationLibrary
{
    public class IBANValidation
    {
        private readonly string IBANAPIValidatorKey = "a1cd8ac82a2940d9bb3f9fa20d84261ddd6cd538";

        public string ValidateIBAN(string IbanNumber)
        {
            //https://api.ibanapi.com/v1/validate/EE471000001020145685?api_key=API_KEY
            //Sample IBAN number = DE56501201000000484637
            string APIUrl = "https://api.ibanapi.com/v1/validate/" + IbanNumber + "?api_key=" + IBANAPIValidatorKey;

            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.PostAsync(APIUrl, null).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            string resultMessage = JsonConvert.DeserializeObject<IBANRootObject>(resultContent).message;

            if (resultMessage == "Valid IBAN Number")
            {
                     
                return "Valid IBAN Number";
            }
            else
            {
                return "Invalid IBAN Number";
            }


        }
    }

    public class IBANRootObject
    {
        public int result { get; set; }
        public string message { get; set; }
        public Validation[] validations { get; set; }
        public int expremental { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string country_code { get; set; }
        public string iso_alpha3 { get; set; }
        public string country_name { get; set; }
        public string currency_code { get; set; }
        public string sepa_member { get; set; }
        public Sepa sepa { get; set; }
        public string bban { get; set; }
        public string bank_account { get; set; }
        public BankData bank { get; set; }
    }

    public class Sepa
    {
        public string sepa_credit_transfer { get; set; }
        public string sepa_direct_debit { get; set; }
        public string sepa_sdd_core { get; set; }
        public string sepa_b2b { get; set; }
        public string sepa_card_clearing { get; set; }
    }

    public class BankData
    {
        public string bank_name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string bic { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    public class Validation
    {
        public int result { get; set; }
        public string message { get; set; }
    }



}
