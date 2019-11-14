using IdReader.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace IdReader
{
    public sealed partial class StartModePage : Page
    {
        public StartModePage()
        {
            this.InitializeComponent();
        }
        ~StartModePage()
        {
            if (newView != null)
            {
                newView.CoreWindow.Close();
            }
        }

        private bool normalMode { get; set; } = true;
        private bool endMode { get; set; } = false;
        Frame Screenframe = null;
        CoreApplicationView newView = null;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            base.OnNavigatedTo(e);
            if (e.Parameter == null)
            {
                return;
            }

            var Tag = e.Parameter as string;

            if (Tag == "endE")
            {
                endMode = true;
                normalMode = false;
                modeText.Text = "结束模式";
                unfinished.Visibility = Visibility.Visible;
            }
            else if (Tag == "end")
            {
                endMode = true;
                normalMode = false;
                modeText.Text = "结束模式";

            }
            else if (Tag == "start")
            {
                endMode = false;
            }



        }



        private async void LaunchScreen(object sender, RoutedEventArgs e)
        {
            newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            ApplicationView newAppView = null;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                newView.TitleBar.ExtendViewIntoTitleBar = true;
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                var newWindow = Window.Current;
                newAppView = ApplicationView.GetForCurrentView();

                Screenframe = new Frame();
                Screenframe.Navigate(typeof(OutScreen), null);
                newWindow.Content = Screenframe;
                newWindow.Activate();

                newViewId = newAppView.Id;
            });
            var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
            newAppView.TryEnterFullScreenMode();

        }

        private void ReadCard_Click(object sender, RoutedEventArgs e)
        {

            KeepReadingButton.IsEnabled = false;
            StopKeepReadingButton.IsEnabled = false;
            ReadingButton.IsEnabled = false;
            CCReader.ReadCardOnce();
            ReadingButton.IsEnabled = true;
            KeepReadingButton.IsEnabled = true;
            StopKeepReadingButton.IsEnabled = false;
        }

        private void KeepReadCard_Click(object sender, RoutedEventArgs e)
        {
            KeepReadingButton.IsEnabled = false;
            StopKeepReadingButton.IsEnabled = true;
            ReadingButton.IsEnabled = false;
            CCReader.KeepReadCard();
        }

        private void StopKeepReadCard_Click(object sender, RoutedEventArgs e)
        {
            CardReader.StopKeepReadCard();
            ReadingButton.IsEnabled = true;
            KeepReadingButton.IsEnabled = false;
            StopKeepReadingButton.IsEnabled = true;
        }


        private async void CCReader_IDCardReceived(object sender, EventArgs e)
        {

            Member m = new Member(CCReader.CurrentCard);

            await checkAndStartOutScreen(m);

            GGReader.LoadGroup(m.GroupInfo);
        }

        private async Task<Member> checkAndStartOutScreen(Member m)
        {
            try
            {


                if (!endMode)
                {
                    await m.SetStart();
                }
                else
                {
                    await m.SetBack(normalMode);
                }


                if (m.isOk && m.GroupInfo != null)
                {
                    RxStBack.Background = new SolidColorBrush(Colors.Green);
                    RxStText.Text = "验证通过";
                }
                else
                {
                    RxStBack.Background = new SolidColorBrush(Colors.Red);
                    RxStText.Text = m.msg == null ? "验证异常" : m.msg;
                }

                if (Screenframe != null && newView != null && m != null)
                {

                    try
                    {
                        await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Screenframe.Navigate(typeof(OutScreen), m);
                        });
                    }
                    catch (Exception)
                    {

                    }

                }

            }
            catch (HttpRequestException)
            {
                RxStBack.Background = new SolidColorBrush(Colors.Red);
                RxStText.Text = "网络异常";
            }
            catch (Exception)
            {

                RxStBack.Background = new SolidColorBrush(Colors.Red);
                RxStText.Text = "验证失败";
            }

            return m;
        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

            Member m = new Member(sender.Text);

            m = await checkAndStartOutScreen(m);

            CCReader.LoadCard(m);
            GGReader.LoadGroup(m.GroupInfo);

        }
    }
}
