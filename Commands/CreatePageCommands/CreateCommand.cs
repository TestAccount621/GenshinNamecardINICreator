using GenshinNamecardINICreator.CreationClasses;
using GenshinNamecardINICreator.Models;
using GenshinNamecardINICreator.Properties;
using GenshinNamecardINICreator.Stores;
using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenshinNamecardINICreator.Commands.CreatePageCommands
{
    public class CreateCommand : AsyncCommandBase
    {
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly ProgressBarWindowViewModel progressBarWindowViewModel;
        private readonly CreateNamecardModPageViewModel createNamecardModPageViewModel;

        public CreateCommand(ModalNavigationStore modalNavigationStore,
                             ProgressBarWindowViewModel progressBarWindowViewModel,
                             CreateNamecardModPageViewModel createNamecardModPageViewModel)
        {
            this.modalNavigationStore = modalNavigationStore;
            this.progressBarWindowViewModel = progressBarWindowViewModel;
            this.createNamecardModPageViewModel = createNamecardModPageViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            string path = Settings.Default.OutputFolder;
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                if (parameter is NamecardData namecard)
                {
                    List<CreateNamecardModPageItemViewModel> list = createNamecardModPageViewModel.InsideCollection.ToList();
                    NamecardMergedINI create = new NamecardMergedINI(namecard);
                    int totalCount = list.Count;
                    progressBarWindowViewModel.SetTitle("Creating the namecard mod collection.");
                    progressBarWindowViewModel.UpdateProgress(0, "");
                    modalNavigationStore.CurrentViewModel = progressBarWindowViewModel;
                    await create.CreateMod(list, progressBarWindowViewModel);
                    modalNavigationStore.CurrentViewModel = null;
                }
            }
        }
    }
}
