using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationLibrary
{
    public class VATValidation
    {
        private readonly string VATAPIValidatorKey = "488fad4dc8c94652a36dbb1dba4bdd83";
        //old key "b11f2b45b9e18f20146e731f3abdf2be"

        //new xx
        public string ValidateVAT(string VATNumber)
        {
            //http://www.apilayer.net/api/validate?access_key
            //'sample = GB429214460' - invalid
            //LU26375245 - valid
            string APIUrl = "http://www.apilayer.net/api/validate?access_key=" + VATAPIValidatorKey + "&vat_number=" + VATNumber;
            //string APIUrl = "http://www.apilayer.net/api/validate?access_key=6cc96f7c59d5c0fd8c255c31b743ddbf&vat_number=LU26375245";
            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.PostAsync(APIUrl, null).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            bool resultMessage = JsonConvert.DeserializeObject<VATRootobject>(resultContent).valid;

            if (resultMessage)
            {
                return "Valid VAT Number";
            }
            else
            {
                return "Invalid VAT Number";
            }
            //return "Valid VAT Number";
        }

    }

    public class VATRootobject
    {
        public bool valid { get; set; }
        public string database { get; set; }
        public bool format_valid { get; set; }
        public string query { get; set; }
        public string country_code { get; set; }
        public string vat_number { get; set; }
        public string company_name { get; set; }
        public string company_address { get; set; }
    }


}
