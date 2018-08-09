using System;
using System.Collections.Generic;
using System.Text;

namespace WalletScaner.Ether.Common
{
    class EtherData
    {
        private string _apikey;
        private String  _address;

        public EtherData(string apiKey)
        {
            _apikey = apiKey;
        }

        public string ApiKey
        {
            get => _apikey;
        }

        public String Adress
        {
            get => _address;
            set => _address = value;
        }

    }
}
