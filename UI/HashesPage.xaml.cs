using GenshinNamecardINICreator.classes;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GenshinNamecardINICreator.UI
{
    /// <summary>
    /// Interaction logic for HashesPage.xaml
    /// </summary>
    public partial class HashesPage : Page, INotifyPropertyChanged
    {
        private string _hashNameText = "";
        public string HashNameText
        {
            get { return _hashNameText; }
            set { _hashNameText = value; NotifyPropertyChanged("HashNameText"); }
        }
        private string _hashMainText = "";
        public string HashMainText
        {
            get { return _hashMainText; }
            set { _hashMainText = value; NotifyPropertyChanged("HashMainText"); }
        }
        private string _hashBannerText = "";
        public string HashBannerText
        {
            get { return _hashBannerText; }
            set { _hashBannerText = value; NotifyPropertyChanged("HashBannerText"); }
        }
        private string _hashPreviewText = "";
        public string HashPreviewText
        {
            get { return _hashPreviewText; }
            set { _hashPreviewText = value; NotifyPropertyChanged("HashPreviewText"); }
        }
        private string _textBoxTagValue = "";
        /// <summary>
        /// The only options that do anything are 'New' and 'Edit', otherwise set to '' for viewing only. This controls all aspects of the page essentially.
        /// </summary>
        public string TextBoxTagValue
        {
            get { return _textBoxTagValue; }
            set { _textBoxTagValue = value; NotifyPropertyChanged("TextBoxTagValue"); }
        }

        public HashesPage()
        {
            InitializeComponent();
            DataContext = this;
            ListCollectionView listColectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(MainWindow.Namecards);
            listColectionView.IsLiveSorting = true;
            listColectionView.LiveSortingProperties.Add(nameof(NamecardData.Name));
            listColectionView.SortDescriptions.Add(new SortDescription(nameof(NamecardData.Name), ListSortDirection.Ascending));
            cbx_KnownHashes.ItemsSource = listColectionView;
        }

        private bool CompareNamecardData(NamecardData data)
        {
            if (MainWindow.Namecards.Count > 0)
            {
                foreach (var n in MainWindow.Namecards)
                {
                    if (n.Equals(data)) { return true; }
                }
            }
            return false;
        }

        private void SaveNamecards()
        {
            string json = JsonConvert.SerializeObject(MainWindow.Namecards, Formatting.Indented);
            File.WriteAllText(MainWindow.NamecardHashesFileName, json);
        }

        private void Button_Click_NewOrEdit(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Content.Equals("New"))
                {
                    HashBannerText = "";
                    HashMainText = "";
                    HashNameText = "";
                    HashPreviewText = "";
                }
                TextBoxTagValue = button.Content.ToString();
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (cbx_KnownHashes.SelectedItem is NamecardData namecard)
                {

                    var message = String.Format("Are you sure you want to remove \"{0}\" from the namecard list?", namecard.Name);
                    if (MessageBox.Show(message, "Delete?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        MainWindow.Namecards.Remove(namecard);
                        SaveNamecards();
                    }
                }
            }
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            //Validate data
            if (!Validation.GetHasError(txtBox_BannerHash)
                && !Validation.GetHasError(txtBox_PreviewHash)
                && !Validation.GetHasError(txtBox_MainHash)
                && !Validation.GetHasError(txtBox_Name))
            {
                NamecardData entry = new NamecardData(txtBox_Name.Text, txtBox_MainHash.Text, txtBox_PreviewHash.Text, txtBox_BannerHash.Text);

                if (TextBoxTagValue.Equals("New"))
                {
                    if (CompareNamecardData(entry))
                    {
                        MessageBox.Show("This hash combination already exists.");
                        return;
                    }
                    MainWindow.Namecards.Add(entry);
                    TextBoxTagValue = "Save";
                    cbx_KnownHashes.Items.Refresh();
                    cbx_KnownHashes.SelectedItem = entry;
                }
                else if (TextBoxTagValue.Equals("Edit"))
                {
                    if (cbx_KnownHashes.SelectedItem is NamecardData original)
                    {
                        if (entry.Equals(original) && entry.Name.Equals(original.Name))
                        {
                            TextBoxTagValue = "Save";
                            return;
                        }
                        else
                        {
                            int index = MainWindow.Namecards.IndexOf(original);
                            MainWindow.Namecards[index].Update(entry);
                            TextBoxTagValue = "Save";
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all the namecard data marked in red.");
                return;
            }
            SaveNamecards();
        }

        private void cbx_KnownHashes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedItem is NamecardData namecard)
                {
                    HashBannerText = namecard.BannerHash;
                    HashMainText = namecard.MainHash;
                    HashNameText = namecard.Name;
                    HashPreviewText = namecard.PreviewHash;
                }
                else
                {
                    HashBannerText = "";
                    HashMainText = "";
                    HashNameText = "";
                    HashPreviewText = "";
                }
            }
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            // Finish the hash list stuff and bind it to the combobox. This will set the text properties to the currently selected item there.
            if (sender is Button button)
            {
                if (cbx_KnownHashes.SelectedItem is NamecardData namecard)
                {
                    HashBannerText = namecard.BannerHash;
                    HashMainText = namecard.MainHash;
                    HashNameText = namecard.Name;
                    HashPreviewText = namecard.PreviewHash;
                }
                else
                {
                    HashBannerText = "";
                    HashMainText = "";
                    HashNameText = "";
                    HashPreviewText = "";

                }
                TextBoxTagValue = button.Content.ToString();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void UpdateNamecardList()
        {
            if (cbx_KnownHashes.SelectedIndex != -1)
            {
                if (cbx_KnownHashes.SelectedItem is NamecardData selected)
                {

                }
            }
        }
    }
}
