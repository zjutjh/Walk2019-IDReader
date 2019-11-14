using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace Core
{
   public class Program
    {
        static AppServiceConnection connection = null;
        public static IDCardReader reader = null;
        /// <summary>
        /// Creates an app service thread
        /// </summary>
        static void Main(string[] args)
        {

            try
            {
                reader = new IDCardReader();
            }
            catch (Exception)
            {
            }


            Thread appServiceThread = new Thread(new ThreadStart(ThreadProc));
            appServiceThread.Start();
            while (true)
            {
                Thread.Sleep(10000);
            }


        }

        /// <summary>
        /// Creates the app service connection
        /// </summary>
        static async void ThreadProc()
        {
            connection = new AppServiceConnection();
            connection.AppServiceName = "CommunicationServiceX";
            connection.PackageFamilyName = "15275ichen.IDReader_ht7yjew84py3y";
            connection.RequestReceived += Connection_RequestReceived;

            AppServiceConnectionStatus status = await connection.OpenAsync();
            switch (status)
            {
                case AppServiceConnectionStatus.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Connection established - waiting for requests");
                    Console.WriteLine();
                    break;
                case AppServiceConnectionStatus.AppNotInstalled:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The app AppServicesProvider is not installed.");
                    return;
                case AppServiceConnectionStatus.AppUnavailable:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The app AppServicesProvider is not available.");
                    return;
                case AppServiceConnectionStatus.AppServiceUnavailable:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("The app AppServicesProvider is installed but it does not provide the app service {0}.", connection.AppServiceName));
                    return;
                case AppServiceConnectionStatus.Unknown:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("An unkown error occurred while we were trying to open an AppServiceConnection."));
                    return;
            }
        }

        /// <summary>
        /// Receives message from UWP app and sends a response back
        /// </summary>
        private static async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            string key = args.Request.Message.First().Key;
            string value = args.Request.Message.First().Value.ToString();

            ValueSet valueSet = new ValueSet();


            if (key == "check")
            {
                if (reader != null)
                {
                    valueSet.Add("run", "run");
                }
                else
                {
                   
                    try
                    {
                        reader = new IDCardReader();
                        valueSet.Add("run", "run");
                    }
                    catch (Exception)
                    {
                        valueSet.Add("noReader", "run");
                    }
                }
            }
            else if (key == "install")
            {
                RunBat(value + "Core\\device\\install.bat");
            }
            else
            {
                if (reader == null)
                {
                    try
                    {
                        reader = new IDCardReader();
                    }
                    catch
                    {

                    }
                }
                if (reader != null)
                {
                    var idcard = reader.Read();
                    if (idcard != null)
                    {
                        if (idcard.number != "")
                        {
                            valueSet.Add("data", ConvertJsonString(idcard));
                        }

                    }
                    else
                    {
                      
                    }
                }
              

            }

            await args.Request.SendResponseAsync(valueSet);
        }



        private static string ConvertJsonString(object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }

            return null;
        }


        private static void RunBat(string batPath)
        {
            Process pro = new Process();
            FileInfo file = new FileInfo(batPath);
            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            pro.StartInfo.FileName = batPath;
            pro.StartInfo.CreateNoWindow = false;
            pro.Start();
        }

    }
}
