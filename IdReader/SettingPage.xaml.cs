using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static BridgeLibs.AppServiceMessage;

namespace IdReader
{

    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Util.TestMode = true;
        }

        private void ToggleButton_Loaded(object sender, RoutedEventArgs e)
        {
            ((ToggleButton)sender).IsChecked = Util.TestMode;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Util.TestMode = false;
        }

        private async void InstallDevice(object sender, RoutedEventArgs e)
        {
            ValueSet vs = new ValueSet();
            vs.Add("mode", "install");
            vs.Add("path", System.AppDomain.CurrentDomain.BaseDirectory);
            AppServiceConnection connection = App.Connection;
            if (connection != null)
            {
                var vss = await connection.SendMessageAsync(vs);

            }
        }
    }
}
