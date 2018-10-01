using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.IMessaging
{
    public interface IRandomGenerator
    {
        string Receive();

        string Receive(int timeout);

        void Send(string rndCode);
    }
}
