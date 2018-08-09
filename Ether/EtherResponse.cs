using System;
using System.Collections.Generic;
using System.Text;

namespace WalletScaner.Ether
{
    class EtherResponse<T>
    {

        public int Status { get; set; }

        public String Message { get; set; }

        public T Result { get; set; }

    }
}
