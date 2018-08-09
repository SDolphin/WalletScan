using System;
using System.Collections.Generic;
using System.Text;

using RestSharp;

namespace WalletScaner.Ether.Common
{
    static class RequestGenerator
    {
        private static RestRequest BaseRequest()
        {
            RestRequest request = new RestRequest("/api");
            request.AddParameter("module", "account");
            return request;
        }

        public static RestRequest GetTxListReq(EtherData etherData, int page, int offset, string sort = "desc")
        {
            RestRequest request = BaseRequest();
            request.AddParameter("action", "txlist");
            //request.AddParameter("address", String.Join(',', etherData.Adress));
            request.AddParameter("address", etherData.Adress);
            request.AddParameter("startblock", "0");
            request.AddParameter("endblock", "99999999");
            request.AddParameter("page", page);
            request.AddParameter("offset", offset);
            request.AddParameter("sort", sort);
            request.AddParameter("apikey", etherData.ApiKey);
            return request;
        }

    }
}
