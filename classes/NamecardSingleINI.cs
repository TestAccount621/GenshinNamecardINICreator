using System.IO;
using System.Windows;

namespace GenshinNamecardAutomater.classes
{
    public class NamecardSingleINI
    {
        private string iniName;
        public List<string> sortedFiles = [];
        public List<string> extraFiles = [];

        public NamecardSingleINI(DirectoryInfo name, int swapcard, NamecardData namecard)
        {
            iniName = name.FullName + @"\namecard.ini";
            sortedFiles.Clear();
            extraFiles.Clear();
            var files = name.GetFiles().Where(x => x.Extension.Equals(".png")).ToList();
            List<string> fileNames = [];
            foreach (var file in files)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(file.Name));
            }
            fileNames.Sort(new NumericStringComparer());
            int test;
            foreach (var fi in fileNames)
            {
                if (Int32.TryParse(fi.Last().ToString(), out test))
                {
                    sortedFiles.Add(fi);
                }
                else
                {
                    extraFiles.Add(fi);
                }
            }
            if (sortedFiles.Count > 0)
            {
                if (sortedFiles.Count == 1)
                {
                    CreateNameCardSingleImage_INI(swapcard, namecard);
                }
                else if (sortedFiles.Count > 1)
                {
                    CreateNameCardGIF_INI(swapcard, namecard);
                }
            }
        }
        /// <summary>
        /// Creates the INI file for a singular namecard folder. It's place within the folder of the images.
        /// </summary>
        /// <param name="swapcard">The currently position of the namespace toggle that the mod will be in.</param>
        /// <param name="namecard">The namecard data, of which only the hashes matter.</param>
        public void CreateNameCardGIF_INI(int swapcard, NamecardData namecard)
        {
            try
            {
                using (var fs = new FileStream(iniName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var fw = new StreamWriter(fs))
                    {
                        fw.WriteLine("; Constants");
                        fw.WriteLine("");
                        fw.WriteLine("[Constants]");
                        fw.WriteLine("global persist $swapvar = 0");
                        fw.WriteLine("; The line below here MUST be line #6 for the program that generated this. Please don't move it!");
                        fw.WriteLine(String.Format("global persist $swapcard = {0}", swapcard.ToString()));
                        fw.WriteLine(String.Format("global persist $swapMax = {0}", sortedFiles.Count));
                        fw.WriteLine("global persist $halfspeed = 0");
                        fw.WriteLine("global persist $speedtoggle = 0");
                        fw.WriteLine("global persist $gamefps = 60");
                        fw.WriteLine("global persist $ActiveNameCard = 0");
                        fw.WriteLine("");
                        fw.WriteLine("[Present]");
                        fw.WriteLine("post $ActiveNameCard = 0");
                        fw.WriteLine("");
                        fw.WriteLine("if $ActiveNameCard == 1 && $halfspeed < $gamefps");
                        if (sortedFiles.Count > 900) { fw.WriteLine("\t$halfspeed = $halfspeed + 59"); }
                        else { fw.WriteLine("\t$halfspeed = $halfspeed + 30"); }
                        fw.WriteLine("\t$speedtoggle = 0");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("if $halfspeed >= $gamefps");
                        fw.WriteLine("\t$halfspeed = $halfspeed - $gamefps");
                        fw.WriteLine("\t$speedtoggle = 1");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("if $\\NameCardCollection\\toggletogglevar == 1 && $ActiveNameCard == 1 && $speedtoggle == 1 && $\\NameCardCollection\\swapcard == $swapcard");
                        fw.WriteLine("\t$swapvar = $swapvar + 1");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("if $swapvar >= $swapMax || $swapvar < 0");
                        fw.WriteLine("\t$swapvar = 0");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("; TextureOverrides");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardA]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.MainHash));
                        fw.WriteLine("$ActiveNameCard = 1");
                        fw.WriteLine("run = CommandListNameCardA");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardD]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.PreviewHash));
                        fw.WriteLine("$ActiveNameCard = 1");
                        fw.WriteLine("if $ActiveNameCard == 1 && $\\NameCardCollection\\swapcard == $swapcard");
                        fw.WriteLine("\tthis = ResourceNameCardD");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardE]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.BannerHash));
                        fw.WriteLine("$ActiveNameCard = 1");
                        fw.WriteLine("if $ActiveNameCard == 1 && $\\NameCardCollection\\swapcard == $swapcard");
                        fw.WriteLine("\tthis = ResourceNameCardE");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("; CommandList");
                        fw.WriteLine("");
                        fw.WriteLine("[CommandListNameCardA]");
                        fw.WriteLine("if $ActiveNameCard == 1 && $\\NameCardCollection\\swapcard == $swapcard");
                        fw.WriteLine("\tif $swapvar >= 0 && $swapvar <= $swapMax");
                        for (int i = 0; i < sortedFiles.Count; i++)
                        {
                            if (i == 0)
                            {
                                fw.WriteLine(String.Format("\t\tif $swapvar == {0}", i));
                                fw.WriteLine(String.Format("\t\t\tps-t0 = ResourceNameCardA{0}", i + 1));
                            }
                            else
                            {
                                fw.WriteLine(String.Format("\t\telse if $swapvar == {0}", i));
                                fw.WriteLine(String.Format("\t\t\tps-t0 = ResourceNameCardA{0}", i + 1));
                            }
                        }
                        fw.WriteLine("\t\tendif");
                        fw.WriteLine("\tendif");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("; Resources");
                        fw.WriteLine("");
                        for (int i = 0; i < sortedFiles.Count; i++)
                        {
                            fw.WriteLine(String.Format("[ResourceNameCardA{0}]", i + 1));
                            fw.WriteLine(String.Format("filename = {0}", sortedFiles[i] + ".png"));
                        }
                        fw.WriteLine("");
                        fw.WriteLine("[ResourceNameCardD]");
                        fw.WriteLine(String.Format("filename = {0}", extraFiles[0] + ".png"));
                        fw.WriteLine("");
                        fw.WriteLine("[ResourceNameCardE]");
                        fw.WriteLine(String.Format("filename = {0}", extraFiles[1] + ".png"));
                        fw.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        public void CreateNameCardSingleImage_INI(int swapcard, NamecardData namecard)
        {
            try
            {
                using (var fs = new FileStream(iniName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var fw = new StreamWriter(fs))
                    {
                        fw.WriteLine("; Constants");
                        fw.WriteLine("");
                        fw.WriteLine("[Constants]");
                        fw.WriteLine("global persist $swapvar = 0");
                        fw.WriteLine("; The line below here MUST be line #6 for the program that generated this. Please don't move it!");
                        fw.WriteLine(String.Format("global persist $swapcard = {0}", swapcard.ToString()));
                        fw.WriteLine("global persist $ActiveNameCard = 0");
                        fw.WriteLine("");
                        fw.WriteLine("[Present]");
                        fw.WriteLine("post $ActiveNameCard = 0");
                        fw.WriteLine("");
                        fw.WriteLine("; TextureOverrides");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardA]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.MainHash));
                        fw.WriteLine("$ActiveNameCard = 1");
                        fw.WriteLine("if $ActiveNameCard == 1 && $\\NameCardCollection\\swapcard == $swapcard");
                        fw.WriteLine("\tthis = ResourceNameCardA");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardD]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.PreviewHash));
                        fw.WriteLine("$ActiveNameCard = 1");
                        fw.WriteLine("if $ActiveNameCard == 1 && $\\NameCardCollection\\swapcard == $swapcard");
                        fw.WriteLine("\tthis = ResourceNameCardD");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("[TextureOverrideNameCardE]");
                        fw.WriteLine(String.Format("hash = {0}", namecard.BannerHash));
                        fw.WriteLine("$ActiveNameCard = 1");
                        fw.WriteLine("if $ActiveNameCard == 1 && $\\NameCardCollection\\swapcard == $swapcard");
                        fw.WriteLine("\tthis = ResourceNameCardE");
                        fw.WriteLine("endif");
                        fw.WriteLine("");
                        fw.WriteLine("; Resources");
                        fw.WriteLine("");
                        fw.WriteLine("[ResourceNameCardA]");
                        fw.WriteLine(String.Format("filename = {0}", sortedFiles[0] + ".png"));
                        fw.WriteLine("");
                        fw.WriteLine("[ResourceNameCardD]");
                        fw.WriteLine(String.Format("filename = {0}", extraFiles[0] + ".png"));
                        fw.WriteLine("");
                        fw.WriteLine("[ResourceNameCardE]");
                        fw.WriteLine(String.Format("filename = {0}", extraFiles[1] + ".png"));
                        fw.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
