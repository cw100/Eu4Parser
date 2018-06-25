using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Parser
{
    public struct Colour
    {
        int r;
        int g;
        int b;
         public Colour(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }

            
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
        public Colour color;
        public int mercantilism;
        public bool existsIn1444;
        public bool inHRE;



        public Country(string tag, string name, string primaryCulture, string techGroup, string governmentType, 
            string religion, List<Province> provinces, int captialId, int mercantilism)
        {
            this.tag = tag;
            this.name = name;
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
            CalculateDevelopment();
            FindCapitalName();
            FindHREStatus(); 
        }
        public void SetColour(string r, string g, string b)
        {
            try
            {
                color = new Colour(int.Parse(r), int.Parse(g), int.Parse(b));
            }
            catch
            {

            }
        }
        void CalculateDevelopment()
        {
            foreach(Province province in provinces)
            {
                development += province.development;
                totalTax += province.tax;
                totalProduction += province.production;
                totalManpower += province.manpower;
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
            //Console.WriteLine(tag + " " + name + " " + development + " (" + totalTax + ", " + totalProduction + ", " + totalManpower + ") " + primaryCulture + " " + religion + " " + capitalName + " " + techGroup + " " + governmentType + " " + " Exists in 1444 " + existsIn1444 + " In hre " + inHRE);
            if(existsIn1444)
            Console.WriteLine(tag + " " + name + " " + development + " (" + totalTax + ", " + totalProduction + ", " + totalManpower + ")");
        }
    }
}
