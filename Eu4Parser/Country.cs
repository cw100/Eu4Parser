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
        public bool existsInCurrentDate;
        public bool inHRE;

        public Dictionary<string, int> numberOfTradeGoods;
        public int numberOfForts;

        public List<Country> vassals;
        public List<Country> alliances;
        public List<Country> unions;
        public List<Country> tradeLeague;
        public List<Country> royalMarriages;
        public List<Country> tributaries;
        public Country vassalOverlord;
        public Country unionOverlord;
        public List<Country> guaranteeing;
        public List<Country> guaranteedBy;
        public Country tradeLeagueLeader;
        public Country tributaryOverlord;
        public bool isHREEmperor;
        public bool isCelestialEmperor;
        public bool isTradeLeader;
        public Country(string tag, string primaryCulture, string techGroup, string governmentType, 
            string religion, List<Province> provinces, int captialId, int mercantilism)
        {

            vassals = new List<Country>();
            alliances = new List<Country>();
            unions = new List<Country>();
            royalMarriages = new List<Country>();
            tributaries = new List<Country>();
            tradeLeague = new List<Country>();
            guaranteeing = new List<Country>();
            guaranteedBy = new List<Country>();

            isHREEmperor = false;
            isCelestialEmperor = false;
            isTradeLeader = false;
            this.tag = tag;
           
            this.primaryCulture = primaryCulture;
            this.techGroup = techGroup;
            this.governmentType = governmentType;
            this.religion = religion;
            this.provinces = provinces;
            if(provinces.Count >0)
            { existsInCurrentDate = true; }
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
        public void AddGuarentee(Country country)
        {
            guaranteeing.Add(country);
        }
        public void AddGuarenteedBy(Country country)
        {
            guaranteedBy.Add(country);
        }
        public void AddVassel(Country country)
        {
            vassals.Add(country);
        }
        public void AddAlliance(Country country)
        {
            alliances.Add(country);
        }
        public void AddUnion(Country country)
        {
            unions.Add(country);
        }
        public void AddRoyalMarriage(Country country)
        {
            royalMarriages.Add(country);
        }
        public void AddTradeLeagueMember(Country country)
        {
            tradeLeague.Add(country);
        }
        public void AddTributary(Country country)
        {
            royalMarriages.Add(country);
        }
        public void SetTradeLeagueLeader(Country country)
        {
            tradeLeagueLeader = country;
        }
        public void SetVassalOverlord(Country country)
        {
            vassalOverlord = country;
        }
        public void SetTributaryOverlord(Country country)
        {
            tributaryOverlord = country;
        }
        public void SetUnionOverlord(Country country)
        {
            unionOverlord = country;
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
        public void PrintName()
        {
            if (ClosestConsoleColor(color.R, color.G, color.B) != ConsoleColor.Black)
            {
                Console.ForegroundColor = ClosestConsoleColor(color.R, color.G, color.B);
            }
            else
            {
                Console.ForegroundColor = ClosestConsoleColor(color.R, color.G, color.B);
                Console.BackgroundColor = ConsoleColor.White;
            }
            Console.WriteLine(tag + " " + name);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public void PrintDevelopment()
        {
            if (existsInCurrentDate)
            {
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
        }
        public void PrintDiplomacy()
        {
            if (existsInCurrentDate)
            { 
            PrintName();
                if(isCelestialEmperor)
                {
                    Console.WriteLine("\tIs the Celestial Emperor!");
                }
                if (isHREEmperor)
                {
                    Console.WriteLine("\tIs the Emperor of the HRE!");
                }
                if (alliances.Count() > 0)
            {
                Console.WriteLine("\tAlliances:");
                foreach(Country country in alliances )
                {
                    Console.Write("\t\t");
                    country.PrintName();
                }
            }
            if (royalMarriages.Count() > 0)
            {
                Console.WriteLine("\tRoyal Marriages:");
                foreach (Country country in royalMarriages)
                {
                    Console.Write("\t\t");
                    country.PrintName();
                }
            }
            if (unions.Count() > 0|| unionOverlord != null)
            {
                Console.WriteLine("\tUnions:");
                foreach (Country country in unions)
                {
                   Console.Write("\t\tOverlord of: ");
                   country.PrintName();
                }
                if (unionOverlord != null)
                {
                    Console.Write("\t\tUnion under: ");
                    unionOverlord.PrintName();
                }

            }
            if (vassals.Count() > 0 || vassalOverlord != null)
            {
                Console.WriteLine("\tVassals:");
                foreach (Country country in vassals)
                {
                    Console.Write("\t\tOverlord of: ");
                    country.PrintName();
                }
                if (vassalOverlord != null)
                {
                    Console.Write("\t\tVassal under: ");
                    vassalOverlord.PrintName();
                }

            }
            if (tributaries.Count() > 0 || tributaryOverlord != null)
            {
                Console.WriteLine("\tTributaries:");
                foreach (Country country in tributaries)
                {
                    Console.Write("\t\tOverlord of: ");
                    country.PrintName();
                }
                if (tributaryOverlord != null)
                {
                    Console.Write("\t\tTributary under: ");
                    tributaryOverlord.PrintName();
                }

            }
                if (tradeLeague.Count() > 0)
                {
                    Console.WriteLine("\tTrade League:");
                    if (tradeLeagueLeader == this)
                    {
                        Console.WriteLine("\t\tTrade League leader of: ");
                        foreach (Country country in tradeLeague)
                        {
                            Console.Write("\t\t\t");
                            country.PrintName();

                        }

                    }
                    else
                    {
                        Console.Write("\t\tTrade League member under: ");
                        tradeLeagueLeader.PrintName();

                        foreach (Country country in tradeLeagueLeader.tradeLeague)
                        {
                            if (country != this)
                            {
                                Console.Write("\t\t\t");
                                country.PrintName();
                            }

                        }
                    }
                    
                }
                if (guaranteedBy.Count() > 0 || guaranteeing.Count() > 0)
                {
                    Console.WriteLine("\tGuarantees:");
                    foreach (Country country in guaranteeing)
                    {
                        Console.Write("\tGuaranteeing: ");
                        country.PrintName();
                    }
                    foreach (Country country in guaranteedBy)
                    {
                        Console.Write("\tGuaranteed By: ");
                        country.PrintName();
                    }

                }
            }

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
