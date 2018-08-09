using System;
using System.Collections.Generic;

using WalletScaner.Interfaces;

using WalletScaner.Ether;
using WalletScaner.Types;

namespace WalletScaner
{

    /// <summary>
    /// class monitors the activity of your wallet. new transaq etc
    /// </summary>
    public class WalletScan
    {
        private List<IWalletWatcher> listWallets;
        private int _interval;

        public WalletScan(List<IWalletWatcher> listWallets)
        {
            this.listWallets = listWallets;
            _interval = 60 * 1000;
        }


        /// <summary>
        /// how often we will ask for new transactions
        /// </summary>
        public int Interval
        {
            set => _interval = value;
        }

        public event SearchEventDelegate SearchEventHandler;

        public void StartWatching()
        {
            foreach (IWalletWatcher wallet in listWallets)
            {
                wallet.SearchTransaqEvent += Wallet_SearchTransaqEvent;
                wallet.StartWatching(_interval);
            }
        }

        public void StoptWatching()
        {
            foreach (IWalletWatcher wallet in listWallets)
            {
                wallet.SearchTransaqEvent -= Wallet_SearchTransaqEvent;
                wallet.Dispose();
            }
        }

        /// <summary>
        /// when there is a new transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="transaqs"></param>
        private void Wallet_SearchTransaqEvent(object sender, List<ITransaq> transaqs)
        {
            
            if (transaqs.Count != 0)
            {
                SearchOnHandler(transaqs);
            }

        }

        private void SearchOnHandler(List<ITransaq> transaqs)
        {
            if (SearchEventHandler != null)
            {
                SearchEventHandler(this, transaqs);
            }
        }
    }
}
