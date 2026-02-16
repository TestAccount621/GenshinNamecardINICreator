using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenshinNamecardINICreator.Models;

namespace GenshinNamecardINICreator.ViewModels
{
    public class NamecardHashesPageComboBoxItemViewModel : BaseViewModel
    {
        public NamecardData Namecard { get; private set; }
        public string Name => Namecard.Name;

        public NamecardHashesPageComboBoxItemViewModel(NamecardData namecard)
        {
            Namecard = namecard;
        }


        public void Update(NamecardData namecard)
        {
            Namecard = namecard;
            OnPropertyChanged(nameof(Namecard));
        }
    }
}
