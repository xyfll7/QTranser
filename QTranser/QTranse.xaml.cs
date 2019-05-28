//#define LOG
//#define gitDOWNLOAD

using Newtonsoft.Json.Linq;
using QTranser.ViewModles;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QTranser.QTranseLib;
using GlobalHotKey;
using WindowsInput;
using WindowsInput.Native;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using QTranser.Properties;

namespace QTranser
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class QTranse: UserControl
    {
        public static HotKeyManager HotKeyManage;
        public static MainViewModel Mvvm { get; private set; } = new MainViewModel();
        public static QShower Shower { get; private set; }

        private ClipboardMonitor _clipboardMonitor;
        private InputSimulator Sim { get; set; } = new InputSimulator();
        private ForegroundWindow ForegroundW { get; set; } = new ForegroundWindow();
        private Update Update { get; } = new Update();

        // 全局未捕获异常
        private GlobalUnhandledException GlobalExcep { get; set; } = new GlobalUnhandledException();

        public QTranse()
        {
            InitializeComponent();
            DataContext = Mvvm;
        }

        // 初始化/加载
        private void QTranser_Loaded(object sender, RoutedEventArgs e)
        {
            _clipboardMonitor = new ClipboardMonitor(this);
            _clipboardMonitor.ClipboardUpdate += OnClipboardUpdate;

            HotKeyManage = new HotKeyManager(this);
            HotKeyManage.KeyPressed += OnHotKeyPressed;
            RegisterHotKey();
                
            var SysColor = new SysColorChanger(this);
            SysColor.SysColorChange += () => Mvvm.LogoColor = Theme.GetLogoColor(); 
        }

        
        // 剪切板事件处理
        private async void OnClipboardUpdate(object sender, EventArgs e)
        {

            string str = ClipboardGetText();

            if (str == "") return;

            // 翻译次数
            TanseTimes.AddTodayTranseTime();

            str = AddSpacesBeforeCapitalLetters(str);

            Mvvm.StrQ = "...";
            Mvvm.StrI = "...";
            string sss = await Task.Run(()=> TranslationResultDisplay(str));

            bool isRepeat = Mvvm.HistoryWord.Any<HistoryWord>(o => o.Word.Trim().ToLower() == str.Trim().ToLower());
            if(isRepeat) return;
            else
            {
                Mvvm.HistoryWord.Insert(0, new HistoryWord() { Word = str ,Translate = sss});
            }

            if (Mvvm.HistoryWord.Count > 8) Mvvm.HistoryWord.RemoveAt(8);


            // 软件升级
            Update.GetNewVersion(Mvvm);
        }
        private string ClipboardGetText()
        {
            string str = "";
            if (Clipboard.ContainsText())
            {
                try { str = Clipboard.GetText(); }
                catch (COMException) { }
                finally
                {
                    Thread.Sleep(20);
                    try { str = Clipboard.GetText(); }
                    catch (COMException) { str = "剪切板被占用"; }
                }
            }
#if gitDOWNLOAD
            str = Download(str);
#endif
            return str.Trim().Replace("  ", "");
        }
        private string AddSpacesBeforeCapitalLetters(string str)
        {
            str = Regex.Replace(str, "([a-z])([A-Z](?=[A-Z])[a-z]*)", "$1 $2");
            str = Regex.Replace(str, "([A-Z])([A-Z][a-z])", "$1 $2");
            str = Regex.Replace(str, "([a-z])([A-Z][a-z])", "$1 $2");
            str = Regex.Replace(str, "([a-z])([A-Z][a-z])", "$1 $2");
            return str.ToLower();  // 因为金山词霸翻译接口不认识大写。
        }
        private int I { get; set; } = 0;
        private string TranslationResultDisplay(string str)
        {
            var translator = new Translator();

            string transResultJson = translator.GetJson(str);
            dynamic transResult = JToken.Parse(transResultJson) as dynamic;

            string detailsStrO = "";
            try
            {
                if(transResult.en != "" && transResult.am != "" && transResult.en != null)
                {
                    detailsStrO += "英:[" + transResult?.en + "]" + "  美:[" + transResult?.am + "]" + Environment.NewLine + Environment.NewLine;
                }
                if(transResult.api == "jinshan-en")
                {
                    foreach (var strr in transResult.dst)
                    {
                        detailsStrO += strr.part + Environment.NewLine;
                        foreach (var strrr in strr.means)
                        {
                            detailsStrO += strrr + "  ";
                        }
                        detailsStrO += Environment.NewLine;
                    }
                }
                if (transResult.api == "jinshan-cn")
                {
                    foreach (var strr in transResult.dst)
                    {
                        detailsStrO += strr + Environment.NewLine;
                    }
                }
           
                Mvvm.StrI = str;
                string baiduStr = "";

                foreach(var strrr in transResult.baidu)
                {
                    baiduStr += strrr.dst + "\n";
                }
                Mvvm.StrQ = baiduStr.Replace("\n", "");
                if(detailsStrO != "")
                {
                    Mvvm.StrO = detailsStrO.Substring(0, detailsStrO.Length - 2);
                }
                else
                {
                    detailsStrO = baiduStr;
                    Mvvm.StrO = baiduStr;
                }

            }
            catch{ }

            return detailsStrO;

#if YOUDAOAPI
            var errotCode = transResult?.errorCode;
            if (errotCode == "108" && i < 5)
            {
                TranslationResultDisplay(str); i++;
                return "{}";
            }
            if (errotCode == "401")
            {
                QTranseLib.Idkey.idkdy();
            }
            i = 0;
            Mvvm.StrI = str;
            // 将翻译结果写入 transResult.json 文件
#if LOG
            Loger.json(transResult);
#endif
            string detailsStr = transResult?.translation?[0] + Environment.NewLine;

            if (transResult?.basic != null)
            {
                detailsStr += "----------------" + Environment.NewLine;
                foreach (var strr in transResult?.basic?.explains)
                {
                    detailsStr += strr + Environment.NewLine;
                }
            }

            if (transResult?.web != null)
            {
                detailsStr += "----------------" + Environment.NewLine;
                foreach (var element in transResult.web)
                {
                    detailsStr += element.key + Environment.NewLine;
                    if (element?.value != null)
                    {
                        foreach (var strr in element.value)
                        {
                            detailsStr += "  " + strr + Environment.NewLine;
                        }
                    }
                }
            }
            try
            {
                string s = transResult?.translation?[0];
                string z = detailsStr.Substring(0, detailsStr.Length - 2);
                Mvvm.StrQ = s.Replace("\n", "");
                Mvvm.StrO = z;
                return z;
            }
            catch (Exception) { }
            return "";
#endif
        }

        //下载github文件
        private string Download(string str)
        {
            if (str.StartsWith("https://") && str.EndsWith(".git"))
            {
                ExecuteInCmd(str);
                str = "正在下载";
            }
            return str;
        }
        public async void ExecuteInCmd(string str)
        {
            string cmdline = $"git clone {str}";
            await Task.Run(() => {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.WorkingDirectory = @"C:\Users\Administrator\Desktop";
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.StandardInput.AutoFlush = true;
                    process.StandardInput.WriteLine(cmdline + " &exit");

                    //获取cmd窗口的输出信息  
                    string output = process.StandardOutput.ReadToEnd();
                    MessageBox.Show(output);
                    process.WaitForExit();
                    process.Close();
                }
            });
        }

        // 热键注册/响应
        private void RegisterHotKey()
        {
            var kc = new KeyConverter();
            var mkc = new ModifierKeysConverter();
            //try
            //{
            //    var hotKeyQ = HotKeyManage.Register(Key.Q, ModifierKeys.Control);
            //    Mvvm.HotKeyQ = HotKeyManage.ToString();
            //    HotKeyEditor.HotKey.hotKeyModQ = ModifierKeys.Control;
            //    HotKeyEditor.HotKey.hotKeyQ = Key.Q;
            //}
            //catch
            //{
            //    Mvvm.HotKeyQ = HotKeyManage.ToString() + "(冲突)";
            //}
            //try
            //{
            //    var hotKeyW = HotKeyManage.Register(Key.R, ModifierKeys.Control);
            //    Mvvm.HotKeyW = HotKeyManage.ToString();
            //    HotKeyEditor.HotKey.hotKeyModW = ModifierKeys.Control;
            //    HotKeyEditor.HotKey.hotKeyW = Key.R;
            //}
            //catch
            //{
            //    Mvvm.HotKeyW = HotKeyManage.ToString() + "(冲突)";
            //}
            //try
            //{
            //    var hotKeyB = HotKeyManage.Register(Key.B, ModifierKeys.Control);
            //    Mvvm.HotKeyB = HotKeyManage.ToString();
            //    HotKeyEditor.HotKey.hotKeyModB = ModifierKeys.Control;
            //    HotKeyEditor.HotKey.hotKeyB = Key.B;
            //}
            //catch
            //{
            //    Mvvm.HotKeyB = HotKeyManage.ToString() + "(冲突)";
            //}


            try
            {
                if (Settings.Default.hotKeyModQ != "" && Settings.Default.hotKeyQ != "")
                {
                    ModifierKeys mod = (ModifierKeys)mkc.ConvertFromString(Settings.Default.hotKeyModQ);
                    Key key = (Key)kc.ConvertFromString(Settings.Default.hotKeyQ);

                    HotKeyManage.Register(key, mod);
                }
                else
                {
                    HotKeyManage.Register(Key.Q, ModifierKeys.Control);

                    Settings.Default.hotKeyModQ = mkc.ConvertToString(ModifierKeys.Control);
                    Settings.Default.hotKeyQ = kc.ConvertToString(Key.Q);
                }
                Mvvm.HotKeyQ = HotKeyManage.ToString();
            }

            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                Mvvm.HotKeyQ = HotKeyManage.ToString() + "(冲突)";
            }

            //////////////////////////////////////////////////




            try
            {
                if (Settings.Default.hotKeyModW != "" && Settings.Default.hotKeyW != "")
                {
                    ModifierKeys mod = (ModifierKeys)mkc.ConvertFromString(Settings.Default.hotKeyModW);
                    Key key = (Key)kc.ConvertFromString(Settings.Default.hotKeyW);

                    HotKeyManage.Register(key, mod);
                }
                else
                {
                    HotKeyManage.Register(Key.W, ModifierKeys.Control);

                    Settings.Default.hotKeyModW = mkc.ConvertToString(ModifierKeys.Control);
                    Settings.Default.hotKeyW = kc.ConvertToString(Key.W);
                }
                Mvvm.HotKeyW = HotKeyManage.ToString();
            }

            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                Mvvm.HotKeyW = HotKeyManage.ToString() + "(冲突)";
            }

            //////////////////////////////////////////////////



            try
            {
                if (Settings.Default.hotKeyModB != "" && Settings.Default.hotKeyB != "")
                {
                    ModifierKeys mod = (ModifierKeys)mkc.ConvertFromString(Settings.Default.hotKeyModB);
                    Key key = (Key)kc.ConvertFromString(Settings.Default.hotKeyB);

                    HotKeyManage.Register(key, mod);
                }
                else
                {
                    HotKeyManage.Register(Key.B, ModifierKeys.Control);

                    Settings.Default.hotKeyModB = mkc.ConvertToString(ModifierKeys.Control);
                    Settings.Default.hotKeyB = kc.ConvertToString(Key.B);
                }
                Mvvm.HotKeyB = HotKeyManage.ToString();
            }

            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                Mvvm.HotKeyB = HotKeyManage.ToString() + "(冲突)";
            }

            //////////////////////////////////////////////////

            try
            {
                if (Settings.Default.hotKeyModG != "" && Settings.Default.hotKeyG != "")
                {
                    ModifierKeys mod = (ModifierKeys)mkc.ConvertFromString(Settings.Default.hotKeyModG);
                    Key key = (Key)kc.ConvertFromString(Settings.Default.hotKeyG);

                    HotKeyManage.Register(key, mod);
                }
                else
                {
                    HotKeyManage.Register(Key.G, ModifierKeys.Control);

                    Settings.Default.hotKeyModG = mkc.ConvertToString(ModifierKeys.Control);
                    Settings.Default.hotKeyG = kc.ConvertToString(Key.G);
                }
                Mvvm.HotKeyG = HotKeyManage.ToString();
            }
            
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                Mvvm.HotKeyG = HotKeyManage.ToString() + "(冲突)";
            }


            Settings.Default.Save();
        }

        private void OnHotKeyPressed(object sender, KeyPressedEventArgs e)
        {
            var kc = new KeyConverter();
            if (e.HotKey.Key == (Key)kc.ConvertFromString(Settings.Default.hotKeyQ))
            {
                ForegroundW.SetForeground("Shell_TrayWnd");
                textBox.Focus();
                textBox.Clear();
            }
            if(e.HotKey.Key == (Key)kc.ConvertFromString(Settings.Default.hotKeyW))
            {
                if (Shower == null)
                { Shower = new QShower(); }

                Shower.ShowOrHide(ActualHeight, ActualWidth, PointToScreen(new Point()).X);
            }
            if (e.HotKey.Key == (Key)kc.ConvertFromString(Settings.Default.hotKeyB))
            { 
                Sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                Thread.Sleep(20);
                string str = ClipboardGetText(); 
                Process.Start("https://www.baidu.com/s?ie=UTF-8&wd=" + str);
            }
            if (e.HotKey.Key == (Key)kc.ConvertFromString(Settings.Default.hotKeyG))
            {
                Sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                Thread.Sleep(20);
                string str = ClipboardGetText();
                Process.Start("http://google.com#q=" + str);
            }
        }

        // 打开输入框
        private void Logo_MouseEnter(object sender, MouseEventArgs e)
        {
            // 必须借助真实鼠标/键盘按键 SetForeground函数 才能抢到焦点。
            Sim.Mouse.Keyboard.KeyUp(VirtualKeyCode.RIGHT);
            ForegroundW.SetForeground("Shell_TrayWnd");
            textBox.Focus();
            textBox.SelectionStart = textBox.Text.Length;
        }

        // 输入文字处理
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (Shower == null)
            { Shower = new QShower(); }
            Shower.InputStrProsessing(sender, e);
        }

        // 打开/关闭 翻译详情
        private void Logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Shower == null)
            { Shower = new QShower(); }
            Shower.ShowOrHide(ActualHeight, ActualWidth, PointToScreen(new Point()).X);
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Visibility = Visibility.Collapsed;
            Runfile(@"C:\Program Files\QTranser", "QTranser_Installer.msi");
        }
        private void Runfile(string path, string fileName)
        {
            string targetPath = string.Format(path);
            Process process = new Process();
            process.StartInfo.WorkingDirectory = targetPath;
            process.StartInfo.FileName = fileName;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
    }
}