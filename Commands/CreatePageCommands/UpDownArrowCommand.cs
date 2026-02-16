using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenshinNamecardINICreator.Commands.CreatePageCommands
{
    public class UpDownArrowCommand : CommandBase
    {
        private readonly CreateNamecardModPageViewModel viewModel;

        public UpDownArrowCommand(CreateNamecardModPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        /// <summary>
        /// This will take 2 parameters. The button Tag value and the listview to grab the SelectedItems
        /// </summary>
        /// <param name="parameter">Button's Tag & ListView Control</param>
        public override void Execute(object? parameter)
        {
            if (!viewModel.IsMoving)
            {
                viewModel.IsMoving = true;
                var values = (object[])parameter;
                // The tag on the button to mark if it's up or down
                string tag = values[0].ToString();
                // The listview so I can grab the selected items.
                ListView listView = values[1] as ListView;
                if (listView != null)
                {
                    if (listView.SelectedItems.Count > 0)
                    {
                        List<CreateNamecardModPageItemViewModel> selected = listView.SelectedItems.Cast<CreateNamecardModPageItemViewModel>().ToList();
                        viewModel.MoveVertically(tag, selected);
                    }
                }
                viewModel.IsMoving = false;
            }
        }
    }
}
