using GenshinNamecardINICreator.Commands;
using GenshinNamecardINICreator.Commands.CreatePageCommands;
using GenshinNamecardINICreator.CreationClasses;
using GenshinNamecardINICreator.Properties;
using GenshinNamecardINICreator.Stores;
using MintPlayer.ObservableCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GenshinNamecardINICreator.ViewModels
{
    public class CreateNamecardModPageViewModel : BaseViewModel
    {
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly NamecardHashesStore namecardHashesStore;
        private readonly ListViewsRefreshStore listViewsRefreshStore;
        private readonly MintPlayer.ObservableCollection.ObservableCollection<CreateNamecardModPageItemViewModel> _outOfCollection;
        public IEnumerable<CreateNamecardModPageItemViewModel> OutOfCollection => _outOfCollection;

        private readonly MintPlayer.ObservableCollection.ObservableCollection<CreateNamecardModPageItemViewModel> _insideCollection;

        public IEnumerable<CreateNamecardModPageItemViewModel> InsideCollection => _insideCollection;

        public bool InsideCollectionHasItems => _insideCollection.Any();

        private bool _isMoving;
        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
            set
            {
                _isMoving = value;
                OnPropertyChanged(nameof(IsMoving));
            }
        }

        public ICommand OpenConfirmationWindowCommand { get; }
        public ICommand SwapArrowCommand { get; }
        public ICommand UpDownArrowCommand { get; }

        public CreateNamecardModPageViewModel(ModalNavigationStore modalNavigationStore,
                                              NamecardHashesStore namecardHashesStore,
                                              ListViewsRefreshStore listViewsRefreshStore,
                                              ProgressBarWindowViewModel progressBarWindowViewModel)
        {
            this.modalNavigationStore = modalNavigationStore;
            this.namecardHashesStore = namecardHashesStore;
            this.listViewsRefreshStore = listViewsRefreshStore;

            _outOfCollection = [];
            _insideCollection = [];

            LoadAndSortFolders();

            OpenConfirmationWindowCommand = new OpenConfirmationWindowCommand(modalNavigationStore, namecardHashesStore, progressBarWindowViewModel, this);
            UpDownArrowCommand = new UpDownArrowCommand(this);
            SwapArrowCommand = new SwapArrowCommand(this);

            this.listViewsRefreshStore.RefreshNeeded += ListViewsRefreshStore_RefreshNeeded;
        }

        private void ListViewsRefreshStore_RefreshNeeded()
        {
            LoadAndSortFolders();
        }

        private void LoadAndSortFolders()
        {
            IsMoving = true;
            if (String.IsNullOrEmpty(Settings.Default.OutputFolder))
            {
                Settings.Default.OutputFolder = AppDomain.CurrentDomain.BaseDirectory;
                Settings.Default.Save();
            }
            var directory = new DirectoryInfo(Settings.Default.OutputFolder);
            if (directory.Exists)
            {
                _outOfCollection.Clear();
                _insideCollection.Clear();
                List<CreateNamecardModPageItemViewModel> outside = [];
                List<CreateNamecardModPageItemViewModel> inside = [];
                var folders = directory.GetDirectories();
                if (folders.Count() > 0)
                {
                    List<Tuple<DirectoryInfo, int>> list = [];
                    foreach (var folder in folders)
                    {
                        // Exclude all folders that do not even have a .png in it.
                        var hasImages = folder.GetFiles().Where(x => x.Extension.Equals(".png"));
                        if (hasImages.Any())
                        {
                            // Check if the folder is already part of the merge essentially.
                            var files = folder.GetFiles().Where(x => x.Name.Equals("namecard.ini"));
                            bool found = false;
                            if (files.Any())
                            {
                                foreach (var file in files)
                                {
                                    // Checks the 6th line of the ini since that SHOULD be where the $swapcard variable is placed.
                                    var line = File.ReadLines(file.FullName).Skip(5).FirstOrDefault();
                                    if (line != null)
                                    {
                                        try
                                        {
                                            string numberString = new string(line.Where(Char.IsDigit).ToArray());
                                            int number = int.Parse(numberString);
                                            if (number >= 0)
                                            {
                                                list.Add(new Tuple<DirectoryInfo, int>(folder, number));
                                            }
                                            else
                                            {
                                                list.Add(new Tuple<DirectoryInfo, int>(folder, 9999));
                                            }
                                            inside.Add(new CreateNamecardModPageItemViewModel(folder));
                                            found = true;
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                    }
                                }
                            }
                            if (found)
                            {
                                found = false;
                                continue;
                            }
                            outside.Add(new CreateNamecardModPageItemViewModel(folder));
                        }
                    }
                    var outsideSorted = outside.OrderBy(x => x.DisplayName);
                    _outOfCollection.AddRange(outsideSorted);
                    if (list.Count > 0)
                    {
                        var insideSortedTuple = list.OrderBy(x => x.Item2);
                        List<CreateNamecardModPageItemViewModel> insideSorted = [];
                        foreach (var item in insideSortedTuple)
                        {
                            insideSorted.Add(new CreateNamecardModPageItemViewModel(item.Item1));
                        }
                        _insideCollection.AddRange(insideSorted);
                    }
                    else
                    {
                        if (inside.Count > 0) { _insideCollection.AddRange(inside.OrderBy(x => x.DisplayName)); }
                    }
                    OnPropertyChanged(nameof(InsideCollectionHasItems));
                }
            }
            else
            {
                Settings.Default.OutputFolder = AppDomain.CurrentDomain.BaseDirectory;
                Settings.Default.Save();
            }
            IsMoving = false;
        }

        public void MoveVertically(string tag, List<CreateNamecardModPageItemViewModel> selected)
        {
            // TODO Find a better method. This one can break when clicking a direction then the opposite without clearing selection. No idea why.

            List<Tuple<CreateNamecardModPageItemViewModel, int>> indexList = [];
            // Create expected indexes.
            for (int i = 0; i < selected.Count; i++)
            {
                int oldIndex = _insideCollection.IndexOf(selected[i]);
                indexList.Add(new Tuple<CreateNamecardModPageItemViewModel, int>(selected[i], oldIndex));
            }
            if (tag.Equals("Up"))
            {
                for (int i = 0; i < selected.Count; i++)
                {
                    int oldIndex = _insideCollection.IndexOf(selected[i]);
                    int moveTo = oldIndex - 1 >= 0 ? oldIndex - 1 : 0;
                    Tuple<CreateNamecardModPageItemViewModel, int> current = indexList.Where(x => x.Item1 == selected[i]).FirstOrDefault();
                    if (current.Item2 != oldIndex)
                    {
                        moveTo -= Math.Abs(current.Item2 - oldIndex);
                        if (moveTo < 0) { moveTo = 0; }
                    }
                    _insideCollection.Move(oldIndex, moveTo);
                    if (selected.Count > 1 && i == selected.Count - 1)
                    {
                        oldIndex = _insideCollection.IndexOf(selected[0]);
                        moveTo = oldIndex - 1 >= 0 ? oldIndex - 1 : 0;
                        _insideCollection.Move(oldIndex, moveTo);
                    }
                }
            }
            else if (tag.Equals("Down"))
            {
                for (int i = selected.Count - 1; i >= 0; i--)
                {
                    int oldIndex = _insideCollection.IndexOf(selected[i]);
                    int moveTo = oldIndex + 1 < _insideCollection.Count - 1 ? oldIndex + 1 : _insideCollection.Count - 1;
                    Tuple<CreateNamecardModPageItemViewModel, int> current = indexList.Where(x => x.Item1 == selected[i]).FirstOrDefault();
                    if (current.Item2 != oldIndex)
                    {
                        moveTo += Math.Abs(current.Item2 - oldIndex);
                    }
                    _insideCollection.Move(oldIndex, moveTo);
                    if (selected.Count > 1 && i == 0)
                    {
                        // Is this needed for down? Seems needed for Up when index is max.
                        //oldIndex = _insideCollection.IndexOf(selected[selected.Count - 1]);
                        //moveTo = oldIndex + 1 >= 0 ? oldIndex + 1 : selected.Count - 1;
                        //_insideCollection.Move(oldIndex, moveTo);
                    }

                }
            }
        }

        public void MoveHorizontally(List<CreateNamecardModPageItemViewModel> fromList, string toName)
        {
            if (toName.Equals("lv_Left"))
            {
                foreach (var item in fromList)
                {
                    if (!_outOfCollection.Contains(item)) 
                    {
                        _outOfCollection.Add(item);
                        _insideCollection.Remove(item);
                    }
                }
            }
            else if (toName.Equals("lv_Right"))
            {
                foreach (var item in fromList)
                {
                    if (!_insideCollection.Contains(item))
                    {
                        _insideCollection.Add(item);
                        _outOfCollection.Remove(item);
                    }
                }
            }
            OnPropertyChanged(nameof(InsideCollectionHasItems));
        }

        protected override void Dispose()
        {
            this.listViewsRefreshStore.RefreshNeeded -= ListViewsRefreshStore_RefreshNeeded;

            base.Dispose();
        }
    }
}
