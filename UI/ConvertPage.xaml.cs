using GenshinNamecardAutomater.classes;
using GenshinNamecardAutomater.Properties;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GenshinNamecardAutomater.UI
{
    /// <summary>
    /// Interaction logic for ConverGIFPage.xaml
    /// </summary>
    public partial class ConvertPage : Page
    {
        private ObservableCollection<FileInfo> Files = [];
        private bool _lockButton = false;
        public ConvertPage()
        {
            InitializeComponent();
            Files = [];
        }

        private void Button_Click_Open(object sender, RoutedEventArgs e)
        {
            bool update = false;
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
                if (_files.Count() > 0)
                {
                    foreach (var f in _files)
                    {
                        FileInfo fileInfo = new FileInfo(f);
                        if (!Files.Contains(fileInfo))
                        {
                            Files.Add(fileInfo);
                            update = true;
                        }
                    }
                }
                if (update)
                {
                    UpdateListView();
                }
            }
        }

        private void Button_Click_Remove(object sender, RoutedEventArgs e)
        {
            bool update = false;
            if (lv_SelectedGIFs.SelectedItems.Count > 0)
            {
                string message = "Are you sure you want to remove the selected GIF files from the list?";
                var result = MessageBox.Show(message, "Confirm Removal?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (var f in lv_SelectedGIFs.SelectedItems)
                    {
                        if (f is FileInfo file)
                        {
                            if (Files.Contains(file))
                            {
                                Files.Remove(file);
                                update = true;
                            }
                        }
                    }
                }
                if (update)
                {
                    UpdateListView();
                }
            }
        }

        private void UpdateListView()
        {
            var sorted = Files.OrderBy(x => x.FullName);
            lv_SelectedGIFs.ItemsSource = sorted;
        }

        private async void ConvertTask(ObservableCollection<FileInfo> files)
        {
            await Task.Run(() =>
            {
            });
        }

        private async void Button_Click_Convert(object sender, RoutedEventArgs e)
        {
            if (!_lockButton)
            {
                _lockButton = true;
                if (Files.Count > 0)
                {
                    var convert = new ConvertToNamecardFormat(Settings.Default.OutputFolder);
                    foreach (var item in Files)
                    {
                        convert.BeginConversion(item.FullName);
                    }
                    Files.Clear();
                    lv_SelectedGIFs.ItemsSource = null;
                    MessageBox.Show("All images have been converted to the namecard format.");
                }
                _lockButton = false;
            }
        }
        /// <summary>
        /// The entire purpose of this SelectionChanged is that I don't want to do a converter right now for something this silly/simple
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lv_SelectedGIFs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lv_SelectedGIFs.Items.Count > 0)
            {
                if (lv_SelectedGIFs.SelectedItems.Count > 0)
                {
                    btn_Remove.IsEnabled = true;
                    return;
                }
            }
            btn_Remove.IsEnabled = false;
        }
    }
}
