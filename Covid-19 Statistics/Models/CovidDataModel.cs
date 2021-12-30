using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.Models
{
    public class CovidDataModel
    {
        public string CountryID { get; set; }
        public string Name { get; set; }
        public DailyData Today { get; set; }
        public TotalData All { get; set; }
        public int Count { get; set; }
        public Area[] Areas { get; set; }
        public class Area
        {
            public string Name { get; set; }
            public DailyData Daily { get; set; }
            public TotalData Total { get; set; }
            public Area[] Children { get; set; }
        }           
        public class DailyData
        {
            public int NumOfConfirmed { get; set; }
            public int NumOfCured { get; set; }
            public int NumOfDead { get; set; }
            public int NumOfComfirmedIncreased { get; set; }
            public int NumOfSuspected { get; set; }
            public int NumOfSeverePatients { get; set; }
            public int NumOfImportedCases { get; set; }
            public int NumOfAsymptomatic { get; set; }
            public int NumOfLocalCases { get; set; }
        }

        public class TotalData
        {
            public int NumOfConfirmedLeft { get; set; }
            public int NumOfConfirmedTotal { get; set; }
            public int NumOfSuspected { get; set; }
            public int NumOfDead { get; set; }
            public string DeadRate { get; set; }
            public int NumOfCured { get; set; }
            public string CuredRate { get; set; }
            public int NowSevere { get; set; }
            public int NumOfImportedCase { get; set; }
            public int NumOfAsymptonmatic { get; set; }
            public int NumOfLocalConfirmed { get; set; }
            public int NumOfLocalAccConfirmed { get; set; }
        }
    }
}
