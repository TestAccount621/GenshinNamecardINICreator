using GenshinNamecardINICreator.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GenshinNamecardINICreator.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private string _keyCycleForward = Settings.Default.KeyCycleForward;
        private string _keyCycleBackward = Settings.Default.KeyCycleBackward;
        private string _keyCycleRandom = Settings.Default.KeyCycleRandom;
        private string _keyPause = Settings.Default.KeyPause;
        private int _randomLogin = Settings.Default.RandomLogin;

        public string KeyCycleForward 
        { 
            get { return _keyCycleForward; }
            set 
            {  
                _keyCycleForward = value;
                Settings.Default.KeyCycleForward = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(KeyCycleForward));
            }
        }
        public string KeyCycleBackward
        {
            get { return _keyCycleBackward; }
            set
            {
                _keyCycleBackward = value;
                Settings.Default.KeyCycleBackward = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(KeyCycleBackward));
            }
        }
        public string KeyCycleRandom
        {
            get { return _keyCycleRandom; }
            set
            {
                _keyCycleRandom = value;
                Settings.Default.KeyCycleRandom = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(KeyCycleRandom));
            }
        }
        public string KeyPause
        {
            get { return _keyPause; }
            set
            {
                _keyPause = value;
                Settings.Default.KeyPause = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(KeyPause));
            }
        }
        public int RandomLogin
        {
            get { return _randomLogin; }
            set
            {
                _randomLogin = value;
                Settings.Default.RandomLogin = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(RandomLogin));
            }
        }

        // These variables are only for the key events.
        private List<Key> keysPressed = [];
        private string keysPressedText = "";
        private Dictionary<string, string> keyConversions = new Dictionary<string, string>() 
        {
            {"d0", "0" },
            {"d1", "1" },
            {"d2", "2" },
            {"d3", "3" },
            {"d4", "4" },
            {"d5", "5" },
            {"d6", "6" },
            {"d7", "7" },
            {"d8", "8" },
            {"d9", "9" },
            {"oem1", ";" },
            {"oem3", "`" },
            {"oem6", "]" },
            {"oem5", @"\" },
            {"oemopenbrackets", "[" },
            {"oemquotes", "'" },
            {"oemminus", "vk_oem_minus" },
            {"oemplus", "vk_oem_plus" },
            {"oemcomma", "," },
            {"oemquestion", "/" },
            {"oemperiod", "." },
        };

        public void RandomLoginSelectionChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cbx)
            {
                if (cbx.SelectedIndex != -1 && cbx.SelectedIndex != Settings.Default.RandomLogin)
                {
                    RandomLogin = cbx.SelectedIndex;
                }
            }
         
        }

        public void TextBoxHotKeys_KeyDown(object sender, KeyEventArgs e)
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
                else if (!shortcutText.ToString().Contains("ctrl")) { shortcutText.Append(" ctrl"); }
            }
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                if (shortcutText.Equals("")) { shortcutText.Append("shift"); }
                else if (!shortcutText.ToString().Contains("shift")) { shortcutText.Append(" shift"); }
            }
            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                if (shortcutText.Equals("")) { shortcutText.Append("alt"); }
                else if (!shortcutText.ToString().Contains("alt")) { shortcutText.Append(" alt"); }
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
        public void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            keysPressed.Remove(key);
            if (keysPressed.Count == 0)
            {
                if (sender is TextBox textBox)
                {
                    var output = ConvertHotkeysToFinalString(keysPressedText);
                    switch (textBox.Name)
                    {
                        case "txtBox_KeyCycleRandom":
                            KeyCycleRandom = output.Equals("") ? KeyCycleRandom : output;
                            break;
                        case "txtBox_KeyCycleBackward":
                            KeyCycleBackward = output.Equals("") ? KeyCycleBackward : output;
                            break;
                        case "txtBox_KeyCycleForward":
                            KeyCycleForward = output.Equals("") ? KeyCycleForward : output;
                            break;
                        case "txtBox_KeyPause":
                            KeyPause = output.Equals("") ? KeyPause : output;
                            break;
                    }
                    keysPressedText = "";

                    // These lines just remove the focus from the textbox after the keybind is set to change the background back to default.
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(textBox), null);
                    Keyboard.ClearFocus();
                }
            }
        }
        /// <summary>
        /// Creates the final text string for the changing hotkey. Retuns an empty string if only a modifier was 
        /// held the entire time, the original string if it has a modifier, or adds "no_modifiers" if no modifiers were held.
        /// </summary>
        /// <param name="text">The text string of the held keys for the hotkey.</param>
        /// <returns>The final text output for the hotkey.</returns>
        private string ConvertHotkeysToFinalString(string text)
        {
            var split = text.Split(" ");
            string result = "";
            if (split.Length == 1)
            {
                if (split[0].Equals("ctrl") || split[0].Equals("shift") || split[0].Equals("alt"))
                {
                    return "";
                }
            }
            if (split.Length > 0)
            {
                List<string> keys = [];
                bool hasModifier = false;
                for (int i = 0; i < split.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(split[i]))
                    {
                        if (keyConversions.ContainsKey(split[i])) { keys.Add(keyConversions[split[i]]); }
                        else if (split[i].Equals("ctrl") || split[i].Equals("shift") || split[i].Equals("alt"))
                        {
                            hasModifier = true;
                            if (!keys.Contains(split[i])) { keys.Insert(0, split[i]); }
                        }
                        else { keys.Add(split[i]); }
                    }
                }
                if (keys.Count > 0)
                {
                    if (!hasModifier) { keys.Insert(0, "no_modifiers"); }
                    result = string.Join(" ", keys);
                }
            }
            return result;
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
                        if (item.Equals(key.ToString().ToLower()))
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
