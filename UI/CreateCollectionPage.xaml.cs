using GenshinNamecardINICreator.Properties;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GenshinNamecardINICreator.UI
{
    /// <summary>
    /// Interaction logic for CreateCollectionPage.xaml
    /// </summary>
    public partial class CreateCollectionPage : Page
    {
        private ObservableCollection<DirectoryInfo> baseFolderCollection = new ObservableCollection<DirectoryInfo>();

        public CreateCollectionPage()
        {
            InitializeComponent();
            RefreshCollectionSorting();
        }
        /// <summary>
        /// This sorts the "In the collection" listview section so that if some of the folders are already merged and sorted previously, it will keep that order.
        /// </summary>
        public void RefreshCollectionSorting()
        {
            if (String.IsNullOrEmpty(Settings.Default.OutputFolder))
            {
                Settings.Default.OutputFolder = AppDomain.CurrentDomain.BaseDirectory;
                Settings.Default.Save();
            }
            var directory = new DirectoryInfo(Settings.Default.OutputFolder);
            if (directory.Exists)
            {
                baseFolderCollection.Clear();
                MainWindow.changedFolderCollection.Clear();
                var folders = directory.GetDirectories();
                if (folders.Count() > 0)
                {
                    List<Tuple<DirectoryInfo, int>> list = [];
                    foreach (var folder in folders)
                    {
                        // Exclude all folders that do not even have a .png in it.
                        var hasImages = folder.GetFiles().Where(x => x.Extension.Equals(".png"));
                        if (hasImages.Any())
                        {
                            // Check if the folder is already part of the merge essentially.
                            var files = folder.GetFiles().Where(x => x.Name.Equals("namecard.ini"));
                            bool found = false;
                            if (files.Any())
                            {
                                foreach (var file in files)
                                {
                                    // Checks the 5th line of the ini since that SHOULD be where the $swapcard variable is placed.
                                    var line = File.ReadLines(file.FullName).Skip(5).FirstOrDefault();
                                    if (line != null)
                                    {
                                        try
                                        {
                                            string numberString = new string(line.Where(Char.IsDigit).ToArray());
                                            int number = int.Parse(numberString);
                                            if (number >= 0)
                                            {
                                                list.Add(new Tuple<DirectoryInfo, int>(folder, number));
                                            }
                                            else
                                            {
                                                list.Add(new Tuple<DirectoryInfo, int>(folder, 9999));
                                            }
                                            MainWindow.changedFolderCollection.Add(folder);
                                            found = true;
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                            throw;
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                found = false;
                                continue;
                            }
                            baseFolderCollection.Add(folder);
                        }
                    }
                    baseFolderCollection = new ObservableCollection<DirectoryInfo>(baseFolderCollection.OrderBy(x => x.Name));
                    lv_NotInCollection.ItemsSource = baseFolderCollection;
                    if (list.Count > 0)
                    {
                        var sorted = list.OrderBy(x => x.Item2);
                        ObservableCollection<DirectoryInfo> _newList = [];
                        foreach (var item in sorted)
                        {
                            _newList.Add(item.Item1);
                        }
                        lv_InCollection.ItemsSource = MainWindow.changedFolderCollection = _newList;
                    }
                    else
                    {
                        lv_InCollection.ItemsSource = MainWindow.changedFolderCollection;
                    }
                }
            }
            else
            {
                Settings.Default.OutputFolder = AppDomain.CurrentDomain.BaseDirectory;
                Settings.Default.Save();
            }
        }

        private void Button_Click_RightArrow(object sender, RoutedEventArgs e)
        {
            if (lv_NotInCollection.SelectedItems.Count >= 1)
            {
                for (int i = lv_NotInCollection.SelectedItems.Count - 1; i >= 0; i--)
                {
                    var d = (DirectoryInfo)lv_NotInCollection.SelectedItems[i];
                    if (!MainWindow.changedFolderCollection.Contains(d))
                    {
                        MainWindow.changedFolderCollection.Add(d);
                        baseFolderCollection.Remove(d);
                    }
                }
                lv_NotInCollection.UnselectAll();
            }
        }

        private void Button_Click_LeftArrow(object sender, RoutedEventArgs e)
        {
            if (lv_InCollection.SelectedItems.Count >= 1)
            {
                if (lv_InCollection.SelectedItems[0] is DirectoryInfo)
                {
                    for (int i = lv_InCollection.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        var d = (DirectoryInfo)lv_InCollection.SelectedItems[i];
                        if (MainWindow.changedFolderCollection.Contains(d))
                        {
                            MainWindow.changedFolderCollection.Remove(d);
                            baseFolderCollection.Add(d);
                        }
                    }
                    baseFolderCollection = new ObservableCollection<DirectoryInfo>(baseFolderCollection.OrderBy(x => x.Name));
                    lv_NotInCollection.ItemsSource = baseFolderCollection;
                }
            }
        }

        private void Button_Click_Up(object sender, RoutedEventArgs e)
        {
            MoveItemsInCollection(-1);
        }

        private void Button_Click_Down(object sender, RoutedEventArgs e)
        {
            MoveItemsInCollection(1);
        }

        /// <summary>
        /// Allows the up/down arrows to work.
        /// </summary>
        /// <param name="amount">1 for the down arrow and -1 for the up arrow</param>
        private void MoveItemsInCollection(int amount)
        {
            if (lv_InCollection != null)
            {
                if (lv_InCollection.SelectedItems.Count > 0)
                {
                    for (int i = lv_InCollection.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        if (lv_InCollection.SelectedItems[i] is DirectoryInfo directory)
                        {
                            if (MainWindow.changedFolderCollection.Contains(directory))
                            {
                                int index = MainWindow.changedFolderCollection.IndexOf(directory);
                                if ((amount == 1 && index < MainWindow.changedFolderCollection.Count - 1) || (amount == -1 && index > 0))
                                {
                                    MainWindow.changedFolderCollection.Move(index, index + amount);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Button_Click_Create(object sender, RoutedEventArgs e)
        {
            var window = new ConfirmationWindow();
            window.Owner = Application.Current.MainWindow;
            window.Show();
        }
    }
}
