using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Commands
{
    public class ChangeHomeViewViewCommand : CommandBase
    {
        private HomeViewViewModel viewModel;

        public ChangeHomeViewViewCommand(HomeViewViewModel homeViewViewModel)
        {
            viewModel = homeViewViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (parameter.ToString() == "Create")
            {
                viewModel.ActiveView = viewModel.CreateNamecardModPageViewModel;
            }
            else if (parameter.ToString() == "Convert")
            {
                viewModel.ActiveView = viewModel.ConvertPageViewModel;
            }
            else if (parameter.ToString() == "Hashes")
            {
                viewModel.ActiveView = viewModel.NamecardHashesPageViewModel;
            }
            else if (parameter.ToString() == "Settings")
            {
                viewModel.ActiveView = viewModel.SettingsPageViewModel;
            }
        }
    }
}
