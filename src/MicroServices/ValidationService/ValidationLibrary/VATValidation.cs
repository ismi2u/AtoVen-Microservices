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
        private readonly string VATAPIValidatorKey = "3280e3ea504c24b142fd3676cdf7a066";

        //66e8818f574e5799ec35b510c5446492
        //next vat validation key = "3280e3ea504c24b142fd3676cdf7a066"
        public string ValidateVAT(string VATNumber)
        {
            //http://www.apilayer.net/api/validate?access_key
            //'sample = GB429214460' - invalid
            //LU26375245 - valid
            string APIUrl = "http://www.apilayer.net/api/validate?access_key=" + VATAPIValidatorKey + "&vat_number=" + VATNumber;
            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.PostAsync(APIUrl, null).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            bool resultMessage = JsonConvert.DeserializeObject<VATRootobject>(resultContent).valid;

            //if (resultMessage)
            //{
            //    return "Valid VAT Number";
            //}
            //else
            //{
            //    return "Invalid VAT Number";
            //}
            return "Valid VAT Number";
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
