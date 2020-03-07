using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class NavigationItemViewModel: ViewModelBase
    {
        private string _displayMember;

        public NavigationItemViewModel(long id, string displayMember)
        {
            _displayMember = displayMember;
            Id = id;
        }

        public long Id { get; set; }

        public string DisplayMember
        {
            get { return _displayMember; }
            set { 
                _displayMember = value;
                OnPropertyChanged();
            }
        }

    }
}
