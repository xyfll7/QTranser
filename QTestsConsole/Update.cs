using Newtonsoft.Json.Linq;
using QTranser.QTranseLib.Update;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTests
{
    class Update
    {
        public static void Main()
        {
            IsUpdate();
            Console.WriteLine(updateinfo.IsUpdate);
            Console.WriteLine(updateinfo.UpdateUrl);
            downLoad();


            Console.ReadKey();
        }

        /*async*/ public  static  void downLoad()
        {
            string path = @"C:\Users\Administrator\Desktop\QTranser.exe";
            var client = new RestClient("https://raw.githubusercontent.com");
            var request = new RestRequest("/xyfll7/QTranser/master/QTranser_Installer/Debug/QTranser_Installer.msi", Method.GET);
            //var client = new RestClient("https://dl.softmgr.qq.com");
            //var request = new RestRequest("/original/im/QQ9.1.1.24953.exe", Method.GET);
            Console.WriteLine("开始下载");
            client.DownloadData(request).SaveAs(path);
            Console.WriteLine("下载结束");

            //IRestResponse response = await Task.Run(() => client.Execute(request) );

        }


        public struct updateinfo
        {
            public static String IsUpdate { get; set; }
            public static String UpdateUrl { get; set; }
        }
 
        public static void IsUpdate()
        {
            dynamic result = JToken.Parse(GetUpdate()) as dynamic;
            updateinfo.IsUpdate = result.version;
            updateinfo.UpdateUrl = result.updateUrl;
    
        }

        public static string GetUpdate()
        {
            var client = new RestClient("http://localhost:2399");
            var request = new RestRequest("/api/transer/update", Method.GET);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
