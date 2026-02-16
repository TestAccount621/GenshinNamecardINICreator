using GenshinNamecardINICreator.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Commands
{
    public class RefreshFolderCommand : CommandBase
    {
        private readonly ListViewsRefreshStore viewModel;

        public RefreshFolderCommand(ListViewsRefreshStore listViewsRefreshStore)
        {
            this.viewModel = listViewsRefreshStore;
        }

        public override void Execute(object? parameter)
        {
            viewModel.Refresh();
        }
    }
}
