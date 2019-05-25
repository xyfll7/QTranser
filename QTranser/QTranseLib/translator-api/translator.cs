using RestSharp;
using Newtonsoft.Json;

namespace QTranser.QTranseLib
{

    public class Translator
    {

        public string GetJson(string str)
        {
            var mac = new GetMac();
            var client = new RestClient("http://47.95.197.94:2399");
            var request = new RestRequest("/api/transer", Method.POST);
            var postdata = new words()
            {
                mac = mac.GetMacAddress(),
                word = str,
                githubID = Properties.Settings.Default.GitHubUserId
            };
            var json = JsonConvert.SerializeObject(postdata);
            request.AddJsonBody(json);
            IRestResponse response = client.Execute(request);

            // Internal Server Error
            if(response.Content == "Internal Server Error")
            {
                response.Content = "{ \"baidu\": \"Internal Server Error\" }";
            }
            if(response.Content == "")
            {
                response.Content = "{ \"baidu\": \"没连网吗？\" }";
            }
            return response.Content;
        }
    }
}
