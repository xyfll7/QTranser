using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTranser.QTranseLib.SQLite
{
    public interface IDataProvider
    {
        bool Delete(IHistoryWord friend);

        List<IHistoryWord> GetAllFriends();

        IHistoryWord GetFriendById(string id);

        bool Insert(IHistoryWord friend);

        bool Update(IHistoryWord friend);
    }
}
