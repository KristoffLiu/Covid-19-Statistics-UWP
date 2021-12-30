using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Lepton_Plotlib.Controls
{
    public sealed partial class MapPlotControl : UserControl
    {
        public MapPlotControl()
        {
            this.InitializeComponent();
        }

        //public static readonly DependencyProperty DateTimeProperty =
        //DependencyProperty.Register("DateTime", typeof(DateTime?), typeof(DateTimeSelector3), new PropertyMetadata(null, OnDateTimeChanged));

        ///// <summary>
        ///// 获取或设置DateTime的值
        ///// </summary>  
        //public DateTime? DateTime
        //{
        //    get { return (DateTime?)GetValue(DateTimeProperty); }
        //    set { SetValue(DateTimeProperty, value); }
        //}
    }
}
