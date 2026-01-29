using GenshinNamecardINICreator.classes;
using GenshinNamecardINICreator.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GenshinNamecardINICreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string NamecardHashesFileName = "namecard_hashes.json";
        UI.ConvertPage ConverGIFPage { get; set; }
        UI.CreateCollectionPage CollectionPage { get; set; }
        UI.HashesPage HashesPage { get; set; }
        UI.SettingsPage SettingsPage { get; set; }
        public static ObservableCollection<NamecardData> Namecards = new ObservableCollection<NamecardData>();
        public static ObservableCollection<DirectoryInfo> changedFolderCollection = new ObservableCollection<DirectoryInfo>();



        public MainWindow()
        {
            InitializeComponent();
            txtBox_OutputFolder.Text = Directory.Exists(Settings.Default.OutputFolder) ? Settings.Default.OutputFolder : AppDomain.CurrentDomain.BaseDirectory;
            ConverGIFPage = new UI.ConvertPage();
            CollectionPage = new UI.CreateCollectionPage();
            HashesPage = new UI.HashesPage();
            SettingsPage = new UI.SettingsPage();
            frame_PageSelected.Content = CollectionPage;
            if (File.Exists(NamecardHashesFileName))
            {
                var text = File.ReadAllText(NamecardHashesFileName);
                var list = JsonConvert.DeserializeObject<List<NamecardData>>(text);
                if (list != null)
                {
                    Namecards.Clear();
                    foreach (var item in list)
                    {
                        Namecards.Add(item);
                    }
                }
            }
        }

        private void Button_Click_PageChange(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                AdjustButtonTags(button);
                frame_PageSelected.NavigationService.RemoveBackEntry();
                if (button.Name.Equals("btn_GIFConvert")) { frame_PageSelected.Content = ConverGIFPage; }
                else if (button.Name.Equals("btn_Settings")) { frame_PageSelected.Content = SettingsPage; }
                else if (button.Name.Equals("btn_HashesChange")) { frame_PageSelected.Content = HashesPage; }
                else if (button.Name.Equals("btn_INICreation")) { frame_PageSelected.Content = CollectionPage; }
            }

        }

        private void AdjustButtonTags(Button button)
        {
            btn_GIFConvert.Tag = btn_GIFConvert == button ? "Active" : "";
            btn_Settings.Tag = btn_Settings == button ? "Active" : "";
            btn_HashesChange.Tag = btn_HashesChange == button ? "Active" : "";
            btn_INICreation.Tag = btn_INICreation == button ? "Active" : "";
        }

        private void Button_Click_SelectOutputFolder(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var dialog = new OpenFolderDialog
                {
                    Multiselect = false,
                    InitialDirectory = !Directory.Exists(Settings.Default.OutputFolder)
                    ? AppDomain.CurrentDomain.BaseDirectory
                    : Settings.Default.OutputFolder
                };
                var result = dialog.ShowDialog();
                if (result == true)
                {
                    var directoryInfo = new DirectoryInfo(dialog.FolderName);
                    txtBox_OutputFolder.Text = directoryInfo.FullName;
                    Settings.Default.OutputFolder = directoryInfo.FullName;
                    Settings.Default.Save();
                    RefreshCollectionList();
                }
            }
        }

        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            RefreshCollectionList();
        }

        public void RefreshCollectionList()
        {
            var directory = new DirectoryInfo(Settings.Default.OutputFolder);
            if (directory.Exists)
            {
                if (frame_PageSelected.Content is UI.CreateCollectionPage)
                {
                    CollectionPage.RefreshCollectionSorting();
                }
            }
        }
    }
}