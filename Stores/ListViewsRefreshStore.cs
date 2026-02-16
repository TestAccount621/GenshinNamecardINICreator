using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Stores
{
    public class ListViewsRefreshStore
    {
        public event Action RefreshNeeded;

        public void Refresh()
        {
            RefreshNeeded?.Invoke();
        }
    }
}
