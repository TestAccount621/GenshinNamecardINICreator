using GenshinNamecardINICreator.Models;
using GenshinNamecardINICreator.Properties;
using GenshinNamecardINICreator.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace GenshinNamecardINICreator.CreationClasses
{
    public class NamecardMergedINI
    {
        private readonly string _fileName = "NameCardCollection.ini";
        private readonly NamecardData _namecard;
        private string _parentDirectory = "";
        private readonly NamecardSingleINI singleINI;
        public NamecardMergedINI(NamecardData namecard)
        {
            _namecard = namecard;
            singleINI = new NamecardSingleINI();
        }

        public async Task CreateMod(List<CreateNamecardModPageItemViewModel> directories, ProgressBarWindowViewModel progressBar)
        {
            await Task.Run(async () =>
            {
                _parentDirectory = directories[0].DirectoryItem.Parent.FullName;
                int progressMax = directories.Count + 1;
                string mergedINIpath = Path.Combine(_parentDirectory, _fileName);
                int swapmax = 0;
                List<String> failed = new List<String>();
                foreach (CreateNamecardModPageItemViewModel d in directories)
                {
                    if (Directory.Exists(d.DirectoryItem.FullName))
                    {
                        progressBar.UpdateProgress((double)swapmax / (double)progressMax, d.DirectoryItem.Name);
                        await singleINI.CreateSingleINI(d.DirectoryItem, swapmax, _namecard);
                        swapmax++;
                    }
                    else
                    {
                        failed.Add(d.DirectoryItem.Name);
                    }
                }
                if (swapmax > 0)
                {
                    progressBar.UpdateProgress(((double)progressMax - 1) / (double)progressMax, _fileName);
                    await CreateMergedINI(mergedINIpath, swapmax, _namecard);
                }
                if (failed.Count > 0)
                {
                    string message = "The following folders were not added to the collection since they were renamed or removed between the last refresh and now:";
                    foreach (String name in failed)
                    {
                        message += "\n" + name;
                    }
                    MessageBox.Show(message);
                }
                else
                {
                    MessageBox.Show("Namecard mod created!");
                }
            });
        }

        private async Task CreateMergedINI(string path, int swapmax, NamecardData namecard)
        {
            await Task.Run(() =>
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
                            fw.WriteLine("global persist $swapforward = 0");
                            fw.WriteLine("global persist $swapbackward = 0");
                            fw.WriteLine("global persist $swaprandom = 0");
                            fw.WriteLine("; The total number of namecard folders");
                            fw.WriteLine(String.Format("global persist $swapmax = {0}", swapmax));
                            fw.WriteLine("; Change this to set the startup namecard when random login is off");
                            fw.WriteLine("global persist $swapcard = 0");
                            fw.WriteLine("global persist $toggletogglevar = 1");
                            fw.WriteLine("; Set this to 1 to turn on randomizing on login or 0 for to turn that off.");
                            fw.WriteLine(String.Format("global $random = {0}", Settings.Default.RandomLogin));
                            fw.WriteLine("");
                            fw.WriteLine("");
                            fw.WriteLine("[KeyPause]");
                            fw.WriteLine("condition = $active == 1");
                            fw.WriteLine(String.Format("key = {0}", Settings.Default.KeyPause));
                            fw.WriteLine("type = cycle");
                            fw.WriteLine("$toggletogglevar = 0,1");
                            fw.WriteLine("");
                            fw.WriteLine("[KeyToggleForward]");
                            fw.WriteLine("condition = $active == 1");
                            fw.WriteLine(String.Format("key = {0}", Settings.Default.KeyCycleForward));
                            fw.WriteLine("type = cycle");
                            fw.WriteLine("$swapforward = 0,1");
                            fw.WriteLine("");
                            fw.WriteLine("[KeyToggleBackward]");
                            fw.WriteLine("condition = $active == 1");
                            fw.WriteLine(String.Format("key = {0}", Settings.Default.KeyCycleBackward));
                            fw.WriteLine("type = cycle");
                            fw.WriteLine("$swapbackward = 0,1");
                            fw.WriteLine("");
                            fw.WriteLine("[KeyToggleRandomCycle]");
                            fw.WriteLine("condition = $active == 1");
                            fw.WriteLine(String.Format("key = {0}", Settings.Default.KeyCycleRandom));
                            fw.WriteLine("type = cycle");
                            fw.WriteLine("$random = 0,1");
                            fw.WriteLine("");
                            fw.WriteLine("[Present]");
                            fw.WriteLine("post $active = 0");
                            fw.WriteLine("");
                            fw.WriteLine("; Random selection on login");
                            fw.WriteLine("if $random == 1");
                            fw.WriteLine("\t$random = 0");
                            fw.WriteLine("\trun = CommandListRandomNamecard");
                            fw.WriteLine("endif");
                            fw.WriteLine("");
                            fw.WriteLine("if $swapforward == 1");
                            fw.WriteLine("\t$swapforward = 0");
                            fw.WriteLine("\t$swapcard = $swapcard + 1");
                            fw.WriteLine("endif");
                            fw.WriteLine("");
                            fw.WriteLine("if $swapbackward == 1");
                            fw.WriteLine("\t$swapbackward = 0");
                            fw.WriteLine("\t$swapcard = $swapcard - 1");
                            fw.WriteLine("endif");
                            fw.WriteLine("");
                            fw.WriteLine("if $swapcard >= $swapmax");
                            fw.WriteLine("\t$swapcard = 0");
                            fw.WriteLine("endif");
                            fw.WriteLine("");
                            fw.WriteLine("if $swapcard < 0");
                            fw.WriteLine("\t$swapcard = $swapmax - 1");
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
                            fw.WriteLine("[CommandListRandomNamecard]");
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
                    MessageBox.Show("Failed to create 'NameCardCollection.ini'.\n" + ex.Message);
                }
            });
        }

    }
}
