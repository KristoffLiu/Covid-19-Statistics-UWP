using Covid_19_Statistics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.Services
{
    public class CovidDataService
    {
        public static List<CovidDataModel> Countries { get; set; }

        public static void Init()
        {
            Countries = new List<CovidDataModel>();
        }

        public static async void Sync()
        {
            if(await TencentAPIService.Init())
            {
                Countries.Add(TencentAPIService.GetChinaData());
            }            
        }
    }
}
