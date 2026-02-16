using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Commands.HashesPageCommands
{
    public class SubmitCommand : CommandBase
    {
        private readonly NamecardHashesPageViewModel viewModel;
        public SubmitCommand(NamecardHashesPageViewModel namecardHashesPageViewModel)
        {
            this.viewModel = namecardHashesPageViewModel;
        }
        public override void Execute(object? parameter)
        {
            var values = (object[])parameter;
            var name = values[0].ToString();
            var main = values[1].ToString();
            var preview = values[2].ToString();
            var banner = values[3].ToString();
            if (viewModel.CurrentState == Models.NamecardHashesPageStateEnum.New)
            {
                viewModel.AddNamecard(name, main, preview, banner);
            }
            else if (viewModel.CurrentState == Models.NamecardHashesPageStateEnum.Edit)
            {
                viewModel.UpdateNamecard(name, main, preview, banner);
            }
        }
    }
}
