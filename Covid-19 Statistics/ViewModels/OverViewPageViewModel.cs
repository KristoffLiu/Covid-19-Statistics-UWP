using Covid_19_Statistics.Services;
using Lepton_Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.ViewModels
{
    public class OverViewPageViewModel : PageViewModelBase
    {
        public MapViewModel MapViewModel { get; set; }
        public OverViewPageCovidDataViewModel CovidDataViewModel { get; set; }

        public bool IsLoading { get; set; }

        public OverViewPageViewModel()
        {
            
        }
    }

    public class OverViewPageCovidDataViewModel : CovidDataViewModel
    {

    }
}
