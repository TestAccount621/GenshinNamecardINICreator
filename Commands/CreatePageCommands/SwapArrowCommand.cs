using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenshinNamecardINICreator.Commands.CreatePageCommands
{
    public class SwapArrowCommand : CommandBase
    {
        private readonly CreateNamecardModPageViewModel viewModel;

        public SwapArrowCommand(CreateNamecardModPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {

            if (!viewModel.IsMoving)
            {
                viewModel.IsMoving = true;
                var values = (object[])parameter;
                if (values[0] is ListView fromListView && values[1] is ListView toListView)
                {
                    List<CreateNamecardModPageItemViewModel> selected = fromListView.SelectedItems.Cast<CreateNamecardModPageItemViewModel>().ToList();
                    viewModel.MoveHorizontally(selected, ((ListView)values[1]).Name);
                }
                viewModel.IsMoving = false;
            }
        }
    }
}
