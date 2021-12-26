using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace Covid_19_Statistics.Models
{
    public class MapModel
    {
        public MapModel()
        {
            Id = 0;
            Name = "";
            areas = new List<AreaModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AreaModel> areas { get; set; }

    }

    public class AreaModel
    {
        public string name { get; set; }
        public int value { get; set; }
    }

    public class AreaPathModel : AreaModel
    {
        public string pathsText { get; set; }
        public AreaPathModel(XmlNamedNodeMap attributes)
        {
            foreach (var attribute in attributes)
            {
                switch (attribute.NodeName)
                {
                    case "name":
                        name = attribute.InnerText;
                        break;
                    case "d":
                        pathsText = attribute.InnerText;
                        break;
                }
            }
        }
    }

    public class AreaCircleModel : AreaModel
    {
        public float r { get; set; }
        public float cx { get; set; }
        public float cy { get; set; }
        public AreaCircleModel(XmlNamedNodeMap attributes)
        {
            foreach (var attribute in attributes)
            {
                switch (attribute.NodeName)
                {
                    case "name":
                        name = attribute.InnerText;
                        break;
                    case "r":
                        r = float.Parse(attribute.InnerText);
                        break;
                    case "cx":
                        cx = float.Parse(attribute.InnerText);
                        break;
                    case "cy":
                        cx = float.Parse(attribute.InnerText);
                        break;
                }
            }
        }
    }
}
