using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationLibrary
{

    // EMail Validation API Key: 64a55d465c764754533313377e5e6167
    public class EmailValidation
    {

        //http://apilayer.net/api/check?access_key=64a55d465c764754533313377e5e6167&email=ikingkong@rediffmail.com&smpt=1
        private readonly string EmailAPIValidatorKey = "64a55d465c764754533313377e5e6167";
        //next email valid key "not created"
        public string ValidateEmail(string emailAddress)
        {
            //Sample email  = test@gmail.com

            string APIUrl = "http://apilayer.net/api/check?access_key=" + EmailAPIValidatorKey + "&email=" + emailAddress + "&smpt=1";


            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.PostAsync(APIUrl, null).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            bool resultMessage = JsonConvert.DeserializeObject<EmailRootobject>(resultContent).smtp_check;

            //if (resultMessage)
            //{

            //    return "Valid Email Address";
            //}
            //else
            //{
            //    return "Invalid Email Address";
            //}

            return "Valid Email Address";
        }


        public class EmailRootobject
        {
            public string email { get; set; }
            public string did_you_mean { get; set; }
            public string user { get; set; }
            public string domain { get; set; }
            public bool format_valid { get; set; }
            public bool mx_found { get; set; }
            public bool smtp_check { get; set; }
            public bool catch_all { get; set; }
            public bool role { get; set; }
            public bool disposable { get; set; }
            public bool free { get; set; }
            public float score { get; set; }
        }

    }
}
