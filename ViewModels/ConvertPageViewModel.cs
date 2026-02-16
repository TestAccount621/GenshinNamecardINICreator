using GenshinNamecardINICreator.Commands.ConvertPageCommands;
using GenshinNamecardINICreator.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GenshinNamecardINICreator.ViewModels
{
    public class ConvertPageViewModel : BaseViewModel
    {
        private readonly ObservableCollection<ConvertPageItemViewModel> _files;
        public IEnumerable<ConvertPageItemViewModel> Files => _files;

        public ICommand ConvertCommand { get; }
        public ICommand RemoveSelectionCommand { get; }
        public ICommand SelectImagesCommand { get; }


        public ConvertPageViewModel(ListViewsRefreshStore listViewsRefreshStore,
                                    ProgressBarWindowViewModel progressBarWindowViewModel,
                                    ModalNavigationStore modalNavigationStore)
        {
            _files = [];

            ConvertCommand = new ConvertCommand(this, listViewsRefreshStore, progressBarWindowViewModel, modalNavigationStore);
            RemoveSelectionCommand = new ConvertPageRemoveCommand(this);
            SelectImagesCommand = new ConvertPageSelectImagesCommand(this);
        }

        public void ClearListView()
        {
            _files.Clear();
        }

        public void RemoveFileInfo(ConvertPageItemViewModel fileInfo)
        {
            _files.Remove(fileInfo);
        }

        public void AddFileInfo(ConvertPageItemViewModel fileInfo)
        {
            if (!_files.Contains(fileInfo))
            {
                _files.Add(fileInfo);
            }
        }
    }
}
