using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobutsuShogi.network
{
    enum ClientState
    {
        Connecting,
        Connected,
        SendingData,
        DataSent,
        ReadingResponse,
        ResponseRead

    }
}
