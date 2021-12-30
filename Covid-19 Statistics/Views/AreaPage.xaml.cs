using Covid_19_Statistics.Models;
using Covid_19_Statistics.Services;
using Covid_19_Statistics.ViewModels;
using Lepton_Library.Helper;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static Covid_19_Statistics.Models.CovidDataModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Covid_19_Statistics.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AreaPage : Page
    {
        AreaPageViewModel ViewModel;
        CovidDataModel.Area Area { get; set; }
        public AreaPage()
        {
            this.InitializeComponent();
            ViewModel = new AreaPageViewModel();
            DataGrid.Translation += new Vector3(0, 0, 8);
            VisualizationGrid.Translation += new Vector3(0, 0, 8);
            AreaDetailsGrid.Translation += new Vector3(0, 0, 8);
            ScaleGrid.Translation += new Vector3(0, 0, 16);
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var idPair = e.Parameter as string[];
            ViewModel.AreaId    = idPair[0];
            ViewModel.CountryID = idPair[1];

            MapPlotImage.Source = new SvgImageSource(new Uri("ms-appx:///Assets/Maps/" + ViewModel.CountryID + "/" + ViewModel.AreaId + ".svg"));
            var animAreaName = ConnectedAnimationService.GetForCurrentView().GetAnimation("AreaNameForwardConnectedAnimation");
            var animMapPlotImageName = ConnectedAnimationService.GetForCurrentView().GetAnimation("MapPlotImageForwardConnectedAnimation");
            if (animAreaName != null && animMapPlotImageName != null)
            {
                animAreaName.TryStart(AreaNameTextBlock);
                animMapPlotImageName.TryStart(MapPlotImage);
            }

            CovidDataModel country = CovidDataService.Countries.Find(x => x.CountryID == ViewModel.CountryID);
            Area = country.Areas.First(x => x.Name == ViewModel.AreaId);
            Area.Children.ToList().ForEach(x =>
            {
                CovidDataViewModel areaViewModel = new CovidDataViewModel();
                areaViewModel.Update(x);
                ViewModel.SubAreaList.Add(areaViewModel);
            });

            ViewModel.CovidDataViewModel.Update(Area);

            AnimatedMapPlotControl.MapSource = await MapService.GetMapBySVGAsync(ViewModel.CountryID, ViewModel.AreaId);
        }

        bool resourcedLoaded = false;
        public async Task createResource()
        {
            ViewModel.MapViewModel = new MapViewModel(await MapService.GetMapBySVGAsync(ViewModel.CountryID, ViewModel.AreaId));
            foreach(var item in ViewModel.MapViewModel.areas)
            {
                item.Transform = Matrix3x2.CreateScale(0.6f, 0.6f);
            }

            foreach (Area area in Area.Children)
            {
                AreaViewModel areaViewModel = ViewModel.MapViewModel.areas.Find(x => x.name.Contains(area.Name));
                if (areaViewModel == null) continue;
                if (area.Total.NumOfConfirmedLeft == 0)
                {

                }
                else if (area.Total.NumOfConfirmedLeft < 10)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFE7B8");
                }
                else if (area.Total.NumOfConfirmedLeft < 50)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFDAB3");
                }
                else if (area.Total.NumOfConfirmedLeft < 100)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFC89E");
                }
                else// if (area.Total.NumOfConfirmedLeft < 500)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFAA80");
                }
            }
            resourcedLoaded = true;
        }

        private void CanvasAnimatedControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            if (resourcedLoaded)
            {
                foreach (var areaView in ViewModel.MapViewModel.areas)
                {
                    areaView.Act(args.DrawingSession);
                }
                foreach (var areaView in ViewModel.MapViewModel.areas)
                {
                    areaView.Draw(args.DrawingSession);
                }
                foreach (var areaView in ViewModel.MapViewModel.areas)
                {
                    areaView.DrawText(args.DrawingSession);
                }
            }            
        }

        private async void CanvasAnimatedControl_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            await createResource();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
