using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTranser.QTranseLib.SQLite
{
    public interface IHistoryWord
    {
        string Id { get; set; }
        uint Times { get; set; }
        string Word { get; set; }
        string Translate { get; set; }
    }
}
