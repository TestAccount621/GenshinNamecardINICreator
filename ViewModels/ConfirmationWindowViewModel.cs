using GenshinNamecardINICreator.Commands;
using GenshinNamecardINICreator.Commands.CreatePageCommands;
using GenshinNamecardINICreator.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenshinNamecardINICreator.ViewModels
{
    public class ConfirmationWindowViewModel : BaseViewModel
    {
        private readonly NamecardHashesStore namecardHashesStore;
        private readonly ModalNavigationStore modalNavigationStore;

        public MintPlayer.ObservableCollection.ObservableCollection<Models.NamecardData> Namecards => namecardHashesStore.Namecards;

        public ConfirmationWindowViewModel(NamecardHashesStore namecardHashesStore,
                                           ModalNavigationStore modalNavigationStore,
                                           ProgressBarWindowViewModel progressBarWindowViewModel,
                                           CreateNamecardModPageViewModel createNamecardModPageViewModel)
        {
            this.namecardHashesStore = namecardHashesStore;
            this.modalNavigationStore = modalNavigationStore;

            SubmitCommand = new CreateCommand(modalNavigationStore, progressBarWindowViewModel, createNamecardModPageViewModel);
            CancelCommand = new CloseModalCommand(this.modalNavigationStore);
        }


        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
    }
}
