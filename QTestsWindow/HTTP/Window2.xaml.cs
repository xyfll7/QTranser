using MockHttpServer;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace QTestsWindow
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        private MockServer server { get; set; }
        private int TestPort = 2399;
        private string Token { get; set; }
        private string UserInfo { get; set; }

        public void GetToken()
        {
            server = new MockServer(TestPort, RequestHandlers());
        }
        private List<MockHttpHandler> RequestHandlers()
        {
            var requestHandlers = new List<MockHttpHandler>()
            {
                new MockHttpHandler("/github", "GET", (req, rsp, prm) => {

                    rsp.Header("Access-Control-Allow-Origin", req.Headers.Get("Origin"));
                    var buffer = Encoding.UTF8.GetBytes("来了老弟~！");
                    rsp.Content(buffer);

                    Token = prm["access_token"];
                    getUserInfo();
                    server.Dispose();
                })
            };
            return requestHandlers;
        }




        private void getUserInfo()
        {
            //https://api.github.com/user?access_token=${access_token}
            var client = new RestClient($"https://api.github.com/");
            var result = client.Execute(new RestRequest($"user?access_token={Token}", Method.GET));
            UserInfo = result.Content;

            dynamic transResult = JToken.Parse(UserInfo) as dynamic;
            "fas".ToString();
            MessageBox.Show(transResult.name.ToString());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/login/oauth/authorize?client_id=2bad7ade7264aaafee4b&scope=user:email");
            GetToken();
        }
    }
}
