using Covid_19_Statistics.Models;
using Lepton_Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Covid_19_Statistics.ViewModels
{
    public class CovidDataViewModel : ViewModelBase
    {
        private string _name = "";
        public string Name
        {
            get { return _name; }
            set
            {
                Set(ref _name, value);
            }
        }

        public DailyData Today { get; set; } = new DailyData();
        public TotalData Total { get; set; } = new TotalData();
        public CovidDataViewModel()
        {

        }

        public void Update(CovidDataModel covidDataModel)
        {
            Name = covidDataModel.Name;
            Today.Update(covidDataModel.Today);
            Total.Update(covidDataModel.All);
        }

        public void Update(CovidDataModel.Area areaDataModel)
        {
            Name = areaDataModel.Name;
            Today.Update(areaDataModel.Daily);
            Total.Update(areaDataModel.Total);
        }


        public class DailyData : ViewModelBase
        {
            public int _confirm = 0;
            public int NumOfConfirmed
            {
                get { return _confirm; }
                set
                {
                    Set(ref _confirm, value);
                }
            }

            public void Update(CovidDataModel.DailyData dailyData)
            {
                NumOfConfirmed = dailyData.NumOfConfirmed;
            }
        }

        public class TotalData : ViewModelBase
        {
            private int _numOfConfirmedLeft = 0;
            private int _numOfconfirmedTotal = 0;
            private int _numOfSuspected = 0;
            private int _numOfDead = 0;
            private string _deadRate = "";
            private int _cured = 0;
            private string _curedRate = "";

            public int NumOfConfirmedLeft
            {
                get { return _numOfConfirmedLeft; }
                set
                {
                    Set(ref _numOfConfirmedLeft, value);
                }
            }
            public int NumOfConfirmedTotal
            {
                get { return _numOfconfirmedTotal; }
                set
                {
                    Set(ref _numOfconfirmedTotal, value);
                }
            }
            public int NumOfSuspected
            {
                get { return _numOfSuspected; }
                set
                {
                    Set(ref _numOfSuspected, value);
                }
            }
            public int NumOfDead
            {
                get { return _numOfDead; }
                set
                {
                    Set(ref _numOfDead, value);
                }
            }
            public string DeadRate
            {
                get { return _deadRate; }
                set
                {
                    Set(ref _deadRate, value);
                }
            }
            public int NumOfCured
            {
                get { return _cured; }
                set
                {
                    Set(ref _cured, value);
                }
            }
            public string CuredRate
            {
                get { return _curedRate; }
                set
                {
                    Set(ref _curedRate, value);
                }
            }


            private int _numOfImportedCases = 0;
            public int NumOfImportedCases
            {
                get { return _numOfImportedCases; }
                set
                {
                    Set(ref _numOfImportedCases, value);
                }
            }

            private int _numOfAsymptonmatic = 0;
            public int NumOfAsymptonmatic
            {
                get { return _numOfAsymptonmatic; }
                set
                {
                    Set(ref _numOfAsymptonmatic, value);
                }
            }

            private int _numOfLocalAccConfirmed = 0;
            public int NumOfLocalAccConfirmed
            {
                get { return _numOfLocalAccConfirmed; }
                set
                {
                    Set(ref _numOfLocalAccConfirmed, value);
                }
            }

            private int _numOfLocalConfirmed = 0;
            public int NumOfLocalConfirmed
            {
                get { return _numOfLocalConfirmed; }
                set
                {
                    Set(ref _numOfLocalConfirmed, value);
                }
            }

            public void Update(CovidDataModel.TotalData totalData)
            {
                NumOfConfirmedLeft = totalData.NumOfConfirmedLeft;
                NumOfConfirmedTotal = totalData.NumOfConfirmedTotal;
                NumOfSuspected = totalData.NumOfSuspected;
                NumOfDead = totalData.NumOfDead;
                DeadRate = totalData.DeadRate;
                NumOfCured = totalData.NumOfCured;
                CuredRate = totalData.CuredRate;
                NumOfImportedCases = totalData.NumOfImportedCase;
                NumOfAsymptonmatic = totalData.NumOfAsymptonmatic;
                NumOfLocalAccConfirmed = totalData.NumOfLocalAccConfirmed;
                NumOfLocalConfirmed = totalData.NumOfLocalConfirmed;
            }
        }
    }
}
