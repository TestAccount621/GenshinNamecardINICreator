using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Commands.HashesPageCommands
{
    public class NewHashCommand : CommandBase
    {
        private readonly NamecardHashesPageViewModel viewModel;

        public NewHashCommand(NamecardHashesPageViewModel namecardHashesPageViewModel)
        {
            this.viewModel = namecardHashesPageViewModel;
        }

        public override void Execute(object? parameter)
        {
            viewModel.CurrentState = Models.NamecardHashesPageStateEnum.New;
            // Save the currently selected Namecard to go back to if cancel is clicked essentially.
            viewModel.PreviousSelectedNamecard = viewModel.SelectedNamecardHashesPageComboBoxItemViewModel;
            viewModel.SelectedNamecardHashesPageComboBoxItemViewModel = null;
        }
    }
}
