using GenshinNamecardAutomater.Properties;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace GenshinNamecardAutomater.classes
{
    public class NamecardMergedINI
    {
        private string _fileName = "NameCardCollection.ini";
        private string _hashA = "";
        private string _hashD = "";
        private string _hashE = "";
        private string _parentDirectory;
        public NamecardMergedINI(ObservableCollection<DirectoryInfo> directories, NamecardData namecard)
        {
            _parentDirectory = directories[0].Parent.FullName;
            string mergedINIpath = Path.Combine(_parentDirectory, _fileName);
            int swapmax = 0;
            foreach (DirectoryInfo d in directories)
            {
                var namecardINI = new NamecardSingleINI(d, swapmax, namecard);
                swapmax++;
            }
            if (swapmax > 0)
            {
                CreateMergedINI(mergedINIpath, swapmax, namecard);
            }
            MessageBox.Show("Namecard mod created!");
        }

        private void CreateMergedINI(string path, int swapmax, NamecardData namecard)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var fw = new StreamWriter(fs))
                    {
                        fw.WriteLine("namespace = NameCardCollection");
                        fw.WriteLine("");
                        fw.WriteLine("[Constants]");
                        fw.WriteLine("global $active");
                        fw.WriteLine("global persist $swapvar = 0");
                        fw.WriteLine("; The total number of namecard folders");
                        fw.WriteLine(String.Format("global persist $swapmax = {0}", swapmax));
                        fw.WriteLine("; Change this to set the startup namecard when random login is off");
                        fw.WriteLine("global persist $swapcard = 0");
                        fw.WriteLine("global persist $toggletogglevar = 1");
                        fw.WriteLine("; Set this to 1 to turn if on or 0 for off");
                        fw.WriteLine(String.Format("global persist $randomLogin = {0}", Settings.Default.RandomLogin));
                        fw.WriteLine("");
                        fw.WriteLine("");
                        fw.WriteLine("[KeyPause]");
                        fw.WriteLine("condition = $active == 1");
                        fw.WriteLine(String.Format("key = {0}", Settings.Default.KeyPause));
                        fw.WriteLine("type = cycle");
                        fw.WriteLine("$toggletogglevar = 0,1");
                        fw.WriteLine("");
                        fw.WriteLine("[KeyToggle]");
                        fw.WriteLine("condition = $active == 1");
                        fw.WriteLine(String.Format("key = {0}", Settings.Default.KeyCycleForward));
                        fw.WriteLine(String.Format("back = {0}", Settings.Default.KeyCycleBackward));
                        fw.WriteLine("type = cycle");
                        fw.WriteLine("$swapvar = 0,1");
                        fw.WriteLine("");
                        fw.WriteLine("[Present]");
                        fw.WriteLine("post $active = 0");
                        fw.WriteLine("");
                        fw.WriteLine("; Random selection on login");
                        fw.WriteLine("if $randomLogin == 1");
                        fw.WriteLine("\t$randomLogin = 2");
                        fw.WriteLine("\trun = CommandListRandomLogin");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("if $swapvar == 1");
                        fw.WriteLine("\t$swapvar = 0");
                        fw.WriteLine("\t$swapcard = $swapcard + 1");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("if ($swapcard >= $swapmax) || ($swapcard < 0)");
                        fw.WriteLine("\t$swapcard = 0");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardA]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.MainHash));
                        fw.WriteLine("$active = 1");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardD]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.PreviewHash));
                        fw.WriteLine("$active = 1");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardE]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.BannerHash));
                        fw.WriteLine("$active = 1");
                        fw.WriteLine("");
                        fw.WriteLine("[CommandListRandomLogin]");
                        fw.WriteLine("$\\math\\rand\\min = 0");
                        fw.WriteLine("$\\math\\rand\\max = $swapmax");
                        fw.WriteLine("run = commandlist\\math\\rand\\run");
                        fw.WriteLine("$swapcard = $\\math\\rand\\out // 1");
                        fw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
