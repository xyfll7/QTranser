using QTranser.ViewModles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTranser.QTranseLib.SQLite
{
    public class HistoryWord : ViewModel, IHistoryWord
    {
        private string _id;
        private uint _times;
        private string _word;
        private string _translate;

        public string Id
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public uint Times
        {
            get => _times;
            set
            {
                if (value == _times) return;
                _times = value;
                OnPropertyChanged();
            }
        }

        public string Word
        {
            get => _word;
            set
            {
                if (value == _word) return;
                _word = value;
                OnPropertyChanged();
            }
        }

        public string Translate
        {
            get => _translate;
            set
            {
                if (value == _translate) return;
                _translate = value;
                OnPropertyChanged();
            }
        }
    }
}
