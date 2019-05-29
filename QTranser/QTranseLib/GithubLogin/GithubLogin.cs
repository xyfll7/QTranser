using MockHttpServer;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace QTranser.QTranseLib
{
    public class GithubLogin
    {
        private MockServer Server { get; set; }
        private int TestPort { get; set; } = 2399;
        private string Token { get; set; }

        public static void InitGitHubUserName()
        {
            QTranse.Mvvm.UserName = Properties.Settings.Default.GitHubUserName;
        }

        public void Button_Click()
        {
            Process.Start("https://github.com/login/oauth/authorize?client_id=2bad7ade7264aaafee4b&scope=user:email");
            GetToken();
        }
        public void GetToken()
        {
            try
            {
                Server = new MockServer(TestPort, RequestHandlers());
            }
            catch(Exception)
            {
                Server.Dispose();
                Server = new MockServer(TestPort, RequestHandlers());
            }
            
        }
        private List<MockHttpHandler> RequestHandlers()
        {
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler("/github", "GET", (req, rsp, prm) => {
                    try
                    {
                        rsp.Header("Access-Control-Allow-Origin", req.Headers.Get("Origin"));
                        Token = prm["access_token"];
                        var userName = GetUserInfo();
                        var buffer = Encoding.UTF8.GetBytes(userName);
                        rsp.Content(buffer);
                        Server.Dispose();
                    }
                    catch (Exception err)
                    {
                        Server.Dispose();
                        MessageBox.Show(err.ToString());
                    }

                })
            };
            return requestHandlers;
        }


        private string GetUserInfo()
        {
            var client = new RestClient(Properties.Settings.Default.fyHost);
            var result = client.Execute(new RestRequest($"logins/user?access_token={Token}", Method.GET));
            Properties.Settings.Default.GitHubUserInfo = result.Content;

            dynamic transResult = JToken.Parse(Properties.Settings.Default.GitHubUserInfo) as dynamic;

            Properties.Settings.Default.GitHubUserName = transResult.name.ToString();
            Properties.Settings.Default.GitHubUserId = transResult.id.ToString();
           
            Properties.Settings.Default.Save();
            QTranse.Mvvm.UserName = transResult.name.ToString();
            return transResult.name.ToString();
        }
    }
}
