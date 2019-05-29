using CSDeskBand;
using IWshRuntimeLibrary;
using QTranser.QTranseLib;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace QTranser
{
    /// <summary>
    /// QShowse.xaml 的交互逻辑
    /// </summary>
    public partial class QShower : Window
    {
        private GithubLogin githubLogin = new GithubLogin();
       
        public QShower()
        {
            InitializeComponent();
            DataContext = QTranse.Mvvm;
            GithubLogin.InitGitHubUserName();
            TanseTimes.InitTranseTime();
        }

        public void ShowOrHide(double actualHeight, double actualWidth, double pointToScreen)
        {
            try
            {
                if (this.IsVisible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    if (Deskband.Edger == Edge.Top)
                    {
                        this.Left = pointToScreen + actualWidth - this.Width;
                        this.Top = actualHeight;
                    }
                    if (Deskband.Edger == Edge.Bottom)
                    {
                        this.Left = pointToScreen + actualWidth - this.Width;
                        this.Top = SystemParameters.WorkArea.Height - this.Height;
                    }
                    if (Deskband.Edger == Edge.Left)
                    {
                        this.Left = actualWidth;
                        this.Top = SystemParameters.WorkArea.Height - this.Height;
                    }
                    if (Deskband.Edger == Edge.Right)
                    {
                        this.Left = SystemParameters.WorkArea.Width - this.Width;
                        this.Top = SystemParameters.WorkArea.Height - this.Height;
                    }
                    this.Visibility = Visibility.Visible;
                    this.Topmost = true;
                    this.Activate();
                    this.StrIBox.Focus();
                    this.StrIBox.SelectionStart = this.StrIBox.Text.Length;
                }

            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        public void InputStrProsessing(object sender, KeyEventArgs e)
        {
            string str = ((TextBox)sender).Text;
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.L)
            {
                ((TextBox)sender).Clear();
            }
            if (e.Key == Key.Enter)
            {
                if (str.EndsWith("/") || str.EndsWith("?"))
                {
                    Process.Start("https://www.baidu.com/s?ie=UTF-8&wd=" + str);
                }
                else if(str.StartsWith("https://") || str.StartsWith("http://") || str.StartsWith("www."))
                {
                    Process.Start(str);
                }
                else
                {
                    Clipboard.SetText(str);
                }
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private bool IsTop { get; set; } = true;
        private void SetTop_Click(object sender, RoutedEventArgs e)
        {
            //后台代码动态修改控件模板属性 参考链接：https://www.itsvse.com/thread-2740-1-1.html
            Border border = (Border)((Button)sender).Template.FindName("Border", (Button)sender);
            TextBlock textBlock = (TextBlock)((Button)sender).Template.FindName("topPrompt", (Button)sender);
            if (IsTop)
            {
                border.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                this.Deactivated -= Window_Deactivated;
                textBlock.Text = " 取消置顶";
                IsTop = false;
            }
            else
            {
                border.Background = Brushes.Transparent;
                this.Deactivated += Window_Deactivated;
                textBlock.Text = " 置顶";
                IsTop = true;
            }
          
        }

        private void StrIBox_KeyUp(object sender, KeyEventArgs e)
        {
            InputStrProsessing(sender, e);
        }

        private void HistoryList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(((TextBlock)sender).Text);
        }

        private void HistoryList_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            QTranse.Mvvm.HistoryWord.RemoveAt(HistoryList.SelectedIndex);
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            string shortcutPath =  ((DataObject)e.Data).GetFileDropList()[0];
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            MessageBox.Show(shortcut.FullName);
            MessageBox.Show(shortcut.TargetPath);
            //Properties.Settings.Default.设置;
        }

        private void Pather_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            githubLogin.Button_Click();
        }

        private void Hyperlink_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("ms-settings:personalization-colors");
        }
    }
}
