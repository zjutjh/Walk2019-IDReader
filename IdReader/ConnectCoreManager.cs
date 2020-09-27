using IdReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using static BridgeLibs.AppServiceMessage;

namespace IdReader
{
    public class CheckResponseChangedArgs : EventArgs
    {
        public AppServiceStatus message;
        public CheckResponseChangedArgs(AppServiceStatus msg)
        {
            message = msg;
        }
    }


    public class ConnectCoreManager
    {

        public delegate void CheckResponseChangedHandler(object sender, CheckResponseChangedArgs e);
        public event CheckResponseChangedHandler CheckResponseChanged;
       
        public AppServiceStatus appServiceStatus = AppServiceStatus.DisConnect;
        public ConnectCoreManager()
        {
            if (App.Connection == null)
            {
                LaunchBridge();
            }
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer.Tick += CheckConnectService;
            timer.Start();

        }

        public async void LaunchBridge()
        {
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
        }

        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            if (appServiceStatus != AppServiceStatus.DisConnect)
            {
                appServiceStatus = AppServiceStatus.DisConnect;
                CheckResponseChanged?.Invoke(this, new CheckResponseChangedArgs(AppServiceStatus.DisConnect));
            }
        }

        private static ValueSet GetModeValueSet(AppServiceModeMessage mode)
        {
            ValueSet testValue = new ValueSet();
            testValue.Add("mode", mode.ToString());
            return testValue;
        }


        public async Task<IDCard> ReadCard()
        {
            if (appServiceStatus != AppServiceStatus.OK)
                throw new Exception();

            try
            {
                ValueSet testValue = GetModeValueSet(AppServiceModeMessage.readCard);
                var response = await App.Connection.SendMessageAsync(testValue);
                CheckResponse(response);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<IDCard>(response.Message["Data"].ToString());
            }
            catch (Exception)
            {

                if (appServiceStatus != AppServiceStatus.DisConnect)
                {
                    appServiceStatus = AppServiceStatus.DisConnect;
                    CheckResponseChanged?.Invoke(this, new CheckResponseChangedArgs(AppServiceStatus.DisConnect));
                }
                throw new Exception();
            }

        }

        private async void CheckConnectService(object sender, object e)
        {
            ValueSet testValue = GetModeValueSet(AppServiceModeMessage.check);
            try
            {
                var response = await App.Connection.SendMessageAsync(testValue);
                CheckResponse(response);
            }
            catch (Exception)
            {
                if (appServiceStatus != AppServiceStatus.DisConnect)
                {
                    appServiceStatus = AppServiceStatus.DisConnect;
                    CheckResponseChanged?.Invoke(this, new CheckResponseChangedArgs(AppServiceStatus.DisConnect));
                }
            }
        }


        private async void InitCardReader()
        {
            ValueSet testValue = GetModeValueSet(AppServiceModeMessage.initReader);
            try
            {
                var response = await App.Connection.SendMessageAsync(testValue);
            
            }
            catch (Exception)
            {
                if (appServiceStatus != AppServiceStatus.DisConnect)
                {
                    appServiceStatus = AppServiceStatus.DisConnect;
                    CheckResponseChanged?.Invoke(this, new CheckResponseChangedArgs(AppServiceStatus.DisConnect));
                }
            }
        }



        private void CheckResponse(AppServiceResponse response)
        {
            if (response.Status == AppServiceResponseStatus.Success)

                switch (AppServiceResponseCodeConvert(response.Message["code"].ToString()))
                {
                    case AppServiceResponseCode.OK:
                        if (appServiceStatus != AppServiceStatus.OK)
                        {
                            appServiceStatus = AppServiceStatus.OK;
                            CheckResponseChanged?.Invoke(this, new CheckResponseChangedArgs(AppServiceStatus.OK));
                        }

                        break;
                    case AppServiceResponseCode.noReader:
                        if (appServiceStatus != AppServiceStatus.noReader)
                        {
                            appServiceStatus = AppServiceStatus.noReader;
                            CheckResponseChanged?.Invoke(this, new CheckResponseChangedArgs(AppServiceStatus.noReader));
                        
                        }
                        InitCardReader();
                        break;
                    default:
                        if (appServiceStatus != AppServiceStatus.DisConnect)
                        {
                            appServiceStatus = AppServiceStatus.DisConnect;
                            CheckResponseChanged?.Invoke(this, new CheckResponseChangedArgs(AppServiceStatus.DisConnect));
                        }
                        break;
                }
        }
    }
}