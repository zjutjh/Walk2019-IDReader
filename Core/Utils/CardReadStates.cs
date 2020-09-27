using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils
{
    public enum InitCommState
    {
        OK = 1,
        PortOpenFail = 2,
        Unknow = -1,
        DllLoadErrar = -2
    }

    public enum CloseCommState
    {
        OK = 1,
        WrongPort = 0,
        AlreadyClose = -1,
        DllLoadErrar = -2
    }

    public enum AuthenticateState
    {
        OK = 1,
        FindCardFail = 2,
        SelectCardFail = 3,
        NoReader = 4,
        DllLoadErrar = 0
    }

    public enum ReadFPContentState
    {
        OK = 1,
        ReadCardFail = 0,
        NoReader = 4,
        DllLoadErrar = 99
    }

    public enum ReadContentState
    {
        OK = 1,
        Error = 0
    }
}
