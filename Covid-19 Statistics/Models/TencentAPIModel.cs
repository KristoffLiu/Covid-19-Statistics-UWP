using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Covid_19_Statistics.Models.CovidDataModel;

namespace Covid_19_Statistics.Models
{
    public class TencentAPIModel
    {
        public string lastUpdateTime { get; set; }
        public ChinaTotal chinaTotal { get; set; }
        public ChinaAdd chinaAdd { get; set; }
        public bool isShowAdd { get; set; }
        public ShowAddSwitch showAddSwitch { get; set; }
        public Area[] areaTree { get; set; }

        public class ChinaTotal
        {
            public int confirm { get; set; }
            public int heal { get; set; }
            public int dead { get; set; }
            public int nowConfirm { get; set; }
            public int suspect { get; set; }
            public int nowSevere { get; set; }
            public int importedCase { get; set; }
            public int noInfect { get; set; }
            public int showLocalConfirm { get; set; }
            public int showlocalinfeciton { get; set; }
            public int localConfirm { get; set; }
            public int noInfectH5 { get; set; }
            public int localConfirmH5 { get; set; }
            public int local_acc_confirm { get; set; }

            //public CovidDataModel.DailyData ToStandardDailyData()
            //{
            //    return new CovidDataModel.TotalData()
            //    {
            //        NowConfirmed = nowConfirm,
            //        Confirmed = confirm,
            //        Suspected = suspect,
            //        Dead = dead,
            //        DeadRate = deadRate,
            //        ShowHeal = showHeal,
            //        Heal = heal,
            //        HealRate = healRate,
            //        ShowRate = showRate,

            //    };
            //}
        }

        public class ChinaAdd
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

        public class ShowAddSwitch
        {
            public bool all { get; set; }
            public bool confirm { get; set; }
            public bool suspect { get; set; }
            public bool dead { get; set; }
            public bool heal { get; set; }
            public bool nowConfirm { get; set; }
            public bool nowSevere { get; set; }
            public bool importedCase { get; set; }
            public bool noInfect { get; set; }
            public bool localConfirm { get; set; }
            public bool localinfeciton { get; set; }
        }

        public class Area
        {
            public string name { get; set; }
            public Today today { get; set; }
            public Total total { get; set; }
            public Area[] children { get; set; }
            public class Today
            {
                public int confirm { get; set; }
                public bool isUpdated { get; set; }
                public CovidDataModel.DailyData ToStandardDailyData()
                {
                    return new CovidDataModel.DailyData()
                    {
                        confirm = confirm
                    };
                }
            }
            public class Total
            {
                public int nowConfirm { get; set; }
                public int confirm { get; set; }
                public int suspect { get; set; }
                public int dead { get; set; }
                public string deadRate { get; set; }
                public string showRate { get; set; }
                public int heal { get; set; }
                public string healRate { get; set; }
                public bool showHeal { get; set; }
                public int wzz { get; set; }

                public CovidDataModel.TotalData ToStandardTotalData()
                {
                    return new CovidDataModel.TotalData()
                    {
                        NowConfirmed = nowConfirm,
                        Confirmed = confirm,
                        Suspected = suspect,
                        Dead = dead,
                        DeadRate = deadRate,
                        ShowHeal = showHeal,
                        Heal = heal,
                        HealRate = healRate,
                        ShowRate = showRate,
                    };
                }
            }

            public CovidDataModel.Area ToStandardArea()
            {
                return new CovidDataModel.Area()
                {
                    Name = name,
                    Daily = today.ToStandardDailyData(),
                    Total = total.ToStandardTotalData(),
                    Children = ToStandardAreaArray(),
                };
            }

            public CovidDataModel.Area[] ToStandardAreaArray()
            {
                if(children == null) return null;
                CovidDataModel.Area[] results = new CovidDataModel.Area[children.Length];
                for(int i = 0; i < children.Length; i++)
                {
                    results[i] = children[i].ToStandardArea();
                }
                return results;
            }
        }

        public CovidDataModel ToStandardCovidDataModel()
        {
            CovidDataModel result = new CovidDataModel()
            {
                Name = "中国",
                Areas = ToStandardAreaArray()
            };
            return result;
        }
        


        public CovidDataModel.Area[] ToStandardAreaArray()
        {
            if (areaTree == null) return null;
            CovidDataModel.Area[] results = new CovidDataModel.Area[areaTree.Length];
            for (int i = 0; i < areaTree.Length; i++)
            {
                results[i] = areaTree[i].ToStandardArea();
            }
            return results;
        }
    }

    public class TencentAPIDataContainer
    {
        public int ret { get; set; }
        public string data { get; set; }
    }
}
