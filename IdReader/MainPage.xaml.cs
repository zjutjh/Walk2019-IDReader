using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;
namespace IdReader
{   /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += CheckServer;
            timer.Start();
            ContentFrame.Navigate(typeof(StartModePage), "start");
        }

        private async void CheckServer(object sender, object e)
        {
            ValueSet vs = new ValueSet();
            vs.Add("check", "IDCard");
            AppServiceConnection connection = App.Connection;
        

            if (connection == null)
            {
                StColor.Background = new SolidColorBrush(Colors.Red);
                StText.Text = "未连接到服务";
                return;
            }

            try
            {
                var vss = await connection.SendMessageAsync(vs);
                if (vss.Message.Keys.Count < 1)
                {
                    StColor.Background = new SolidColorBrush(Colors.Red);
                    StText.Text = "未连接到服务";
                    return;
                }
                else if (vss.Message.Keys.First() == "noReader")
                {
                    StColor.Background = new SolidColorBrush(Colors.Red);
                    StText.Text = "未连接到刷卡机";
                    return;
                }
            }
            catch (Exception)
            {
                StColor.Background = new SolidColorBrush(Colors.Red);
                StText.Text = "未连接到服务";
                return;
            }


            StColor.Background = new SolidColorBrush(Colors.Green);
            StText.Text = "已连接到服务";
            return;

        }

        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            StColor.Background = new SolidColorBrush(Colors.Red);
            StText.Text = "未连接到服务";
            App.Connection = null;
        }

        private void NavView_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

     

        private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;
            if (navItemTag == "settings")
            {
                _page = typeof(SettingPage);
            }
            else
            { 
                _page =typeof(StartModePage);
            }
    
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) )
            {
                CardReader.StopKeepReadCard();
                ContentFrame.Navigate(_page, navItemTag, transitionInfo);
            }
        }


        private async void LunchBridge()
        {
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
        }

        private void StackPanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (StText.Text == "未连接到服务")
            {
                LunchBridge();
            }
        }
    }
}
