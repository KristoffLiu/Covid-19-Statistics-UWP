using Covid_19_Statistics.Models;
using Covid_19_Statistics.Services;
using Covid_19_Statistics.ViewModels;
using Covid_19_Statistics.Views;
using Lepton_Library.Storage;
using Lepton_Library.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Covid_19_Statistics
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NavigationRootPage : Page
    {
        public static NavigationRootPage Current;
        public static Frame RootFrame = null;
        private RootFrameNavigationHelper _navHelper;
        

        public NavigationRootPage()
        {
            this.InitializeComponent();
            Current = this;
            RootFrame = this.rootFrame;
            _navHelper = new RootFrameNavigationHelper(rootFrame, NavigationViewControl);

            AddNavigationMenuItems();

            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            var appTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            appTitleBar.ButtonBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(AppTitleBar);

            CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle(s);
            NavigationView.SelectedItem = NavigationView.MenuItems.First();
            rootFrame.Navigate(typeof(OverViewPage));
        }

        private void AddNavigationMenuItems()
        {
            foreach (var group in UIControlService.Instance.Groups)
            {
                var itemGroup = new muxc.NavigationViewItem() { Content = group.Title, Tag = group.UniqueId, DataContext = group, Icon = GetIcon(group.ImagePath, group.IsUsingSysIcon) };
                if (!group.UniqueId.Contains("Page")) 
                    itemGroup.InfoBadge = new muxc.InfoBadge() { 
                        Value = group.NewConfirmed,
                        
                        Style = Application.Current.Resources["CriticalValueInfoBadgeStyle"] as Style
                    };
                foreach (var item in group.Items.OrderByDescending(x => x.NewConfirmed))
                {
                    var itemInGroup = new muxc.NavigationViewItem() { Content = item.Title, Tag = item.UniqueId, DataContext = item, Icon = GetIcon(item.ImagePath, group.IsUsingSysIcon) };
                    itemInGroup.InfoBadge = new muxc.InfoBadge()
                    {
                        Value = item.NewConfirmed,
                        Style = Application.Current.Resources["CriticalValueInfoBadgeStyle"] as Style
                    };
                    itemGroup.MenuItems.Add(itemInGroup);
                }

                NavigationViewControl.MenuItems.Add(itemGroup);
            }

            NavigationViewControl.MenuItems.Insert(2, new muxc.NavigationViewItemSeparator());
        }


        public muxc.NavigationView NavigationView
        {
            get { return NavigationViewControl; }
        }

        void UpdateAppTitle(CoreApplicationViewTitleBar coreTitleBar)
        {
            //ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
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

                if (String.Equals(invokedItem.Tag, "OverViewPage"))
                {
                    if (rootFrame.CurrentSourcePageType != typeof(OverViewPage))
                    {
                        rootFrame.Navigate(typeof(OverViewPage));
                    }
                }
                else if (String.Equals(invokedItem.Tag, "CountryAndRegionPage"))
                {
                    if (rootFrame.CurrentSourcePageType != typeof(CountryAndRegionListPage))
                    {
                        rootFrame.Navigate(typeof(CountryAndRegionListPage));
                    }
                }
                else
                {
                    if (invokedItem.DataContext is UIControlGroupViewModel)
                    {
                        var itemId = ((UIControlGroupViewModel)invokedItem.DataContext).UniqueId;
                        rootFrame.Navigate(typeof(CountryAndRegionPage), itemId);
                    }
                    else if (invokedItem.DataContext is UIControlViewModel)
                    {
                        var item = (UIControlViewModel)invokedItem.DataContext;
                        rootFrame.Navigate(typeof(AreaPage), new string[] {item.UniqueId, item.ParentId});
                    }
                }
            }

        }

        public void EnsureNavigationSelection(string id)
        {
            foreach (object rawGroup in this.NavigationView.MenuItems)
            {
                if (rawGroup is muxc.NavigationViewItem group)
                {
                    foreach (object rawItem in group.MenuItems)
                    {
                        if (rawItem is muxc.NavigationViewItem item)
                        {
                            if ((string)item.Tag == id)
                            {
                                group.IsExpanded = true;
                                NavigationView.SelectedItem = item;
                                item.IsSelected = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void NavigationViewControl_PaneClosing(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewPaneClosingEventArgs args)
        {
            UpdateAppTitleMargin(sender);
        }

        private void NavigationViewControl_PaneOpened(Microsoft.UI.Xaml.Controls.NavigationView sender, object args)
        {
            //UpdateAppTitleMargin(sender);
        }

        private void NavigationViewControl_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
        {
            Thickness currMargin = AppTitleBar.Margin;
            if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Margin = new Thickness((sender.CompactPaneLength * 2), currMargin.Top, currMargin.Right, currMargin.Bottom);

            }
            else
            {
                AppTitleBar.Margin = new Thickness(sender.CompactPaneLength, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }

            UpdateAppTitleMargin(sender);
            //UpdateHeaderMargin(sender);
        }

        private void UpdateAppTitleMargin(Microsoft.UI.Xaml.Controls.NavigationView sender)
        {
            const int smallLeftIndent = 4, largeLeftIndent = 24;

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                AppTitle.TranslationTransition = new Vector3Transition();

                if ((sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                         sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
                {
                    AppTitle.Translation = new System.Numerics.Vector3(smallLeftIndent, 0, 0);
                }
                else
                {
                    AppTitle.Translation = new System.Numerics.Vector3(largeLeftIndent, 0, 0);
                }
            }
            else
            {
                Thickness currMargin = AppTitle.Margin;

                if ((sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                         sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
                {
                    AppTitle.Margin = new Thickness(smallLeftIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
                }
                else
                {
                    AppTitle.Margin = new Thickness(largeLeftIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
                }
            }
        }

        //private void UpdateHeaderMargin(Microsoft.UI.Xaml.Controls.NavigationView sender)
        //{
        //    if (PageHeader != null)
        //    {
        //        if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
        //        {
        //            Current.PageHeader.HeaderPadding = (Thickness)App.Current.Resources["PageHeaderMinimalPadding"];
        //        }
        //        else
        //        {
        //            Current.PageHeader.HeaderPadding = (Thickness)App.Current.Resources["PageHeaderDefaultPadding"];
        //        }
        //    }
        //}




        private static IconElement GetIcon(string imagePath, bool isUsingSysIcon)
        {
            if (isUsingSysIcon)
                return new FontIcon() { Glyph = imagePath };
            else
                return imagePath.ToLowerInvariant().EndsWith(".png") ?
                        (IconElement)new BitmapIcon() { UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute), ShowAsMonochrome = false } :
                        (IconElement)new FontIcon()
                        {
                            // FontFamily = new FontFamily("Segoe MDL2 Assets"),
                            Glyph = imagePath
                        };
        }

        private void NavigationViewControl_PaneOpening(Microsoft.UI.Xaml.Controls.NavigationView sender, object args)
        {
            UpdateAppTitleMargin(sender);
        }
    }
}
