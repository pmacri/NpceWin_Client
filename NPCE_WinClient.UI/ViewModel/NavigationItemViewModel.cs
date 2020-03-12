using NPCE_WinClient.UI.Event;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NPCE_WinClient.UI.ViewModel
{
    public class NavigationItemViewModel: ViewModelBase
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;
        
        public NavigationItemViewModel(int id, string displayMember,
            IEventAggregator eventAggregator)
        {
            _displayMember = displayMember;
            Id = id;
            _eventAggregator = eventAggregator;
            OpenAnagraficaDetailViewCommand = new DelegateCommand(OnOpenAnagraficaDetailView);
        }

        private void OnOpenAnagraficaDetailView()
        {
            _eventAggregator.GetEvent<OpenDetailAnagraficaViewEvent>()
                .Publish(Id);

        }

        public int Id { get; set; }


        public string DisplayMember
        {
            get { return _displayMember; }
            set { 
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenAnagraficaDetailViewCommand { get; }

    }
}
