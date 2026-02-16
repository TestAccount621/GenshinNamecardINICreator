using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.ViewModels
{
    public class CreateNamecardModPageItemViewModel : BaseViewModel
    {
        public string DisplayName => DirectoryItem.Name;

        public DirectoryInfo DirectoryItem { get; }

        public CreateNamecardModPageItemViewModel(DirectoryInfo directory)
        {
            DirectoryItem = directory;
        }
    }
}
