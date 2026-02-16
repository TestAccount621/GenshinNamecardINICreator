using GenshinNamecardINICreator.Commands.HashesPageCommands;
using GenshinNamecardINICreator.Models;
using GenshinNamecardINICreator.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GenshinNamecardINICreator.ViewModels
{
    public class NamecardHashesPageViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly NamecardHashesStore namecardHashesStore;
        private readonly ObservableCollection<NamecardHashesPageComboBoxItemViewModel> _namecardHashesPageComboBoxItemViewModels;
        private readonly ErrorsViewModel _errorsViewModel;
        public IEnumerable<NamecardHashesPageComboBoxItemViewModel> NamecardHashesPageComboBoxItemViewModels => _namecardHashesPageComboBoxItemViewModels;

        private NamecardHashesPageComboBoxItemViewModel _selectedNamecardHashesPageComboBoxItemViewModel;
        public NamecardHashesPageComboBoxItemViewModel SelectedNamecardHashesPageComboBoxItemViewModel
        {
            get => _selectedNamecardHashesPageComboBoxItemViewModel;
            set
            {
                _selectedNamecardHashesPageComboBoxItemViewModel = value;
                OnPropertyChanged(nameof(SelectedNamecardHashesPageComboBoxItemViewModel));
                if (value is null)
                {
                    Name = "";
                    MainHash = "";
                    PreviewHash = "";
                    BannerHash = "";
                }
                else
                {
                    Name = value.Namecard.Name;
                    MainHash = value.Namecard.MainHash;
                    PreviewHash = value.Namecard.PreviewHash;
                    BannerHash = value.Namecard.BannerHash;
                }
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }
        public NamecardHashesPageComboBoxItemViewModel PreviousSelectedNamecard;
        
        private string _name = "";
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                _errorsViewModel.ClearErrors(nameof(Name));
                if (!ValidateProperty(nameof(Name), value))
                {
                    _errorsViewModel.AddError(nameof(Name), "Cannot be blank.");
                }
                OnPropertyChanged(nameof(Name)); 
            }
        }
        private string _mainHash = "";
        public string MainHash
        {
            get { return _mainHash; }
            set
            {
                _mainHash = value;
                _errorsViewModel.ClearErrors(nameof(MainHash));
                if (!ValidateProperty(nameof(MainHash), value))
                {
                    _errorsViewModel.AddError(nameof(MainHash), "Invalid namecard format.");
                }
                OnPropertyChanged(nameof(MainHash));
            }
        }
        private string _previewHash = "";
        public string PreviewHash
        {
            get { return _previewHash; }
            set
            {
                _previewHash = value;

                _errorsViewModel.ClearErrors(nameof(PreviewHash));
                if (!ValidateProperty(nameof(PreviewHash), value))
                {
                    _errorsViewModel.AddError(nameof(PreviewHash), "Invalid namecard format.");
                }
                OnPropertyChanged(nameof(PreviewHash));
            }
        }
        private string _bannerHash = "";
        public string BannerHash
        {
            get { return _bannerHash; }
            set
            {
                _bannerHash = value;
                _errorsViewModel.ClearErrors(nameof(BannerHash));
                if (!ValidateProperty(nameof(BannerHash), value))
                {
                    _errorsViewModel.AddError(nameof(BannerHash), "Invalid namecard format.");
                }
                OnPropertyChanged(nameof(BannerHash));
            }
        }

        public bool IsReadOnly { get { return CurrentState == NamecardHashesPageStateEnum.View; } }
        public bool CanSubmit => !HasErrors;

        private NamecardHashesPageStateEnum _currentState = NamecardHashesPageStateEnum.View;

        public NamecardHashesPageStateEnum CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                OnPropertyChanged(nameof(CurrentState));
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public ICommand NewNamecardCommand { get; }
        public ICommand DeleteNamecardCommand { get; }
        public ICommand UpdateNamecardCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }


        public NamecardHashesPageViewModel(NamecardHashesStore namecardHashesStore)
        {
            this.namecardHashesStore = namecardHashesStore;
            _errorsViewModel = new ErrorsViewModel();

            NewNamecardCommand = new NewHashCommand(this);
            DeleteNamecardCommand = new DeleteHashCommand(this);
            UpdateNamecardCommand = new EditHashCommand(this);
            SubmitCommand = new SubmitCommand(this);
            CancelCommand = new CancelCommand(this);

            this.namecardHashesStore.NamecardAdded += NamecardHashesStore_NamecardAdded;
            this.namecardHashesStore.NamecardDeleted += NamecardHashesStore_NamecardDeleted;
            this.namecardHashesStore.NamecardUpdated += NamecardHashesStore_NamecardUpdated;
            this.namecardHashesStore.SelecetedNamecardChanged += NamecardHashesStore_SelecetedNamecardChanged;
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

            _namecardHashesPageComboBoxItemViewModels = [];
            foreach (NamecardData namecard in namecardHashesStore.Namecards)
            {
                _namecardHashesPageComboBoxItemViewModels.Add(new NamecardHashesPageComboBoxItemViewModel(namecard));
            }
        }


        protected override void Dispose()
        {
            namecardHashesStore.NamecardDeleted -= NamecardHashesStore_NamecardDeleted;
            namecardHashesStore.SelecetedNamecardChanged -= NamecardHashesStore_SelecetedNamecardChanged;
            namecardHashesStore.NamecardAdded -= NamecardHashesStore_NamecardAdded;
            namecardHashesStore.NamecardUpdated -= NamecardHashesStore_NamecardUpdated;
            _errorsViewModel.ErrorsChanged -= ErrorsViewModel_ErrorsChanged;

            base.Dispose();
        }

        private void NamecardHashesStore_SelecetedNamecardChanged()
        {
            RefreshNamecardBindings();
        }

        private void NamecardHashesStore_NamecardUpdated(NamecardData obj)
        {
            RefreshNamecardBindings();
        }

        private void NamecardHashesStore_NamecardDeleted(NamecardData obj)
        {
            RefreshNamecardBindings();
        }

        private void NamecardHashesStore_NamecardAdded(NamecardData obj)
        {
            RefreshNamecardBindings();
        }

        private void RefreshNamecardBindings()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(MainHash));
            OnPropertyChanged(nameof(PreviewHash));
            OnPropertyChanged(nameof(BannerHash));
            OnPropertyChanged(nameof(IsReadOnly));
        }

        public void DeleteNamecard(NamecardHashesPageComboBoxItemViewModel namecard)
        {
            _namecardHashesPageComboBoxItemViewModels.Remove(namecard);
            CleanupNamecardChanges(null);
        }

        public void AddNamecard(string name, string main, string preview, string banner)
        {
            // The validation should be in the xaml with a ValidationRule.
            NamecardData namecard = new(name, main, preview, banner);
            NamecardHashesPageComboBoxItemViewModel item = new(namecard);
            _namecardHashesPageComboBoxItemViewModels.Add(item);
            CleanupNamecardChanges(item);
        }

        internal void UpdateNamecard(string name, string main, string preview, string banner)
        {
            // The validation should be in the xaml with a ValidationRule.
            NamecardData namecard = new(name, main, preview, banner);
            NamecardHashesPageComboBoxItemViewModel item = new(namecard);
            int index = _namecardHashesPageComboBoxItemViewModels.IndexOf(PreviousSelectedNamecard);
            if (index != -1)
            {
                _namecardHashesPageComboBoxItemViewModels[index] = item;
                CleanupNamecardChanges(item);
            }
        }

        private void CleanupNamecardChanges(NamecardHashesPageComboBoxItemViewModel item)
        {
            PreviousSelectedNamecard = null;
            CurrentState = NamecardHashesPageStateEnum.View;
            SelectedNamecardHashesPageComboBoxItemViewModel = item;
            OnPropertyChanged(nameof(NamecardHashesPageComboBoxItemViewModels));
        }

        #region ErrorNotification
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanSubmit));
        }
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        private bool ValidateProperty(string propertyName, string value)
        {
            if (!IsReadOnly)
            {
                if (propertyName.Equals(nameof(Name)))
                {
                    return !string.IsNullOrWhiteSpace(value);
                }
                else
                {
                    return Regex.IsMatch(value, @"^[a-zA-Z0-9]{8}$");
                }
            }
            else { return true; }
        }
        #endregion
    }
}
