using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Eu4Parser
{
    

            
    class Country
    {
        public string tag;
        public string name;
        public string primaryCulture;
        public string techGroup;
        public string governmentType;
        public string religion;
        public List<Province> provinces;
        public int development;
        public int totalTax;
        public int totalProduction;
        public int totalManpower;
        public int captialId;
        public string capitalName;
        public Color color;
        public int mercantilism;
        public bool existsIn1444;
        public bool inHRE;

        public Dictionary<string, int> numberOfTradeGoods;
        public int numberOfForts;


        public Country(string tag, string primaryCulture, string techGroup, string governmentType, 
            string religion, List<Province> provinces, int captialId, int mercantilism)
        {
            this.tag = tag;
           
            this.primaryCulture = primaryCulture;
            this.techGroup = techGroup;
            this.governmentType = governmentType;
            this.religion = religion;
            this.provinces = provinces;
            if(provinces.Count >0)
            { existsIn1444 = true; }
            this.captialId = captialId;
            
            this.mercantilism = mercantilism;

            development = 0;
            totalTax = 0;
            totalProduction = 0;
            totalManpower = 0;
           
            FindCapitalName();
            FindHREStatus();
            CalculateProvinceNumbers();
            CalculateDevelopment();
          
            
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public void SetColour(string r, string g, string b)
        {
            try
            {
                color =  Color.FromArgb( int.Parse(r), int.Parse(g), int.Parse(b));
            }
            catch
            {

            }
        }
        void CalculateProvinceNumbers()
        {
            numberOfTradeGoods = new Dictionary<string, int>();

            numberOfForts = 0;
            foreach (Province province in provinces)
            {
                if (!numberOfTradeGoods.ContainsKey(province.tradeGood))
                {
                    numberOfTradeGoods.Add(province.tradeGood, 1);
                }
                else
                {
                    numberOfTradeGoods[province.tradeGood]++;
                }
                if (province.coal)
                {
                    if (!numberOfTradeGoods.ContainsKey("coal"))
                    {
                        numberOfTradeGoods.Add("coal", 1);
                    }
                    else
                    {
                        numberOfTradeGoods["coal"]++;
                    }
                }
                if (province.fort)
                {
                    numberOfForts++;
                }
            }
        }
        void CalculateDevelopment()
        {
            foreach (Province province in provinces)
            {
                if (province.city)
                {
                    development += province.development;
                    totalTax += province.tax;
                    totalProduction += province.production;
                    totalManpower += province.manpower;
                }
                
            }
           
        }
        void FindCapitalName()
        {
            foreach (Province province in provinces)
            {
                if(province.id == captialId)
                {
                    capitalName = province.capital;
                }
            }
        }
        void FindHREStatus()
        {
            foreach (Province province in provinces)
            {
                if (province.id == captialId )
                {
                    inHRE = province.hre;
                }
            }
        }
        public void Print()
        {
            if (existsIn1444)
                // Console.WriteLine(tag + " " + name + " " + development + " (" + totalTax + ", " + totalProduction + ", " + totalManpower + ") " + primaryCulture + " " + religion + " " + capitalName + " " + techGroup + " " + governmentType + " " + " Exists in 1444 " + existsIn1444 + " In hre " + inHRE);
                if (ClosestConsoleColor(color.R, color.G, color.B) != ConsoleColor.Black)
                {
                    Console.ForegroundColor = ClosestConsoleColor(color.R, color.G, color.B);
                }
                 else
                {
                    Console.ForegroundColor = ClosestConsoleColor(color.R, color.G, color.B);
                    Console.BackgroundColor = ConsoleColor.White;
                }

            Console.WriteLine(tag + " " + name + " " + development + " (" + totalTax + ", " + totalProduction + ", " + totalManpower + ")");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        static ConsoleColor ClosestConsoleColor(byte r, byte g, byte b)
        {
            ConsoleColor ret = 0;
            double rr = r, gg = g, bb = b, delta = double.MaxValue;

            foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
            {
                var n = Enum.GetName(typeof(ConsoleColor), cc);
                var c = Color.FromName(n == "DarkYellow" ? "Orange" : n); 
                var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
                if (t == 0.0)
                    return cc;
                if (t < delta)
                {
                    delta = t;
                    ret = cc;
                }
            }
            return ret;
        }
    }
}
