using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.ViewModels
{
    public class AreaPageViewModel : PageViewModelBase
    {
        public string CountryID { get; set; }
        public string AreaId { get; set; }
        public CovidDataViewModel CovidDataViewModel { get; set; } = new CovidDataViewModel();
        public MapViewModel MapViewModel { get; set; }

        public ObservableCollection<CovidDataViewModel> SubAreaList { get; set; } = new ObservableCollection<CovidDataViewModel>();

    }
}
