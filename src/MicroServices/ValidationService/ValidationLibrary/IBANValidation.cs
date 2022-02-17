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
        private readonly string IBANAPIValidatorKey = "a6c6bbd034c7e4a262c09de7eb1b1c0c4a5ebf16";
        //next IBAN valid key "a6c6bbd034c7e4a262c09de7eb1b1c0c4a5ebf16"
        public string ValidateIBAN(string IbanNumber)
        {
            //https://api.ibanapi.com/v1/validate/EE471000001020145685?api_key=a6c6bbd034c7e4a262c09de7eb1b1c0c4a5ebf16
            //Sample IBAN number = DE56501201000000484637, DE89370400440532013000


            //Free API string APIUrl = "https://openiban.com/validate/456456456456?getBIC=true&validateBankCode=true" ;
            string APIUrl = "https://openiban.com/validate/" + IbanNumber + "?getBIC=true&validateBankCode=true" ;

            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.GetAsync(APIUrl).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            bool resultMessage = JsonConvert.DeserializeObject<Rootobject>(resultContent).valid;

            if (resultMessage)
            {

                return "Valid IBAN Number";
            }
            else
            {
                return "Invalid IBAN Number";
            }

            //return "Valid IBAN Number";
        }
    }

    //public class IBANRootObject
    //{
    //    public int result { get; set; }
    //    public string message { get; set; }
    //    public Validation[] validations { get; set; }
    //    public int expremental { get; set; }
    //    public Data data { get; set; }
    //}

    //public class Data
    //{
    //    public string country_code { get; set; }
    //    public string iso_alpha3 { get; set; }
    //    public string country_name { get; set; }
    //    public string currency_code { get; set; }
    //    public string sepa_member { get; set; }
    //    public Sepa sepa { get; set; }
    //    public string bban { get; set; }
    //    public string bank_account { get; set; }
    //    public BankData bank { get; set; }
    //}

    //public class Sepa
    //{
    //    public string sepa_credit_transfer { get; set; }
    //    public string sepa_direct_debit { get; set; }
    //    public string sepa_sdd_core { get; set; }
    //    public string sepa_b2b { get; set; }
    //    public string sepa_card_clearing { get; set; }
    //}

    //public class BankData
    //{
    //    public string bank_name { get; set; }
    //    public string phone { get; set; }
    //    public string address { get; set; }
    //    public string bic { get; set; }
    //    public string city { get; set; }
    //    public string state { get; set; }
    //    public string zip { get; set; }
    //}

    //public class Validation
    //{
    //    public int result { get; set; }
    //    public string message { get; set; }
    //}


    public class Rootobject
    {
        public bool valid { get; set; }
        public string[] messages { get; set; }
        public string iban { get; set; }
        public Bankdata bankData { get; set; }
        public Checkresults checkResults { get; set; }
    }

    public class Bankdata
    {
        public string bankCode { get; set; }
        public string name { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string bic { get; set; }
    }

    public class Checkresults
    {
        public bool bankCode { get; set; }
    }


}
