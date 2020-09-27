using BridgeLibs;
using Core.Utils;
using Newtonsoft.Json;
using Shared;
using System;
using System.Linq;
using System.Threading;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using static BridgeLibs.AppServiceMessage;

namespace Core
{
    public class Program
    {
        static AppServiceConnection connection = null;
        public static IDCardReader reader = null;

        static void Main(string[] args)
        {
            var appServiceThread = new Thread(new ThreadStart(AppServiceThread));
            appServiceThread.Start();
            while (true)
                Thread.Sleep(10000);

        }

        static async void AppServiceThread()
        {
            connection = new AppServiceConnection();
            connection.AppServiceName = "CommunicationServiceX";
            connection.PackageFamilyName = "15275ichen.IDReader_ht7yjew84py3y";
            connection.RequestReceived += Connection_RequestReceived;
            AppServiceConnectionStatus status = await connection.OpenAsync();
            Console.WriteLine(status);
        }


        private async static void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            Console.WriteLine("213");
            ValueSet responseValueSet = new ValueSet();


            if (!args.Request.Message.ContainsKey("mode"))
            {
                responseValueSet.Add("code", AppServiceMessage.AppServiceResponseCode.Unsupport.ToString());
                responseValueSet.Add("msg", "Unsupport");
            }
            else
            {
                AppServiceModeMessage value = AppServiceModeMessageConvert(args.Request.Message["mode"].ToString());
                switch (value)
                {
                    case AppServiceModeMessage.install:

                        InstallDevice(args.Request.Message["path"].ToString());
                        responseValueSet.Add("code", AppServiceMessage.AppServiceResponseCode.OK.ToString());
                        responseValueSet.Add("msg", "OK");
                        break;
                    case AppServiceModeMessage.check:
                        CheckCard(responseValueSet);
                        break;
                    case AppServiceModeMessage.initReader:
                        InitReader(responseValueSet);
                        break;
                    case AppServiceModeMessage.readCard:
                        ReadCard(responseValueSet);
                        break;
                    default:
                        responseValueSet.Add("code", AppServiceMessage.AppServiceResponseCode.Unsupport.ToString());
                        responseValueSet.Add("msg", "Unsupport");
                        break;
                }

            }


            await args.Request.SendResponseAsync(responseValueSet);
        }

        private static void CheckCard(ValueSet valueSet)
        {
            if (reader == null)
                valueSet.Add("code", AppServiceMessage.AppServiceResponseCode.noReader.ToString());
            else
                try
                {
                    reader.PrepareRead();
                    valueSet.Add("code", AppServiceMessage.AppServiceResponseCode.OK.ToString());
                }
                catch (NoReaderException)
                {
                    valueSet.Add("code", AppServiceMessage.AppServiceResponseCode.noReader.ToString());
                }
               
        }

        private static void ReadCard(ValueSet valueSet)
        {
            try
            {
                var card = reader.Read();
                valueSet.Add("code", AppServiceMessage.AppServiceResponseCode.OK.ToString());
                valueSet.Add("data", Newtonsoft.Json.JsonConvert.SerializeObject(card));
            }
            catch (Exception)
            {
                valueSet.Add("code", AppServiceMessage.AppServiceResponseCode.Unknow.ToString());
                reader = null;
            }
        }

        private static void InitReader(ValueSet valueSet)
        {
            try
            {
                reader = new IDCardReader();
                valueSet.Add("code", AppServiceMessage.AppServiceResponseCode.OK.ToString());
            }
            catch (Exception)
            {
                reader = null;
                valueSet.Add("code", AppServiceMessage.AppServiceResponseCode.noReader.ToString());

            }
        }

        private static void InstallDevice(string path)
        {
            CoreUtils.RunBat(path + "\\Core\\device\\install.bat");
        }


    }
}
