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
        public DailyData Daily { get; set; }
        public TotalData Total { get; set; }
        public CovidDataViewModel()
        {

        }


        public class DailyData : ViewModelBase
        {
            public int _confirm;
            public int Confirm
            {
                get { return _confirm; }
                set
                {
                    Set(ref _confirm, value);
                }
            }
        }

        public class TotalData : ViewModelBase
        {
            private int _nowConfirmed;
            private int _confirmed;
            private int _suspected;
            private int _dead;
            private string _deadRate;
            private string _showRate;
            private int _heal;
            private string _healRate;
            private bool _showHeal;

            public int NowConfirmed
            {
                get { return _nowConfirmed; }
                set
                {
                    Set(ref _nowConfirmed, value);
                }
            }
            public int Confirmed
            {
                get { return _confirmed; }
                set
                {
                    Set(ref _confirmed, value);
                }
            }
            public int Suspected
            {
                get { return _suspected; }
                set
                {
                    Set(ref _suspected, value);
                }
            }
            public int Dead
            {
                get { return _dead; }
                set
                {
                    Set(ref _dead, value);
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
            public string ShowRate
            {
                get { return _showRate; }
                set
                {
                    Set(ref _showRate, value);
                }
            }
            public int Heal
            {
                get { return _heal; }
                set
                {
                    Set(ref _heal, value);
                }
            }
            public string HealRate
            {
                get { return _healRate; }
                set
                {
                    Set(ref _healRate, value);
                }
            }
            public bool ShowHeal
            {
                get { return _showHeal; }
                set
                {
                    Set(ref _showHeal, value);
                }
            }
        }
    }
}
