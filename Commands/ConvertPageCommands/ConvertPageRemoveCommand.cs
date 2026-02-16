using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GenshinNamecardINICreator.Commands.ConvertPageCommands
{
    public class ConvertPageRemoveCommand : CommandBase
    {
        private readonly ConvertPageViewModel viewModel;

        public ConvertPageRemoveCommand(ConvertPageViewModel convertPageViewModel)
        {
            this.viewModel = convertPageViewModel;
        }

        public override void Execute(object? parameter)
        {
            System.Collections.IList item = (System.Collections.IList)parameter;
            var collection = item.Cast<ConvertPageItemViewModel>();
            foreach (ConvertPageItemViewModel file in collection.ToList())
            {
                viewModel.RemoveFileInfo(file);
            }
        }
    }
}
