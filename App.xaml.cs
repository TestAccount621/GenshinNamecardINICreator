using GenshinNamecardINICreator.ViewModels;
using System.Windows;
using GenshinNamecardINICreator.Stores;

namespace GenshinNamecardINICreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NamecardHashesStore namecardHashesStore;
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly ListViewsRefreshStore listViewsRefreshStore;

        public App()
        {
            namecardHashesStore = new NamecardHashesStore();
            modalNavigationStore = new ModalNavigationStore();
            listViewsRefreshStore = new ListViewsRefreshStore();

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            ProgressBarWindowViewModel progressBarWindowViewModel = new(this.modalNavigationStore);
            CreateNamecardModPageViewModel createNamecardModPageViewModel = new(modalNavigationStore, namecardHashesStore, listViewsRefreshStore, progressBarWindowViewModel);
            HomeViewViewModel homeViewViewModel = new(modalNavigationStore, namecardHashesStore, listViewsRefreshStore, progressBarWindowViewModel);
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(modalNavigationStore, namecardHashesStore, listViewsRefreshStore, homeViewViewModel, progressBarWindowViewModel, createNamecardModPageViewModel)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }
    }

}
