using GenshinNamecardINICreator.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Commands
{
    public class SelectFolderCommand : CommandBase
    {
        private readonly HomeViewViewModel viewModel;

        public SelectFolderCommand(HomeViewViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            dialog.Multiselect = false;
            dialog.InitialDirectory = viewModel.DestinationFolderPath;
            var result = dialog.ShowDialog();
            if (result == true)
            {
                viewModel.DestinationFolderPath = dialog.FolderName;
            }
        }
    }
}
