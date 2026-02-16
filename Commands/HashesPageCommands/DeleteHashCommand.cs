using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GenshinNamecardINICreator.Commands.HashesPageCommands
{
    public class DeleteHashCommand : CommandBase
    {
        private readonly NamecardHashesPageViewModel viewModel;
        public DeleteHashCommand(NamecardHashesPageViewModel namecardHashesPageViewModel)
        {
            this.viewModel = namecardHashesPageViewModel;
        }
        public override void Execute(object? parameter)
        {
            string message = String.Format("Are you sure you want to delete {0}?", viewModel.SelectedNamecardHashesPageComboBoxItemViewModel.Name);
            if (MessageBox.Show(message, "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                viewModel.DeleteNamecard(viewModel.SelectedNamecardHashesPageComboBoxItemViewModel);
            }
        }
    }
}
