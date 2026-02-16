using GenshinNamecardINICreator.CreationClasses;
using GenshinNamecardINICreator.Properties;
using GenshinNamecardINICreator.Stores;
using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GenshinNamecardINICreator.Commands.ConvertPageCommands
{
    public class ConvertCommand : AsyncCommandBase
    {
        private readonly ConvertPageViewModel convertPageViewModel;
        private readonly ListViewsRefreshStore listViewsRefreshStore;
        private readonly ProgressBarWindowViewModel progressBarWindowViewModel;
        private readonly ModalNavigationStore modalNavigationStore;

        public ConvertCommand(ConvertPageViewModel convertPageViewModel,
                              ListViewsRefreshStore listViewsRefreshStore,
                              ProgressBarWindowViewModel progressBarWindowViewModel,
                              ModalNavigationStore modalNavigationStore)
        {
            this.convertPageViewModel = convertPageViewModel;
            this.listViewsRefreshStore = listViewsRefreshStore;
            this.progressBarWindowViewModel = progressBarWindowViewModel;
            this.modalNavigationStore = modalNavigationStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            string path = Settings.Default.OutputFolder;
            if (!String.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                if (parameter is IEnumerable<ConvertPageItemViewModel> files)
                {
                    ConvertToNamecardFormat convert = new(path);
                    var list = files.ToList();
                    int totalCount = list.Count;
                    progressBarWindowViewModel.SetTitle("Converting Image/Gifs");
                    progressBarWindowViewModel.UpdateProgress(0, "");
                    modalNavigationStore.CurrentViewModel = progressBarWindowViewModel;
                    for (int i = 0; i < totalCount; i++)
                    {
                        double progress = (double)i/(double)totalCount;
                        progressBarWindowViewModel.UpdateProgress(progress, list[i].FileInfo.Name);
                        await convert.BeginConversion(list[i].FileInfo.FullName);
                    }
                    modalNavigationStore.CurrentViewModel = null;
                    convertPageViewModel.ClearListView();
                    listViewsRefreshStore.Refresh();
                }
            }
        }
    }
}
