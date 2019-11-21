using CVR100A;
using Shared;
using System;
using System.IO;
using System.Text;

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
            if (prepareRead())
            {
                return GetData();
            }
            return null;
        }

        public void InitIdReader()
        {
         
                int iPort, iRetUSB = 0;
                for (iPort = 1001; iPort <= 1016; iPort++)
                {
                    iRetUSB = CVRSDK.CVR_InitComm(iPort);
                    if (iRetUSB == 1)
                    {
                        break;
                    }
                }

            if (iRetUSB != 1)
                throw new Exception();
            
        }

        private bool prepareRead()
        {
            int authenticate = CVRSDK.CVR_Authenticate();
            if (authenticate == 1)
            {
                int readContent = CVRSDK.CVR_Read_Content(1);
               
                if (readContent == 1)
                {
                    return true;
                }

            }
            else if(authenticate==4)
            {
                Program.reader = null;
            }
            return false;
        }

        private IDCard GetData()
        {
            try
            {
                //byte[] imgData = new byte[40960];
                int length = 40960;
                //CVRSDK.Getbase64BMPData(ref imgData[0], ref length);

                byte[] name = new byte[128];
                length = 128;
                CVRSDK.GetPeopleName(ref name[0], ref length);

                byte[] cnName = new byte[128];
                length = 128;
                CVRSDK.GetPeopleChineseName(ref cnName[0], ref length);

                byte[] number = new byte[128];
                length = 128;
                CVRSDK.GetPeopleIDCode(ref number[0], ref length);

                byte[] peopleNation = new byte[128];
                length = 128;
                CVRSDK.GetPeopleNation(ref peopleNation[0], ref length);

                byte[] peopleNationCode = new byte[128];
                length = 128;
                CVRSDK.GetNationCode(ref peopleNationCode[0], ref length);

                byte[] validtermOfStart = new byte[128];
                length = 128;
                CVRSDK.GetStartDate(ref validtermOfStart[0], ref length);

                byte[] birthday = new byte[128];
                length = 128;
                CVRSDK.GetPeopleBirthday(ref birthday[0], ref length);

                byte[] address = new byte[128];
                length = 128;
                CVRSDK.GetPeopleAddress(ref address[0], ref length);

                byte[] validtermOfEnd = new byte[128];
                length = 128;
                CVRSDK.GetEndDate(ref validtermOfEnd[0], ref length);

                byte[] signdate = new byte[128];
                length = 128;
                CVRSDK.GetDepartment(ref signdate[0], ref length);

                byte[] sex = new byte[128];
                length = 128;
                CVRSDK.GetPeopleSex(ref sex[0], ref length);

                byte[] samid = new byte[128];
                CVRSDK.CVR_GetSAMID(ref samid[0]);

                bool bCivic = true;
                byte[] certType = new byte[32];
                length = 32;
                CVRSDK.GetCertType(ref certType[0], ref length);

                string strType = System.Text.Encoding.ASCII.GetString(certType);
                int nStart = strType.IndexOf("I");
                if (nStart != -1) bCivic = false;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var encoding = Encoding.GetEncoding("GB18030");
                if (bCivic)
                {
                    IDCard card = new IDCard()
                    {
                        //ImgString = Encoding.GetEncoding("GB2312").GetString(imgData).Trim('\0'),
                        sex = Encoding.GetEncoding("GB2312").GetString(sex).Trim('\0'),
                        name = Encoding.GetEncoding("GB18030").GetString(name).Trim('\0'),
                        number = Encoding.GetEncoding("GB18030").GetString(number).Trim('\0'),
                        peopleNation = Encoding.GetEncoding("GB18030").GetString(peopleNation).Trim('\0'),
                        birthday= Encoding.GetEncoding("GB18030").GetString(birthday).Trim('\0'),
                        address = Encoding.GetEncoding("GB18030").GetString(address).Trim('\0'),
                        signdate = Encoding.GetEncoding("GB18030").GetString(signdate).Trim('\0'),
                        validtermOfStart = Encoding.GetEncoding("GB18030").GetString(validtermOfStart).Trim('\0'),
                        samid = Encoding.GetEncoding("GB18030").GetString(samid).Trim('\0'),
                    };
                    return card;
                }

            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
            }
            return new IDCard();
        }

    }
}
