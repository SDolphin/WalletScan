using System;
using System.Collections.Generic;
using System.Text;

using WalletScaner.Interfaces;

namespace WalletScaner.Types
{
     public delegate void SearchEventDelegate(object sender, List<ITransaq> transaqs);
}
