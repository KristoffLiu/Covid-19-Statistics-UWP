using Covid_19_Statistics.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.Services
{
    public class UIControlService
    {
        private static readonly object _lock = new object();

        private static UIControlService _instance;

        public static UIControlService Instance
        {
            get
            {
                return _instance;
            }
        }

        static UIControlService()
        {
            _instance = new UIControlService();
        }

        private UIControlService() { }

        private IList<UIControlGroupViewModel> _groups = new List<UIControlGroupViewModel>();
        public IList<UIControlGroupViewModel> Groups
        {
            get { return this._groups; }
        }

        public async Task<IEnumerable<UIControlGroupViewModel>> GetGroupsAsync()
        {
            await _instance.GetControlInfoDataAsync();
            return _instance.Groups;
        }


        public async Task GetControlInfoDataAsync() {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    if (this.Groups.Count() != 0)
                    {
                        return;
                    }
                }

                lock (_lock)
                {
                    foreach (var control in GetMainUIControls())
                    {
                        Groups.Add(control);
                    }
                    foreach (var country in CovidDataService.Countries)
                    {
                        var countryItem = new UIControlGroupViewModel(country.CountryID, country.Name, " ", "\uE707", true, " ", country.Today.NumOfConfirmed);
                        foreach (var province in country.Areas)
                        {
                            countryItem.Items.Add(new UIControlViewModel(province.Name, country.CountryID, province.Name, " ", "\uE7B7", true, " ", province.Daily.NumOfConfirmed));
                        }
                        Groups.Add(countryItem);
                    }
                }
            });
        }


        public ObservableCollection<UIControlGroupViewModel> GetMainUIControls()
        {
            return new ObservableCollection<UIControlGroupViewModel>
            {
                new UIControlGroupViewModel("OverViewPage", "概览", " ", "\uE909", true, " "),
                new UIControlGroupViewModel("CountryAndRegionPage", "国家与地区", " ", "\uEA37", true, " "),
            };
        }
    }
}
