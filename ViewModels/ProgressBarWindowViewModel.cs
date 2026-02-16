using GenshinNamecardINICreator.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.ViewModels
{
    public class ProgressBarWindowViewModel : BaseViewModel
    {
        private readonly ModalNavigationStore modalNavigationStore;

        private string titleText = "";
        public string TitleText 
        { 
            get => titleText;
            set 
            {
                titleText = value;
                OnPropertyChanged(nameof(TitleText));
            }
        }

        private string _currentItem = "";
        public string CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                _currentItem = value;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        private double _curentProgress;
        public double CurrentProgress
        {
            get
            {
                return _curentProgress;
            }
            private set
            {
                _curentProgress = value;
                OnPropertyChanged(nameof(CurrentProgress));
            }
        }

        public ProgressBarWindowViewModel(ModalNavigationStore modalNavigationStore)
        {
            this.modalNavigationStore = modalNavigationStore;
        }

        public void SetTitle(string title)
        {
            titleText = title;
        }

        public void UpdateProgress(double progress, string currentItem)
        {
            CurrentItem = currentItem;
            CurrentProgress = progress;
        }
    }
}
