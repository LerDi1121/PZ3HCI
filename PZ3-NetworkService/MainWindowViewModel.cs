using PZ3_NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        private VodomerViewModel vodomerModelView = new VodomerViewModel();
        private ReportViewModel reportViewModel = new ReportViewModel();
        private DDViewModel networkViewModel = new DDViewModel();
        private DataChartViewModel DataChart = new DataChartViewModel();
        private BindableBase currentViewModel;
        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<String>(OnNav);
            CurrentViewModel = vodomerModelView;
        }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "Network Data":
                    CurrentViewModel = vodomerModelView;
                    break;
                case "Report View":
                    CurrentViewModel = reportViewModel;
                    break;
                case "Network View":
                    CurrentViewModel = networkViewModel;
                    break;
                case "Data Chart":
                    CurrentViewModel = DataChart;
                    break;

            }
        }
    }
}
