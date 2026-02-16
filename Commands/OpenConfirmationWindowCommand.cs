using GenshinNamecardINICreator.Stores;
using GenshinNamecardINICreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenshinNamecardINICreator.Commands
{
    public class OpenConfirmationWindowCommand : CommandBase
    {
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly NamecardHashesStore namecardHashesStore;
        private readonly ProgressBarWindowViewModel progressBarWindowViewModel;
        private readonly CreateNamecardModPageViewModel createNamecardModPageViewModel;

        public OpenConfirmationWindowCommand(ModalNavigationStore modalNavigationStore,
                                             NamecardHashesStore namecardHashesStore,
                                             ProgressBarWindowViewModel progressBarWindowViewModel,
                                             CreateNamecardModPageViewModel createNamecardModPageViewModel)
        {
            this.modalNavigationStore = modalNavigationStore;
            this.namecardHashesStore = namecardHashesStore;
            this.progressBarWindowViewModel = progressBarWindowViewModel;
            this.createNamecardModPageViewModel = createNamecardModPageViewModel;
        }

        public override void Execute(object? parameter)
        {
            ConfirmationWindowViewModel viewModel = new ConfirmationWindowViewModel(this.namecardHashesStore, modalNavigationStore, progressBarWindowViewModel, createNamecardModPageViewModel);
            this.modalNavigationStore.CurrentViewModel = viewModel;
        }
    }
}
