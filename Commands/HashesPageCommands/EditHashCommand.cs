using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Commands.HashesPageCommands
{
    public class EditHashCommand : CommandBase
    {
        private readonly NamecardHashesPageViewModel viewModel;

        public EditHashCommand(NamecardHashesPageViewModel namecardHashesPageViewModel)
        {
            this.viewModel = namecardHashesPageViewModel;
        }

        public override void Execute(object? parameter)
        {
            viewModel.CurrentState = Models.NamecardHashesPageStateEnum.Edit;
            viewModel.PreviousSelectedNamecard = viewModel.SelectedNamecardHashesPageComboBoxItemViewModel;
        }
    }
}
