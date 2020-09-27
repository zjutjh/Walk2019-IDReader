using System;

namespace BridgeLibs
{
    public class AppServiceMessage
    {
        public enum AppServiceResponseCode
        {
            OK = 200,
            Unsupport = 415,
            CoreError = 500,
            noReader,
            DLLError,
            Unknow

        }

        public static AppServiceResponseCode AppServiceResponseCodeConvert(string type)
        {
            switch (type)
            {
                case "OK":
                    return AppServiceResponseCode.OK;
                case "noReader":
                    return AppServiceResponseCode.noReader;
                case "Unsupport":
                    return AppServiceResponseCode.Unknow;

                default:
                    return AppServiceResponseCode.Unknow;
            }
        }
        public static AppServiceModeMessage AppServiceModeMessageConvert(string type)
        {
            switch (type)
            {
                case "install":
                    return AppServiceModeMessage.install;
                case "check":
                    return AppServiceModeMessage.check;
                case "initReader":
                    return AppServiceModeMessage.initReader;
                case "readCard":
                    return AppServiceModeMessage.readCard;
                default:
                    return AppServiceModeMessage.unknow;
            }
        }
       
        public enum AppServiceModeMessage
        {
            install,
            check,
            initReader,
            readCard,
            unknow
        }



        public enum AppServiceStatus
        {
            OK, noReader, DisConnect
        }

    }
}
