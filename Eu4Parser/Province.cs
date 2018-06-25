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
        public string culture;
        public string tradeGood;
        public string capital;
        public bool centerOfTrade;
        public bool city;
        public bool hre;
        public int tax;
        public int production;
        public int manpower;
        public int development;
        public int id;
        public bool coal;
        public bool fort;
        public Province(int id, string owner, string controller, string capital, string tradeGood, string religion , string culture, 
            int tax, int production, int manpower,
            bool centerOfTrade, bool city, bool hre, bool coal, bool fort)
        {
            this.id = id;
            this.owner = owner;
            this.controller = controller;
            this.capital = capital;
            this.tradeGood = tradeGood;
            this.religion = religion;
            this.culture = culture;
            this.tax = tax;
            this.production = production;
            this.manpower = manpower;

            development = tax + production + manpower;

            this.centerOfTrade = centerOfTrade;
            this.city = city;
            this.hre = hre;
            this.coal = coal;
            this.fort = fort;

        }
        public void Print()
        {
            Console.WriteLine(id + "-" + owner + " " + capital + " Development: " + development + "(" + tax + ", " + production + ", " + manpower + ")" +
                " Culture: " + culture + " Religion: " + religion + " Trade Good: " + tradeGood + " Has coal: " + coal + " In hre: " + hre +
                " Is a city: " + city + " Is a center of trade: " + centerOfTrade +" Has a fort: "+ fort);
        } 
    }
}
