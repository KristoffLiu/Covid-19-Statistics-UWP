using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.ViewModels
{
    public class CountryOrRegionPageViewModel : PageViewModelBase
    {
        public CovidDataViewModel CovidDataViewModel { get; set; }
        public MapViewModel MapViewModel { get; set; }
        public bool IsLoading { get; set; }
        public CountryOrRegionPageViewModel()
        {
            CovidDataViewModel = new CovidDataViewModel();
        }
    }
}
