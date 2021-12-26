using Covid_19_Statistics.Services;
using Lepton_Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.ViewModels
{
    public class HomePageViewModel : PageViewModelBase
    {
        public MapViewModel MapViewModel { get; set; }
        public HomePageCovidDataViewModel CovidDataViewModel { get; set; }
        public HomePageViewModel()
        {
            
        }
    }

    public class HomePageCovidDataViewModel : CovidDataViewModel
    {

    }
}
