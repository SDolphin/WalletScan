using System;
using System.Collections.Generic;
using System.Text;

using WalletScaner.Types;

namespace WalletScaner.Interfaces
{
    public interface IWalletWatcher : IDisposable
    {

        event SearchEventDelegate SearchTransaqEvent;

        void StartWatching(int interval);

        void StopWatching();


    }
}
