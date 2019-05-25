using Newtonsoft.Json.Linq;
using QTranser.ViewModles;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace QTranser.QTranseLib
{
    public class Update
    {
        public Boolean isDownLoad { get; set; } = false;

        private String Version { get; set; } = "1.0.1";
        private String NewVersion { get; set; }
        private String UpdateUrl { get; set; }
        private String ClientUrl { get; set; }
        private String RequestUrl { get; set; }
        private int requestTimes { get; set; } = 0;

        public async void GetNewVersion(MainViewModel Mvvm)
        {
            try
            {
                if(requestTimes < 4)
                {
                    UpdateInfoProcess();
                    requestTimes++;
                }
                if (NewVersion != Version && NewVersion != null)
                {
                    if (isDownLoad && Mvvm.Visibility0 != Visibility.Visible)
                    {
                        Mvvm.Visibility1 = Visibility.Visible;
                    }
                    else
                    {
                        Boolean download = await Task.Run(() => downLoad());
                        if (download)
                        {
                            Mvvm.StrQ = "";
                            Mvvm.Visibility1 = Visibility.Visible;
                            isDownLoad = true;
                        }
                    }
                }
            }
            catch { }
        }

        public Boolean downLoad()
        {
            string path = @"C:\Program Files\QTranser\QTranser_Installer.msi";
            var client = new RestClient(ClientUrl);
            var request = new RestRequest(RequestUrl, Method.GET);
            client.DownloadData(request).SaveAs(path);
            return true;
        }

        public void UpdateInfoProcess()
        {
            dynamic updateInfo = JToken.Parse(UpdateInfo()) as dynamic;
            NewVersion = updateInfo.newVersion;
            UpdateUrl = updateInfo.updateUrl;
            ClientUrl = updateInfo.clientUrl;
            RequestUrl = updateInfo.requestUrl;
        }

        private string UpdateInfo()
        {
            var client = new RestClient("http://47.95.197.94:2399");
            var request = new RestRequest("/api/transer/update", Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
