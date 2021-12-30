using Covid_19_Statistics.Plotlib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace Covid_19_Statistics.Plotlib.Helper
{
    public class MapHelper
    {
        public static async Task<MapModel> GetMapBySVGAsync(Uri uri)
        {
            MapModel mapModel = new MapModel();
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var doc = await XmlDocument.LoadFromFileAsync(file);

            foreach (var element in doc.ChildNodes[2].ChildNodes)
            {
                if (element.Attributes == null) continue;
                try
                {
                    var _name = element.Attributes.First(x => x.NodeName.Equals("name"));
                }
                catch (InvalidOperationException ex)
                {
                    continue;
                }
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

        public static async Task<MapModel> GetMapBySVGAsync(string uriStr)
        {
            return await GetMapBySVGAsync(new Uri(uriStr));
        }

        public static async Task<MapModel> GetMapBySVGAsync(string uriStr, string name)
        {
            var map = await GetMapBySVGAsync(new Uri(uriStr));
            map.Name = name;
            return map;
        }

    }
}
