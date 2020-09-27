using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CVR100A
{
    /// <summary>
    /// 身份证阅读类
    /// </summary>
    class CVRSDK
    {
        [DllImport("Termb.dll", EntryPoint = "CVR_InitComm", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_InitComm(int Port);


        [DllImport("Termb.dll", EntryPoint = "CVR_Authenticate", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_Authenticate();


        [DllImport("Termb.dll", EntryPoint = "CVR_Read_Content", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_Read_Content(int Active);


        [DllImport("Termb.dll", EntryPoint = "CVR_Read_FPContent", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_Read_FPContent();

        [DllImport("Termb.dll", EntryPoint = "CVR_FindCard", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_FindCard();

        [DllImport("Termb.dll", EntryPoint = "CVR_SelectCard", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_SelectCard();


        [DllImport("Termb.dll", EntryPoint = "CVR_CloseComm", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int CVR_CloseComm();

        [DllImport("Termb.dll", EntryPoint = "GetCertType", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern unsafe int GetCertType(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetPeopleName", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern unsafe int GetPeopleName(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetPeopleChineseName", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern unsafe int GetPeopleChineseName(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetPeopleNation", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int GetPeopleNation(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetNationCode", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int GetNationCode(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetPeopleBirthday", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleBirthday(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetPeopleAddress", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleAddress(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetPeopleIDCode", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleIDCode(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetDepartment", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetDepartment(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetStartDate", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetStartDate(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetEndDate", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetEndDate(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "GetPeopleSex", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleSex(ref byte strTmp, ref int strLen);


        [DllImport("Termb.dll", EntryPoint = "CVR_GetSAMID", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int CVR_GetSAMID(ref byte strTmp);

        [DllImport("Termb.dll", EntryPoint = "GetBMPData", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetBMPData(ref byte btBmp, ref int nLen);

        [DllImport("Termb.dll", EntryPoint = "GetJpgData", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetJpgData(ref byte btBmp, ref int nLen);

        [DllImport("Termb.dll", EntryPoint = "GetJpgData", CharSet = CharSet.Auto, SetLastError = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int Getbase64BMPData(ref byte pData, ref int pLen);
    }

}
