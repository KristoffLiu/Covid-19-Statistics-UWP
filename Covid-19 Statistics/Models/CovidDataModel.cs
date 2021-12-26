using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.Models
{
    public class CovidDataModel
    {
        public string Name { get; set; }

        DailyData dailyData { get; set; }
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
            public int confirm { get; set; }
            public int heal { get; set; }
            public int dead { get; set; }
            public int nowConfirm { get; set; }
            public int suspect { get; set; }
            public int nowSevere { get; set; }
            public int importedCase { get; set; }
            public int noInfect { get; set; }
            public int localConfirm { get; set; }
            public int noInfectH5 { get; set; }
            public int localConfirmH5 { get; set; }
        }

        public class TotalData
        {
            public int NowConfirmed { get; set; }
            public int Confirmed { get; set; }
            public int Suspected { get; set; }
            public int Dead { get; set; }
            public string DeadRate { get; set; }
            public string ShowRate { get; set; }
            public int Heal { get; set; }
            public string HealRate { get; set; }
            public bool ShowHeal { get; set; }


            public int NowSevere { get; set; }
            public int ImportedCase { get; set; }
            public int NoInfected { get; set; }
            public int ShowLocalConfirm { get; set; }
            public int ShowLocalinfeciton { get; set; }
            public int LocalConfirm { get; set; }
            public int LocalAccumulativeConfirm { get; set; }
        }
    }
}
