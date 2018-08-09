using System;
using System.Collections.Generic;
using System.Text;

namespace WalletScaner.Interfaces
{
    public interface ITransaq
    {
        int TimeStamp { get; set; }

        DateTime DateTime { get; }

        String Hash { get; set; }

        String From { get; set; }

        String To { get; set; }

        Decimal Value { get; set; }

        Decimal Amount { get; }

        int IsError { get; set; }

        int TxreceiptStatus { get; set; }
    }
}
