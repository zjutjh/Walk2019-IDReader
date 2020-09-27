using IdReader.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IdReader
{
    public sealed partial class CardReader : UserControl
    {
        public delegate void IDCardReceivedEventHandler(object sender, System.EventArgs e);
        public event IDCardReceivedEventHandler IDCardReceived;
        private static bool keepReading = false;

        public CardReader()
        {
            this.InitializeComponent();
        }

        public void ReadCardOnce()
        {
            getIdCardOnce();
        }

        public void KeepReadCard()
        {
            keepReading = true;
            KeepgetIdCard();
        }

        public static void StopKeepReadCard()
        {
            keepReading = false;
        }

        public IDCard CurrentCard
        {
            get { return (IDCard)GetValue(CurrentCardProperty); }
            set { SetValue(CurrentCardProperty, value); }
        }
        public static readonly DependencyProperty CurrentCardProperty =
            DependencyProperty.Register("CurrentCard", typeof(IDCard), typeof(MainPage), new PropertyMetadata(new IDCard()));



        private async void getIdCardOnce()
        {
            try
            {
                CurrentCard = await App.coreManager.ReadCard();
                IDCardReceived?.Invoke(this, null);
            }
            catch (Exception)
            {

            }
        }

        public void LoadCard(Member m)
        {
            CurrentCard.name = m.name;

        }
        private async void KeepgetIdCard()
        {

            await Task.Run(async () =>
            {
                try
                {
                    while (keepReading)
                    {
                        CurrentCard = await App.coreManager.ReadCard();
                        IDCardReceived?.Invoke(this, null);
                        Thread.Sleep(800);
                    }
                }
                catch (Exception)
                {


                }
            });

        }
    }
}
