using Covid_19_Statistics.Services;
using Covid_19_Statistics.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Covid_19_Statistics.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CountryAndRegionListPage : Page
    {
        CountryAndRegionListPageViewModel ViewModel;

        public CountryAndRegionListPage()
        {
            this.InitializeComponent();
            ViewModel = new CountryAndRegionListPageViewModel();
            this.DataContext = ViewModel;
            InitAllCountryAndRegion();
        }

        private void InitAllCountryAndRegion()
        {
            foreach(var country in CovidDataService.Countries)
            {
                var countryViewModel = new CountryOrRegionViewModel(country.CountryID, country.Name, "1202", "");
                ViewModel.CountryAndRegionList.Add(countryViewModel);
            }
        }

        private void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as CountryOrRegionViewModel;
            //ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", (UIElement)ChinaText);
            Frame.Navigate(typeof(CountryAndRegionPage), item.Id, new SuppressNavigationTransitionInfo());
        }
    }
}
