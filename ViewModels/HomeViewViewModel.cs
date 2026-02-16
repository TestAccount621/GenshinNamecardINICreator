using GenshinNamecardINICreator.Commands;
using GenshinNamecardINICreator.Properties;
using GenshinNamecardINICreator.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenshinNamecardINICreator.ViewModels
{
    public class HomeViewViewModel : BaseViewModel
    {
        private readonly ListViewsRefreshStore _listViewsRefreshStore;
        public HomeViewViewModel(ModalNavigationStore modalNavigationStore,
                                 NamecardHashesStore namecardHashesStore,
                                 ListViewsRefreshStore listViewsRefreshStore,
                                 ProgressBarWindowViewModel progressBarWindowViewModel)
        {
            _listViewsRefreshStore = listViewsRefreshStore;

            CreateNamecardModPageViewModel = new CreateNamecardModPageViewModel(modalNavigationStore, namecardHashesStore, listViewsRefreshStore, progressBarWindowViewModel);
            ConvertPageViewModel = new ConvertPageViewModel(listViewsRefreshStore, progressBarWindowViewModel, modalNavigationStore);
            SettingsPageViewModel = new SettingsPageViewModel();
            NamecardHashesPageViewModel = new NamecardHashesPageViewModel(namecardHashesStore);

            ChangeViewCommand = new ChangeHomeViewViewCommand(this);
            SelectFolderCommand = new SelectFolderCommand(this);
            RefreshFolderCommand = new RefreshFolderCommand(listViewsRefreshStore);
            ActiveView = CreateNamecardModPageViewModel;
        }

        public CreateNamecardModPageViewModel CreateNamecardModPageViewModel { get; }
        public ConvertPageViewModel ConvertPageViewModel { get; }
        public SettingsPageViewModel SettingsPageViewModel { get; }
        public NamecardHashesPageViewModel NamecardHashesPageViewModel { get; }

        private BaseViewModel _activeView;
        public BaseViewModel ActiveView
        {
            get
            {
                return _activeView;
            }
            set
            {
                _activeView = value;
                OnPropertyChanged(nameof(ActiveView));
            }
        }
        private string _destinationFolderPath = string.IsNullOrWhiteSpace(Settings.Default.OutputFolder) 
            ? AppDomain.CurrentDomain.BaseDirectory 
            : Settings.Default.OutputFolder;
        public string DestinationFolderPath
        {
            get
            {
                return _destinationFolderPath;
            }
            set
            {
                _destinationFolderPath = value;
                Settings.Default.OutputFolder = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(DestinationFolderPath));
                _listViewsRefreshStore.Refresh();
            }
        }

        public ICommand ChangeViewCommand { get; }
        public ICommand SelectFolderCommand { get; }
        public ICommand RefreshFolderCommand { get; }
    }
}
