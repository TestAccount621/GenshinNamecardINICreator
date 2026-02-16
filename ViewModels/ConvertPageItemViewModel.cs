using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.ViewModels
{
    public class ConvertPageItemViewModel : BaseViewModel
    {
        public readonly FileInfo FileInfo;
        public string DisplayName => FileInfo.Name;

        public ConvertPageItemViewModel(FileInfo file)
        {
            this.FileInfo = file;
        }
    }
}
