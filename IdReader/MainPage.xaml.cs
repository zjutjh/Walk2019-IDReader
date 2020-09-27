using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using muxc = Microsoft.UI.Xaml.Controls;
namespace IdReader
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        } 

        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.coreManager.CheckResponseChanged += CoreManager_CheckResponseReceived;
            App.coreManager.LaunchBridge();
            ContentFrame.Navigate(typeof(StartModePage), "start");
        }

        private void CoreManager_CheckResponseReceived(object sender, CheckResponseChangedArgs e)
        {
            switch (e.message)
            {
                case BridgeLibs.AppServiceMessage.AppServiceStatus.OK:
                    SystemInfoColor.Background = new SolidColorBrush(Colors.Green);
                    break;
                case BridgeLibs.AppServiceMessage.AppServiceStatus.noReader:
                    SystemInfoColor.Background = new SolidColorBrush(Colors.Red);
                    break;
                case BridgeLibs.AppServiceMessage.AppServiceStatus.DisConnect:
                    SystemInfoColor.Background = new SolidColorBrush(Colors.Yellow);
                    break;
                default:
                    break;
            }
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


       

        private void StackPanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (App.coreManager.appServiceStatus == BridgeLibs.AppServiceMessage.AppServiceStatus.DisConnect)
                App.coreManager.LaunchBridge();

        }
    }
}
