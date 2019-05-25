using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;

namespace QTranserOneKeyInstaller
{
    class OneKeyInstaller
    {
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 注销QTranser "%~dp0RegAsm.exe" /nologo /unregister "%~dp0..\QTranser.dll"
            RegisterDll("/nologo /unregister /silent \"{0}\"", "QTranser.dll");
            // 关闭Explore
            closeExplorer("taskkill /f /im \"explorer.exe\"");
            // 启动Explore
            strartExplorer();
            // 注册QTranser "%~dp0RegAsm.exe" /nologo /codebase "%~dp0..\QTranser.dll"
            RegisterDll("/nologo /codebase /silent \"{0}\"", "QTranser.dll");
        }

        public void strartExplorer()
        {
            string explorer = string.Format("{0}\\{1}", Environment.GetEnvironmentVariable("WINDIR"), "explorer.exe");
            using (var p = new Process())
            {
                p.StartInfo.FileName = explorer;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                p.Close();
                p.Dispose();
            }

        }

        public void closeExplorer(string str)
        {
            string cmdline = $"{str}";
            using (var p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.WorkingDirectory = @"C:\Users\Administrator\Desktop";
                p.StartInfo.CreateNoWindow = true;

                p.Start();
                p.StandardInput.AutoFlush = true;
                p.StandardInput.WriteLine(cmdline + " &exit");

                //获取cmd窗口的输出信息  
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
                p.Dispose();
            }
        }


        private bool RegisterDll(string args, string fileName)
        {
            bool result = true;
            try
            {
                string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);//获得要注册的dll的物理路径
                if (!File.Exists(dllPath))
                {
                    return false;
                }
                //拼接命令参数
                string startArgs = string.Format(args, dllPath);

                Process p = new Process();//创建一个新进程，以执行注册动作
                p.StartInfo.FileName = "RegAsm";
                p.StartInfo.Arguments = startArgs;
                p.StartInfo.CreateNoWindow = true;

                //以管理员权限注册dll文件
                WindowsIdentity winIdentity = WindowsIdentity.GetCurrent(); //引用命名空间 System.Security.Principal
                WindowsPrincipal winPrincipal = new WindowsPrincipal(winIdentity);
                if (!winPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    p.StartInfo.Verb = "runas";//管理员权限运行
                }
                p.Start();
                p.WaitForExit();
                p.Close();
                p.Dispose();
            }
            catch (Exception ex)
            {
                result = false;
                //记录日志，抛出异常
            }

            return result;
        }
    }
}
