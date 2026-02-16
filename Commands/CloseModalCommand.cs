using GenshinNamecardINICreator.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenshinNamecardINICreator.Commands
{
    public class CloseModalCommand : CommandBase
    {
        private readonly ModalNavigationStore modalNavigationStore;

        public CloseModalCommand(ModalNavigationStore modalNavigationStore)
        {
            this.modalNavigationStore = modalNavigationStore;
        }

        public override void Execute(object? parameter)
        {
            this.modalNavigationStore.Close();
        }
    }
}
