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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace IdReader
{
    public sealed partial class CardReader : UserControl
    {
        private static bool keepReading = false;

        public CardReader()
        {
            this.InitializeComponent();
        }

        public void ReadCardOnce()
        {
            getIdCardOnce();
        }

        public  void KeepReadCard()
        {
            keepReading = true;
            KeepgetIdCard();
        }

        public static void StopKeepReadCard()
        {
            keepReading = false;
        }

        public delegate void IDCardReceivedEventHandler(object sender, System.EventArgs e);
        public event IDCardReceivedEventHandler IDCardReceived;

        public IDCard CurrentCard
        {
            get { return (IDCard)GetValue(CurrentCardProperty); }
            set { SetValue(CurrentCardProperty, value); }
        }
        public static readonly DependencyProperty CurrentCardProperty =
            DependencyProperty.Register("CurrentCard", typeof(IDCard), typeof(MainPage), new PropertyMetadata(new IDCard()));

       

        private async void getIdCardOnce()
        {
            ValueSet vs = new ValueSet();
            vs.Add("Command", "IDCard");
            AppServiceConnection connection = App.Connection;
            if (connection != null)
            {
                var vss = await connection.SendMessageAsync(vs);
                var arry = vss.Message.ToArray();
                if (vss.Message.Keys.Count >= 1)
                {
                    CurrentCard = Newtonsoft.Json.JsonConvert.DeserializeObject<IDCard>(arry[0].Value as String);
                    IDCardReceived?.Invoke(this, null);
                }


            }
        }
        public void LoadCard(Member m)
        {
            CurrentCard.name = m.name;

        }
        private async void KeepgetIdCard()
        {
            ValueSet vs = new ValueSet();
            vs.Add("Command", "IDCard");
            AppServiceConnection connection = App.Connection;
            if (connection != null)
            {
                await Task.Run(async () =>
                {

                    while (keepReading)
                    {
                        var vss = await connection.SendMessageAsync(vs);
                        var arry = vss.Message.ToArray();
                        if (vss.Message.Keys.Count >= 1)
                        {

                            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                            {
                                CurrentCard = Newtonsoft.Json.JsonConvert.DeserializeObject<IDCard>(  arry[0].Value as String);
                                IDCardReceived?.Invoke(this, null);
                            });     
                        }

                        Thread.Sleep(800);
                    }

                });


            }
        }
    }
}
