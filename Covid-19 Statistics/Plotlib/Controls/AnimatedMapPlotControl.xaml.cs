using Covid_19_Statistics.Plotlib.Helper;
using Covid_19_Statistics.Plotlib.Models;
using Covid_19_Statistics.Plotlib.ViewModels;
using Lepton_Library.Helper;
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
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Covid_19_Statistics.Plotlib.Controls
{
    public sealed partial class AnimatedMapPlotControl : UserControl
    {
        MapViewModel mapViewModel;
        public MapModel MapModel { get; set; }

        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("MapSource", typeof(Object), typeof(AnimatedMapPlotControl), new PropertyMetadata(null, OnMapSourceChanged));

        private static async void OnMapSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            AnimatedMapPlotControl target = obj as AnimatedMapPlotControl;
            if(args.NewValue == null)
                return;
            object oldValue = args.OldValue;
            object newValue = args.NewValue;
            if (oldValue != newValue)
                await target.OnMapSourceChanged(oldValue, newValue);
        }
        public AnimatedMapPlotControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 获取或设置DateTime的值
        /// </summary>  
        public object MapSource
        {
            get { return (object)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        bool isUpdated;

        bool _isUpdatingMapSource;
        private async Task OnMapSourceChanged(object oldValue, object newValue)
        {
            _isUpdatingMapSource = true;
            if (newValue is MapModel)
            {
                MapModel = newValue as MapModel;
            }
            else if(newValue is string)
            {
                MapModel = await MapHelper.GetMapBySVGAsync(newValue as string);
            }
            else
            {
                throw new ArgumentException();
            }
            CreateMapResourcesAsync();
            _isUpdatingMapSource = false;
            isUpdated = true;
        }

        private async void MapCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            if (mapViewModel == null) return;
            

            var session = args.DrawingSession;
            mapViewModel.areas.ForEach(area => area.Act(session));
            mapViewModel.areas.ForEach(area => area.Draw(session));
            mapViewModel.areas.ForEach(area => area.DrawText(session));
            if (isUpdated)
            {
                await AdjustLayout();
                isUpdated = false;
            }
        }

        private void MapCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            CreateMapResourcesAsync();
        }

        bool _isUpdatingMapResources;
        public void CreateMapResourcesAsync()
        {
            if (MapModel == null) return;
            _isUpdatingMapResources = true;
            mapViewModel = new MapViewModel(MapModel);
            //AdjustLayout();
            //Rect rect = new Rect();
            //mapViewModel.areas.ForEach(area => rect.Union(area.geometry.ComputeBounds()));

            //foreach (var item in ViewModel.MapViewModel.areas)
            //{
            //    item.Transform = Matrix3x2.CreateScale(0.6f, 0.6f);
            //}

            //foreach (Area area in Area.Children)
            //{
            //    AreaViewModel areaViewModel = ViewModel.MapViewModel.areas.Find(x => x.name.Contains(area.Name));
            //    if (areaViewModel == null) continue;
            //    if (area.Total.NumOfConfirmedLeft == 0)
            //    {

            //    }
            //    else if (area.Total.NumOfConfirmedLeft < 10)
            //    {
            //        areaViewModel.FillColor = ColourHelper.GetColor("#FFFFE7B8");
            //    }
            //    else if (area.Total.NumOfConfirmedLeft < 50)
            //    {
            //        areaViewModel.FillColor = ColourHelper.GetColor("#FFFFDAB3");
            //    }
            //    else if (area.Total.NumOfConfirmedLeft < 100)
            //    {
            //        areaViewModel.FillColor = ColourHelper.GetColor("#FFFFC89E");
            //    }
            //    else if (area.Total.NumOfConfirmedLeft < 500)
            //    {
            //        areaViewModel.FillColor = ColourHelper.GetColor("#FFFFAA80");
            //    }
            //}
            _isUpdatingMapResources = false;
        }

        private async void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (mapViewModel == null) return;
            //var width = e.NewSize.Width;
            //var height = e.NewSize.Height;
            //Rect rect = mapViewModel.areas[0].geometry.ComputeBounds();
            //mapViewModel.areas.ForEach(area => rect.Union(area.geometry.ComputeBounds()));
            //float xOffset = (float)(width / 2 - rect.X - rect.Width / 2);
            //float yOffset = (float)(height / 2 - rect.Y - rect.Height / 2);
            //mapViewModel.areas.ForEach(area =>
            //{
            //    area.Transform = area.Transform * Matrix3x2.CreateTranslation(xOffset, yOffset);
            //});
            this.CurrentWidth = (float)this.ActualWidth;
            this.CurrentHeight = (float)this.ActualHeight;
            await AdjustLayout();
        }

        double CurrentWidth { get; set; }
        double CurrentHeight { get; set; }

        public async Task AdjustLayout()
        {
            await Task.Run(() =>
            {
                if (mapViewModel == null) return;
                var width = CurrentWidth;
                var height = CurrentHeight;
                Rect rect = mapViewModel.areas[0].geometry.ComputeBounds();
                mapViewModel.areas.ForEach(area => rect.Union(area.geometry.ComputeBounds()));
                float xOffset = (float)(width / 2 - rect.X - rect.Width / 2);
                float yOffset = (float)(height / 2 - rect.Y - rect.Height / 2);
                mapViewModel.areas.ForEach(area =>
                {
                    area.Transform = area.Transform * Matrix3x2.CreateTranslation(xOffset, yOffset);
                });
            });
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.CurrentWidth = (float)this.ActualWidth;
            this.CurrentHeight = (float)this.ActualHeight;
            await AdjustLayout();
        }
    }
}
