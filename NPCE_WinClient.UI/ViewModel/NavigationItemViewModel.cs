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
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _detailViewModelName;
        private string _displayMember;
        private IEventAggregator _eventAggregator;

        public NavigationItemViewModel(int id, string displayMember,
            IEventAggregator eventAggregator,
            string detailViewModelName)
        {
            _detailViewModelName = detailViewModelName;
            _displayMember = displayMember;
            Id = id;
            _eventAggregator = eventAggregator;
            OpenAnagraficaDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
            OpenDocumentoDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
            OpenAmbienteDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Publish(
                new OpenDetailViewEventargs
                {
                    Id = Id,
                    ViewModelName = _detailViewModelName
                });

        }

        public int Id { get; set; }


        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenAnagraficaDetailViewCommand { get; }
        public ICommand OpenDocumentoDetailViewCommand { get; }
        public ICommand OpenAmbienteDetailViewCommand { get; }

    }
}
