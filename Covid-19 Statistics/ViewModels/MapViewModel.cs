using Covid_19_Statistics.Models;
using Lepton_Library.Common;
using Lepton_Library.Helper;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Covid_19_Statistics.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<AreaViewModel> areas { get; set; }
        public MapViewModel(MapModel mapModel)
        {
            name = mapModel.Name;
            areas = new List<AreaViewModel>();
            foreach (var area in mapModel.areas)
            {
                if (area.GetType() == typeof(AreaPathModel))
                {
                    areas.Add(new AreaPathViewModel((AreaPathModel)area));
                }
                else if (area.GetType() == typeof(AreaCircleModel))
                {
                    areas.Add(new AreaCircleViewModel((AreaCircleModel)area));
                }
            }
        }
    }

    public class CanvasElementViewModel
    {
        public bool isPointerEntered { get; set; } = false;
        public bool isPointerPressed { get; set; } = false;
    }



    public class AreaViewModel : CanvasElementViewModel
    {
        public CanvasGeometry geometry { get; set; }
        public CanvasDrawingSession session { get; set; }
        public AreaViewModel(AreaModel model)
        {
            this.session = session;
            name = model.name;
            value = 0;
            FillColor = ColourHelper.GetColor("FFE2EBF4");
            PointerEnteredColor = ColourHelper.GetColor("FFCCCCCC");
            PointerPressedColor = ColourHelper.GetColor("FFAAAAAA");
            StrokeColor = Color.FromArgb(255, 255, 255, 255);
            StrokeThickness = 2;
            Transform = Matrix3x2.CreateScale(0.5f, 0.5f);
        }
        public string name { get; set; }
        public int value { get; set; }

        public Color _fillColor { get; set; }
        public Color FillColor
        {
            get
            {
                if (isPointerPressed) return PointerPressedColor;
                else if (isPointerEntered) return PointerEnteredColor;
                else return _fillColor;
            }
            set { _fillColor = value; }
        }
        public Color PointerEnteredColor { get; set; }
        public Color PointerPressedColor { get; set; }
        public Color StrokeColor { get; set; }
        public float StrokeThickness { get; set; }
        public Matrix3x2 Transform { get; set; }
        public virtual void Draw(CanvasDrawingSession session)
        {
         
        }
    }

    public class AreaPathViewModel : AreaViewModel
    {
        public string pathsText { get; set; }
        public AreaPathViewModel(AreaPathModel model) : base(model)
        {
            pathsText = model.pathsText;

        }
        public override void Draw(CanvasDrawingSession session)
        {
            geometry = CanvasPathGeometry.CreateGeometry(session, pathsText);
            geometry = geometry.Transform(Transform);
            session.DrawGeometry(geometry, StrokeColor, StrokeThickness);
            session.FillGeometry(geometry, FillColor);
        }
    }

    public class AreaCircleViewModel : AreaViewModel
    {
        public float r { get; set; }
        public float cx { get; set; }
        public float cy { get; set; }
        public AreaCircleViewModel(AreaCircleModel model) : base(model)
        {
            r = model.r;
            cx = model.cx;
            cy = model.cy;

        }
        public override void Draw(CanvasDrawingSession session)
        {
            geometry = CanvasGeometry.CreateCircle(session, cx, cy, r);
            geometry = geometry.Transform(Transform);
            session.DrawGeometry(geometry, StrokeColor, StrokeThickness);
            session.FillGeometry(geometry, FillColor);
            //session.DrawGeometry(cx, cy, r, StrokeColor, StrokeThickness);
            //session.FillGeometry(cx, cy, r, FillColor);
        }
    }
}
