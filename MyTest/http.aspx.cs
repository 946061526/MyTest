using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Http;
using System.Net.Http;
using Flurl;
using Flurl.Http;
using System.Configuration;
using System.Threading.Tasks;

namespace MyTest
{
    public partial class http : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            var url = "http://www.some-api.com"
                .AppendPathSegment( "endpoint" )
                .SetQueryParams( new
                {
                    api_key = ConfigurationManager.AppSettings["SomeApiKey"],
                    max_results = 20,
                    q = "Don't worry, I'll get encoded!"
                } )
                .SetFragment( "after-hash" );

        }

        private async void HttpPost()
        {
            HttpClient httpClient = new HttpClient();

            Dictionary<string, string> contentDict = new Dictionary<string, string>()
                {
                    {"merchantid","merchantid"},
                    {"data","data"},
                    {"timestamp","timestamp"},
                    {"sign","sign"}
                };
            var content = new FormUrlEncodedContent( contentDict );

            string url = "https://123.207.247.101/pay/unionpay/authelement";
            HttpResponseMessage response = await httpClient.PostAsync( url, content );
            response.EnsureSuccessStatusCode();
            string resultStr = await response.Content.ReadAsStringAsync();
            Console.WriteLine( resultStr );



        }

        public async static Task<HttpResponseMessage> PostJsonAsync()
        {

            return await "http://api.foo.com".PostJsonAsync( new
                {
                    a = 1,
                    b = 2
                } );

            return await "http://site.com/login".PostUrlEncodedAsync( new
              {
                  user = "user",
                  pass = "pass"
              } );
        }

    }
}