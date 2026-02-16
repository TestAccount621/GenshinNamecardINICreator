using GenshinNamecardINICreator.Properties;
using GenshinNamecardINICreator.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GenshinNamecardINICreator.Commands.ConvertPageCommands
{
    public class ConvertPageSelectImagesCommand : CommandBase
    {
        private readonly ConvertPageViewModel viewModel;

        public ConvertPageSelectImagesCommand(ConvertPageViewModel convertPageViewModel)
        {
            this.viewModel = convertPageViewModel;
        }

        public override void Execute(object? parameter)
        {
            List<FileInfo> files = [];
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.Exists(Settings.Default.OutputFolder)
                ? Settings.Default.OutputFolder
                : AppDomain.CurrentDomain.BaseDirectory;
            dialog.Multiselect = true;
            dialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.gif";
            var result = dialog.ShowDialog();
            if (result == true)
            {
                var _files = dialog.FileNames;
                if (_files.Length > 0)
                {
                    foreach (var f in _files)
                    {
                        viewModel.AddFileInfo(new ConvertPageItemViewModel(new FileInfo(f)));
                    }
                }
            }
        }
    }
}
