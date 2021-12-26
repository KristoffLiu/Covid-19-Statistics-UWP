using Covid_19_Statistics.Models;
using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Covid_19_Statistics.Services
{
    public class VisualMapService
    {
        public static Uri localUri = new Uri("ms-appx:///Assets/Maps/China/中国.svg");
        public static List<MapModel> MapModels { get; set; } = new List<MapModel>();
        public static async void Init()
        {
            MapModels.Add(await GetMapBySVGAsync(localUri, "中国"));
        }
        public static async Task<MapModel> GetChinaMap()
        {
            return MapModels.Find(x => x.Name == "中国");
        }

        public static async Task<MapModel> GetMapBySVGAsync(Uri uri, String name)
        {
            MapModel mapModel = new MapModel() { Name = name };
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var doc = await XmlDocument.LoadFromFileAsync(file);

            foreach (var element in doc.ChildNodes[2].ChildNodes)
            {
                if (element.NodeName.Equals("path"))
                {
                    AreaPathModel areaModel = new AreaPathModel(element.Attributes);
                    mapModel.areas.Add(areaModel);
                }
                else if (element.NodeName.Equals("circle"))
                {
                    AreaCircleModel areaModel = new AreaCircleModel(element.Attributes);
                    mapModel.areas.Add(areaModel);
                }
            }
            return mapModel;
        }



        //public async Task GetChinaMap()
        //{
        //    await Task.Run(async () =>
        //    {
        //        var file = await StorageFile.GetFileFromApplicationUriAsync(localUri);
        //        var doc = await XmlDocument.LoadFromFileAsync(file);
        //        IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);

        //        ChinaMapDocument = new CanvasSvgDocument(CanvasControl);
        //        CanvasSvgNamedElement element = await ChinaMapDocument.LoadElementAsync(stream);

        //        ChinaMapDocument.Root.AppendChild(element);
        //        //CanvasSvgNamedElement pathElement = ChinaMapDocument.FindElementById("path0");
        //        //pathElement.SetStringAttribute("fill", "#DC143C");

        //    }).ConfigureAwait(false);
        //}

        //public async Task SaveChinaMap()
        //{
        //    ChinaMapDocument
        //}




    }
}
