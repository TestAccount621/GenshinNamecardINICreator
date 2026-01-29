using GenshinNamecardAutomater.classes;
using System.Windows;

namespace GenshinNamecardAutomater.UI
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {

        public ConfirmationWindow()
        {
            InitializeComponent();
            cbx_SelectNamecardToApply.ItemsSource = MainWindow.Namecards;
        }

        private void Button_Click_Confirm(object sender, RoutedEventArgs e)
        {
            if (cbx_SelectNamecardToApply.SelectedItem is NamecardData namecard)
            {
                var t = new NamecardMergedINI(MainWindow.changedFolderCollection, namecard);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a namecard for these gifs to apply to.");
            }
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
