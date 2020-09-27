using Core.Utils;
using CVR100A;
using Shared;
using System;
using System.IO;
using System.Text;
using Windows.Graphics.Display;
using Windows.Storage.Provider;

namespace Core
{
    public class IDCardReader
    {
        public IDCardReader()
        {
            InitIdReader();
        }

        ~IDCardReader()
        {
            CVRSDK.CVR_CloseComm();
        }

        public IDCard Read()
        {
            if (PrepareRead())
                return GetData();
            return null;
        }

        public void InitIdReader()
        {
            InitCommState USBState = 0;
            for (int Port = 1001; Port <= 1016; Port++)
            {
                USBState = (InitCommState)CVRSDK.CVR_InitComm(Port);
                if (USBState == InitCommState.OK) return;
            }

            switch (USBState)
            {
                case InitCommState.PortOpenFail:
                    throw new Exception();
                case InitCommState.Unknow:
                    throw new Exception();
                case InitCommState.DllLoadErrar:
                    throw new DllLoadException();
            }

        }

        public bool PrepareRead()
        {
            AuthenticateState authenticateState = (AuthenticateState)CVRSDK.CVR_Authenticate();
            switch (authenticateState)
            {
                case AuthenticateState.OK:
                    int readContent = CVRSDK.CVR_Read_Content(1);
                    if (readContent == 1)
                        return true;
                    break;
                case AuthenticateState.FindCardFail:
                    break;
                case AuthenticateState.SelectCardFail:
                    break;
                case AuthenticateState.NoReader:
                    throw new NoReaderException();
                case AuthenticateState.DllLoadErrar:
                    throw new DllLoadException();
            }
            return false;
        }
        public delegate int ReadFunction(ref byte a, ref int b);

        private string ReadCardData(int length, ReadFunction function, string encode = "GB2312")
        {
            byte[] item = new byte[length];
            function(ref item[0], ref length);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding(encode).GetString(item).Trim('\0');
        }

        private IDCard GetData()
        {
            var imgData = ReadCardData(40960, CVRSDK.Getbase64BMPData);
            var name = ReadCardData(128, CVRSDK.GetPeopleName);
            var cnName = ReadCardData(128, CVRSDK.GetPeopleChineseName);
            var number = ReadCardData(128, CVRSDK.GetPeopleIDCode);
            var peopleNation = ReadCardData(128, CVRSDK.GetPeopleNation);
            var peopleNationCode = ReadCardData(128, CVRSDK.GetNationCode);
            var validtermOfStart = ReadCardData(128, CVRSDK.GetStartDate);
            var birthday = ReadCardData(128, CVRSDK.GetPeopleBirthday);
            var address = ReadCardData(128, CVRSDK.GetPeopleAddress);
            var validtermOfEnd = ReadCardData(128, CVRSDK.GetEndDate);
            var signdate = ReadCardData(128, CVRSDK.GetDepartment);
            var sex = ReadCardData(128, CVRSDK.GetPeopleSex);
            var certType = ReadCardData(32, CVRSDK.GetCertType, "ASCII");

            byte[] samid = new byte[128];
            CVRSDK.CVR_GetSAMID(ref samid[0]);

            bool bCivic = true;

            int nStart = certType.IndexOf("I");
            if (nStart != -1) bCivic = false;

            if (!bCivic) throw new NotSupportException();

            IDCard card = new IDCard()
            {
                ImgString = imgData,
                sex = sex,
                name = name,
                number = number,
                peopleNation = peopleNation,
                birthday = birthday,
                address = address,
                signdate = signdate,
                validtermOfStart = validtermOfStart,
                samid = Encoding.GetEncoding("GB18030").GetString(samid).Trim('\0'),
            };
            return card;
        }

    }
}
