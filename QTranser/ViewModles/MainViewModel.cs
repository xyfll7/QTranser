using QTranser.QTranseLib;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace QTranser.ViewModles
{
    public class MainViewModel: ViewModel
    {
        private int _todayTanseTime = 0;
        public int TodayTanseTime
        {
            get => _todayTanseTime;
            set
            {
                if (value == _todayTanseTime) return;
                _todayTanseTime = value;
                OnPropertyChanged();
            }
        }

        private int _historyTanseTime = 0;
        public int WeekTanseTime
        {
            get => _historyTanseTime;
            set
            {
                if (value == _historyTanseTime) return;
                _historyTanseTime = value;
                OnPropertyChanged();
            }
        }

        private string _userName = "——— login ———";
        public string UserName
        {
            get => _userName;
            set
            {
                if (value == _userName) return;
                _userName = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibility0 = Visibility.Collapsed;
        public Visibility Visibility0
        {
            get => _visibility0;
            set
            {
                if (value == _visibility0) return;
                _visibility0 = value;
                OnPropertyChanged();
            }
        }

        private Visibility _visibility1 = Visibility.Collapsed;
        public Visibility Visibility1
        {
            get => _visibility1;
            set
            {
                if (value == _visibility1) return;
                _visibility1 = value;
                OnPropertyChanged();
            }
        }

        private string _strQ = "QTranser";
        public string StrQ
        {
            get => _strQ;
            set
            {
                if (value == _strQ) return;
                _strQ = value;
                OnPropertyChanged();
            }
        }

        private string _strI = "请输入...";
        public string StrI
        {
            get => _strI;
            set
            {
                if (value == _strI) return;
                _strI = value;
                OnPropertyChanged();
            }
        }

        private string _strO = "please input...";
        public string StrO
        {
            get => _strO;
            set
            {
                if (value == _strO) return;
                _strO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<HistoryWord> HistoryWord { get; set; } = new ObservableCollection<HistoryWord>();

        private string _hotKeyQ;
        public string HotKeyQ
        {
            get => _hotKeyQ + Environment.NewLine + "快捷输入";
            set
            {
                if (value == _hotKeyQ) return;
                _hotKeyQ = value;
                OnPropertyChanged();
            }
        }

        private string _hotKeyW;
        public string HotKeyW
        {
            get => _hotKeyW + Environment.NewLine + "打开/关闭翻译详情界面";
            set
            {
                if (value == _hotKeyW) return;
                _hotKeyW = value;
                OnPropertyChanged();
            }
        }

        private string _hotKeyB;
        public string HotKeyB
        {
            get => _hotKeyB + Environment.NewLine + "一键百度";
            set
            {
                if (value == _hotKeyB) return;
                _hotKeyB = value;
                OnPropertyChanged();
            }
        }

        private string _hotKeyG;
        public string HotKeyG
        {
            get => _hotKeyG + Environment.NewLine + "一键谷歌";
            set
            {
                if (value == _hotKeyG) return;
                _hotKeyG = value;
                OnPropertyChanged();
            }
        }

        private Brush _logoColor = Theme.GetLogoColor();
        public Brush LogoColor
        {
            get => _logoColor ;
            set
            {
                if (value == _logoColor) return;
                _logoColor = value;
                OnPropertyChanged();
            }
        }


    }
}