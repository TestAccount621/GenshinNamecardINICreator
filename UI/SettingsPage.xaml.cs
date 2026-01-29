using GenshinNamecardINICreator.Properties;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GenshinNamecardINICreator.UI
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page, INotifyPropertyChanged
    {
        // These are only for keeping track of the new hotkey to place.
        private List<Key> keysPressed = [];
        private string keysPressedText = "";

        // These are for Saving as well as updating the UI on changes.
        private string _keyPauseText = "";
        public string keyPauseText
        {
            get => _keyPauseText;
            set
            {
                if (_keyPauseText != value)
                {
                    _keyPauseText = value;
                    NotifyPropertyChanged("keyPauseText");
                }
            }
        }
        private string _keyCycleFText = "";
        public string keyCycleFText
        {
            get => _keyCycleFText;
            set
            {
                if (_keyCycleFText != value)
                {
                    _keyCycleFText = value;
                    NotifyPropertyChanged("keyCycleFText");
                }
            }
        }
        private string _keyCycleBText = "";
        public string keyCycleBText
        {
            get => _keyCycleBText;
            set
            {
                if (_keyCycleBText != value)
                {
                    _keyCycleBText = value;
                    NotifyPropertyChanged("keyCycleBText");
                }
            }
        }

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = this;
            cbx_RandomLogin.SelectedIndex = Settings.Default.RandomLogin;
            keyPauseText = Settings.Default.KeyPause;
            keyCycleFText = Settings.Default.KeyCycleForward;
            keyCycleBText = Settings.Default.KeyCycleBackward;
        }

        private void cbx_RandomLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_RandomLogin.SelectedIndex != -1)
            {
                if (Settings.Default.RandomLogin != cbx_RandomLogin.SelectedIndex)
                {
                    Settings.Default.RandomLogin = cbx_RandomLogin.SelectedIndex;
                    Settings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Saves the changes to the settings.
        /// </summary>
        private void Save()
        {
            if (Validate())
            {
                // Don't need to save RandomLogin here since it saves anytime the combobox changes.
                //Settings.Default.RandomLogin = cbx_RandomLogin.SelectedIndex;
                Settings.Default.KeyPause = txtBox_KeyPause.Text;
                Settings.Default.KeyCycleForward = txtBox_KeyCycleForward.Text;
                Settings.Default.KeyCycleBackward = txtBox_KeyCycleBackward.Text;
                Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("One or more of the keybinds are empty. Please put in a keybind for the toggle.");
            }
        }

        private bool Validate()
        {
            return !String.IsNullOrWhiteSpace(txtBox_KeyPause.Text)
                || !String.IsNullOrWhiteSpace(txtBox_KeyCycleForward.Text)
                || !String.IsNullOrWhiteSpace(txtBox_KeyCycleBackward.Text);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void TextBoxHotKeys_KeyDown(object sender, KeyEventArgs e)
        {
            // The text box grabs all input.
            e.Handled = true;

            // Fetch the actual shortcut key.
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);

            // While a key is still being pressed to change the hotkey, check if a duplicate key is being pressed after release.
            if (keysPressed.Contains(key)) { return; }
            keysPressed.Add(key);

            // Cancel the input if Escape is pressed.
            if (key is Key.Escape) { return; }
            // Cancel the input if the key has already been pressed and released before.
            if (CheckHotkeyStringForDuplicate(keysPressedText, key)) { return; }

            // Build the shortcut key name.
            StringBuilder shortcutText = new StringBuilder(keysPressedText);
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                if (shortcutText.Equals("")) { shortcutText.Append("ctrl"); }
                else { shortcutText.Append(" ctrl"); }
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                if (shortcutText.Equals("")) { shortcutText.Append("shift"); }
                else { shortcutText.Append(" shift"); }
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                if (shortcutText.Equals("")) { shortcutText.Append("alt"); }
                else { shortcutText.Append(" alt"); }
            }

            // Ignore modifier keys. Prevents duplicate string entry for them.
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }

            if (shortcutText.Equals("")) { shortcutText.Append(key.ToString()); }
            else { shortcutText.Append(" " + key.ToString()); }

            keysPressedText = shortcutText.ToString().ToLower();
        }
        /// <summary>
        /// Controls updating currently pressed down keys for changing the hotkeys for the namecard mod.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            keysPressed.Remove(key);
            if (keysPressed.Count == 0)
            {
                if (sender is TextBox textBox)
                {
                    var output = ConvertSingleCharacterHotkey(keysPressedText);
                    switch (textBox.Name)
                    {
                        case "txtBox_KeyCycleBackward":
                            keyCycleBText = output.Equals("") ? keyCycleBText : output;
                            break;
                        case "txtBox_KeyCycleForward":
                            keyCycleFText = output.Equals("") ? keyCycleFText : output;
                            break;
                        case "txtBox_KeyPause":
                            keyPauseText = output.Equals("") ? keyPauseText : output;
                            break;
                    }
                    keysPressedText = "";
                    Save();
                }
            }
        }
        /// <summary>
        /// Creates the final text string for the changing hotkey. Retuns an empty string if only a modifier was 
        /// held the entire time, the original string if it has a modifier, or adds "no_modifiers" if no modifiers were held.
        /// </summary>
        /// <param name="text">The text string of the held keys for the hotkey.</param>
        /// <returns>The final text output for the hotkey.</returns>
        private string ConvertSingleCharacterHotkey(string text)
        {
            var split = text.Split(" ");
            if (split.Count() == 1)
            {
                if (split[0].Equals("ctrl") || split[0].Equals("shift") || split[0].Equals("alt"))
                {
                    return "";
                }
            }
            else if (split.Count() > 0)
            {
                bool hasModifier = false;
                foreach (var item in split)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.Equals("ctrl") || item.Equals("shift") || item.Equals("alt"))
                        {
                            hasModifier = true;
                            break;
                        }
                    }
                }
                if (!hasModifier)
                {
                    return "no_modifiers " + text;
                }
                else
                {
                    return text;
                }
            }
            return text;
        }
        /// <summary>
        /// Checks if a key was pressed a second time while updating the hotkey.
        /// </summary>
        /// <param name="text">The currently saved keys</param>
        /// <param name="key">The currently pressed down key to compare</param>
        /// <returns>True or False whether "key" was apready pressed before.</returns>
        private bool CheckHotkeyStringForDuplicate(string text, Key key)
        {
            var split = text.Split(" ");
            if ((split.Count() > 1))
            {
                foreach (var item in split)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (item.Equals(key.ToString()))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
