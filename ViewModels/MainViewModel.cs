using GenshinNamecardINICreator.Commands;
using GenshinNamecardINICreator.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenshinNamecardINICreator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly NamecardHashesStore namecardHashesStore;

        public BaseViewModel CurrentModalViewModel => modalNavigationStore.CurrentViewModel;
        public bool IsModalOpen => modalNavigationStore.IsOpen;
        public HomeViewViewModel HomeViewViewModel { get; }
        public ConfirmationWindowViewModel ConfirmationWindowViewModel { get; }
        public ProgressBarWindowViewModel ProgressBarWindowViewModel { get; }

        public MainViewModel(ModalNavigationStore modalNavigationStore,
                             NamecardHashesStore namecardHashesStore,
                             ListViewsRefreshStore listViewsRefreshStore,
                             HomeViewViewModel homeViewViewModel,
                             ProgressBarWindowViewModel progressBarWindowViewModel,
                             CreateNamecardModPageViewModel createNamecardModPageViewModel)
        {
            this.namecardHashesStore = namecardHashesStore;
            this.modalNavigationStore = modalNavigationStore;

            ICommand cancelCommand = new CloseModalCommand(this.modalNavigationStore);

            ConfirmationWindowViewModel = new ConfirmationWindowViewModel(namecardHashesStore, modalNavigationStore, progressBarWindowViewModel, createNamecardModPageViewModel);
            ProgressBarWindowViewModel = progressBarWindowViewModel;
            HomeViewViewModel = homeViewViewModel;
            this.modalNavigationStore.CurrentViewModelChanged += ModalNavigationStore_CurrentViewModelChanged;
        }

        private void ModalNavigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));

        }

        protected override void Dispose()
        {
            this.modalNavigationStore.CurrentViewModelChanged -= ModalNavigationStore_CurrentViewModelChanged;

            base.Dispose();
        }
        public void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            namecardHashesStore.Save();
        }
    }
}
