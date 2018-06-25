using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Parser
{
    class Province
    {
        public string owner;
        public string controller;
        public string religion;
        public string tradeGood;
        public string capital;
        public bool centerOfTrade;
        public bool city;
        public bool hre;
        int tax;
        int production;
        int manpower;
        int id;
        public Province(int id, string owner, string controller, string captial, string tradeGood, string religion, 
            int tax, int production, int manpower,
            bool centerOfTrade, bool city, bool hre)
        {
            this.id = id;
        }
    }
}
