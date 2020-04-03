using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NPCE_WinClient.UI.View
{
    /// <summary>
    /// Interaction logic for PagineBollettinoDetailView.xaml
    /// </summary>
    public partial class PagineBollettinoDetailView : UserControl
    {
        public PagineBollettinoDetailView()
        {
            InitializeComponent();
        }

        private void listView_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
