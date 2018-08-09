using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RestSharp;

using WalletScaner.Interfaces;
using WalletScaner.Ether.Common;
using WalletScaner.Types;

namespace WalletScaner.Ether
{
    public class EtherScan : IWalletWatcher
    {
        private const string API_URL = "http://api.etherscan.io";

        private const int _offset = 100;

        private EtherTransaq _lastEtherTransaq;
        private RestClient _restClient;

        private EtherData etherData;
        private Timer timer;

        private event SearchEventDelegate someSearchMessageEvent;

        public EtherScan(String apiKey, String address)
        {
            _restClient = new RestClient(API_URL);

            etherData = new EtherData(apiKey);
            etherData.Adress = address;
        }


        public void StartWatching( int interval)
        {

            _lastEtherTransaq = GetLastTransaq(etherData.Adress);


            TimerCallback tm = new TimerCallback(Scan);
            timer = new Timer(tm, null, 0, interval);
        }

        public void StopWatching()
        {
            timer.Dispose();
        }

        private object synclock = new object();
        private void Scan(object obj)
        {
            
            
            List<ITransaq> currentEtherTransaqs = new List<ITransaq>();
            GenerateNewTransaqList(currentEtherTransaqs);
           
            SearchOnHandler(currentEtherTransaqs);
        }

        private void GenerateNewTransaqList(List<ITransaq> currentEtherTransaqs)
        {
            //currentEtherTransaqs = new List<ITransaq>();
            Monitor.Enter(synclock);

            bool IsNewTransaq = false;
            EtherTransaq etherTransaqTemp = new EtherTransaq();
            etherTransaqTemp = _lastEtherTransaq;
            bool isDone = false;
            int page = 1;
            do
            {   
                isDone = GenerarePartTransaqList(currentEtherTransaqs, ref IsNewTransaq , ref etherTransaqTemp, page);
                page++;
            }
            while (isDone == false);

            Monitor.Exit(synclock);
        }

        private bool GenerarePartTransaqList(List<ITransaq> currentEtherTransaqs, 
            ref bool IsNewTransaq ,ref EtherTransaq etherTransaqTemp, int page)
        {

            RestRequest request = RequestGenerator.GetTxListReq(etherData, page, _offset);
            List<EtherTransaq> etherTransaqs = ExecuteAs<List<EtherTransaq>>(_restClient, request);

            if (etherTransaqs == null || etherTransaqs.Count == 0)
            {
                return true;
            }

            foreach (EtherTransaq etherTransaq in etherTransaqs)
            {
                if (etherTransaq.DateTime > _lastEtherTransaq.DateTime)
                {
                    currentEtherTransaqs.Add(etherTransaq);

                    if (IsNewTransaq == false)
                    {
                        etherTransaqTemp = etherTransaq;
                        IsNewTransaq = true;
                    }

                }
                else
                {
                    _lastEtherTransaq = etherTransaqTemp;
                    return true;
                }
            }
            return false;
        }

        private EtherTransaq GetLastTransaq(String address)
        {
            //etherData.Adress = address;
            RestRequest request = RequestGenerator.GetTxListReq(etherData, 1, 1);
            List<EtherTransaq> etherTransaqs = ExecuteAs<List<EtherTransaq>>(_restClient, request);
            if (etherTransaqs.Count != 0)
            {
                return etherTransaqs[0];
            }
            else
            {
                return new EtherTransaq();
            }
        }

        private T ExecuteAs<T>(RestClient rest, RestRequest request)
        {
            
            var response = rest.ExecuteAsGet<EtherResponse<T>>(request, "GET");
            if (response.Data != null)
                return response.Data.Result;
            else
                return default(T);
        }

        

        event SearchEventDelegate IWalletWatcher.SearchTransaqEvent
        {
            add
            {
                someSearchMessageEvent += value;
            }
            remove
            {
                someSearchMessageEvent -= value;
            }
        }

        private void SearchOnHandler(List<ITransaq> transaqs)
        {
            if (someSearchMessageEvent != null)
            {
                someSearchMessageEvent(this, transaqs);
            }
        }


        public void Dispose()
        {
            StopWatching();
        }
    }
}
