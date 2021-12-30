using Lepton_Library.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.ViewModels
{
    public class CountryAndRegionListPageViewModel : PageViewModelBase
    {
        public ObservableCollection<CountryOrRegionViewModel> CountryAndRegionList;
        public CountryAndRegionListPageViewModel()
        {
            CountryAndRegionList = new ObservableCollection<CountryOrRegionViewModel>();
        }
    }

    public class CountryOrRegionViewModel : Observable
    {
        private string _name;
        private string _nowInfectedNum;
        private string _mapImageSource;

        public string Id { get; set; }
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
        public string NowInfectedNum
        {
            get { return _nowInfectedNum; }
            set { Set(ref _nowInfectedNum, value); }
        }
        public string MapImageSource
        {
            get { return _mapImageSource; }
            set { Set(ref _mapImageSource, value); }
        }

        public CountryOrRegionViewModel(string id, string name, string nowinfectednum, string mapImageSource)
        {
            Id = id;
            Name = name;
            NowInfectedNum = nowinfectednum;
            MapImageSource = mapImageSource;
        }
    }
}
