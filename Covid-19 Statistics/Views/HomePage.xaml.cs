using Covid_19_Statistics.Models;
using Covid_19_Statistics.Services;
using Covid_19_Statistics.ViewModels;
using Lepton_Library.Helper;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static Covid_19_Statistics.Models.CovidDataModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Covid_19_Statistics.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        HomePageViewModel ViewModel;
        CovidDataModel covidDataModel;

        public HomePage()
        {
            this.InitializeComponent();
            ViewModel = new HomePageViewModel();
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            TimeSpan TotalTime = args.Timing.TotalTime;//经过的时间
            double TotalSeconds = TotalTime.TotalSeconds;//经过的秒
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                areaView.Draw(args.DrawingSession);
            }
        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            covidDataModel = CovidDataService.Countries.Find(x => x.Name == "中国");
            ViewModel.MapViewModel = new MapViewModel(VisualMapService.MapModels.Find(x => x.Name == "中国"));

            foreach (Area area in covidDataModel.Areas[0].Children)
            {
                AreaViewModel areaViewModel = ViewModel.MapViewModel.areas.Find(x => x.name.Equals(area.Name));
                if (area.Total.NowConfirmed == 0)
                {

                }
                else if (area.Total.NowConfirmed < 10)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFE7B2");
                }
                else if (area.Total.NowConfirmed < 100)
                {
                    areaViewModel.FillColor = ColourHelper.GetColor("#FFFFCEA0");
                }
                else if (area.Total.NowConfirmed < 1000)
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

        }

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                if (areaView.geometry.FillContainsPoint(new Vector2((float)pointer.X, (float)pointer.Y)))
                    areaView.isPointerEntered = true;
                else
                    areaView.isPointerEntered = false;
            }
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            foreach (var areaView in ViewModel.MapViewModel.areas)
            {
                if (areaView.geometry.FillContainsPoint(new Vector2((float)pointer.X, (float)pointer.Y)))
                    areaView.isPointerPressed = true;
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

        private void Canvas_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {

        }

        private void ShowMenu(bool isTransient, Point point)
        {
            FlyoutShowOptions myOption = new FlyoutShowOptions();
            myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;
            point.X += 160;
            myOption.Position = point;
            CommandBarFlyout1.ShowAt(Canvas, myOption);
        }





        //Uri localUri = new Uri("ms-appx:///Assets/Maps/China/中国.svg");
        //CanvasSvgDocument svgDocument = null;
        //visualMapService = new VisualMapService(Canvas);

        //GetImage().Wait();
        //async Task GetImage()
        //{
        //    await Task.Run(async () =>
        //    {
        //        var file = await StorageFile.GetFileFromApplicationUriAsync(localUri);
        //        var doc = await XmlDocument.LoadFromFileAsync(file);
        //        IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);

        //        svgDocument = new CanvasSvgDocument(sender);
        //        CanvasSvgNamedElement element = await svgDocument.LoadElementAsync(stream);

        //        svgDocument.Root.AppendChild(element);
        //        CanvasSvgNamedElement pathElement = svgDocument.FindElementById("path0");
        //        pathElement.SetStringAttribute("fill", "#DC143C");

        //    }).ConfigureAwait(false);
        //}

        //args.DrawingSession.DrawSvg(svgDocument, new Size(500, 500));
    }
}
