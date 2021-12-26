using Covid_19_Statistics.Models;
using Covid_19_Statistics.Services;
using Covid_19_Statistics.Views;
using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Covid_19_Statistics
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NavigationRootPage : Page
    {
        public static Frame RootFrame = null;

        public NavigationRootPage()
        {
            this.InitializeComponent();
            RootFrame = this.rootFrame;
            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            var appTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
        }

        private void OnNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.IsSelected)
            {
                // Clicked on an item that is already selected,
                // Avoid navigating to the same page again causing movement.
                return;
            }

            if (args.IsSettingsInvoked)
            {
                if (rootFrame.CurrentSourcePageType != typeof(SettingsPage))
                {
                    rootFrame.Navigate(typeof(SettingsPage));
                }
            }
            else
            {
                var invokedItem = args.InvokedItemContainer;

                if (String.Equals(invokedItem.Tag,"home"))
                {
                    if (rootFrame.CurrentSourcePageType != typeof(HomePage))
                    {
                        rootFrame.Navigate(typeof(HomePage));
                    }
                }
                else if (String.Equals(invokedItem.Tag, "area"))
                {
                    if (rootFrame.CurrentSourcePageType != typeof(AreaPage))
                    {
                        rootFrame.Navigate(typeof(AreaPage));
                    }
                }
            }

        }
    }
}
