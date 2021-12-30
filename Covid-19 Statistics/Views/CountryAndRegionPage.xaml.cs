using Covid_19_Statistics.Models;
using Covid_19_Statistics.Services;
using Covid_19_Statistics.ViewModels;
using Lepton_Library.Helper;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static Covid_19_Statistics.Models.CovidDataModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Covid_19_Statistics.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CountryAndRegionPage : Page
    {
        CountryOrRegionPageViewModel ViewModel;
        CovidDataModel covidDataModel;
        public CountryAndRegionPage()
        {
            this.InitializeComponent();
            ViewModel = new CountryOrRegionPageViewModel();
            this.DataContext = ViewModel;
            ScaleGrid.Translation = new Vector3(0, 0, 16);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var id = e.Parameter as string;

            covidDataModel = CovidDataService.Countries.Find(x => x.CountryID == id);
            ViewModel.CovidDataViewModel.Update(covidDataModel);

            //var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            //if (anim != null)
            //{
            //    anim.TryStart(CountryOrRegionText);
            //}
        }


        private void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Grid Grid in DataGrid.ItemsPanelRoot.Children)
            {
                Grid.Translation = new Vector3(0, 0, 8);
            }
        }

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            //TimeSpan TotalTime = args.Timing.TotalTime;//经过的时间
            //double TotalSeconds = TotalTime.TotalSeconds;//经过的秒
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                areaView.Act(args.DrawingSession);
            }
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                areaView.Draw(args.DrawingSession);
            }
        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            ViewModel.MapViewModel = new MapViewModel(MapService.MapModels.Find(x => x.Name == "中国"));

            foreach (Area area in covidDataModel.Areas)
            {
                AreaViewModel areaViewModel = ViewModel.MapViewModel.areas.Find(x => x.name.Equals(area.Name));
                if (area.Total.NumOfConfirmedLeft == 0)
                {

                }
                else if (area.Total.NumOfConfirmedLeft < 10)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFE7B2");
                }
                else if (area.Total.NumOfConfirmedLeft < 100)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFCEA0");
                }
                else if (area.Total.NumOfConfirmedLeft < 1000)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFA577");
                }
            }
            sender.PointerEntered += Canvas_PointerEntered;
            sender.PointerMoved += Canvas_PointerMoved;
            sender.PointerPressed += Canvas_PointerPressed;
            sender.PointerReleased += Canvas_PointerReleased;
            sender.PointerWheelChanged += Canvas_PointerWheelChanged;
        }
        private void Canvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine(Canvas.Width);
        }
        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                if (areaView.geometry == null) return;
                if (areaView.geometry.FillContainsPoint(new Vector2((float)pointer.X, (float)pointer.Y)))
                    areaView.isPointerEntered = true;
                else
                    areaView.isPointerEntered = false;
            }
        }

        AreaViewModel SelectedAreaViewModel;
        CovidDataModel.Area SelectedAreaDataModel;
        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                if (areaView.geometry.FillContainsPoint(new Vector2((float)pointer.X, (float)pointer.Y)))
                {
                    areaView.isPointerPressed = true;
                    SelectedAreaViewModel = areaView;
                    SelectedAreaDataModel = covidDataModel.Areas.First(x => x.Name.Contains(areaView.name));
                }
            }
        }
        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                areaView.isPointerPressed = false;
            }
            ShowMenu(false, pointer);
        }

        int change = 0;
        private void Canvas_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            change += e.GetCurrentPoint((CanvasAnimatedControl)sender).Properties.MouseWheelDelta;
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                areaView.Transform = Matrix3x2.CreateScale(0.5f + change * 0.00005f, 0.5f + change * 0.00005f);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (GridViewItem item in DataGrid.ItemsPanelRoot.Children)
            {
                ((Grid)item.Content).Translation = new Vector3(0, 0, 8);
            }
        }

        private void ShowMenu(bool isTransient, Point point)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            point.X += 160;
            myOption.Position = point;
            AreaName.Text = SelectedAreaDataModel.Name;
            AreaImage.Source = new SvgImageSource(new Uri("ms-appx:///Assets/Maps/" + covidDataModel.CountryID + "/" + SelectedAreaViewModel.name +".svg"));
            AreaNumOfCurrentConfirmed.Text = SelectedAreaDataModel.Daily.NumOfConfirmed.ToString();
            AreaNumOfAsymptonmatic.Text = SelectedAreaDataModel.Total.NumOfAsymptonmatic.ToString();
            SubAreaDetailFlyout.ShowAt(Canvas, myOption);
        }

        private void GoToAreaButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("AreaNameForwardConnectedAnimation", AreaName);
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("MapPlotImageForwardConnectedAnimation", AreaImage);
            
            Frame.Navigate(typeof(AreaPage), new string[] { SelectedAreaDataModel.Name, covidDataModel.CountryID }, new SuppressNavigationTransitionInfo());

        }
    }
}
