using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Eu4Parser
{
    class Program
    {

        static void Main(string[] args)
        {
            string path = @"E:\Steam\SteamApps\common\Europa Universalis IV";

            List<Province> provinces = new List<Province>();
            provinces = LoadProvinces(path);
            provinces = provinces.OrderBy(o => o.id).ToList();

            //foreach (Province province in provinces)
            //{
            //    province.Print();
            //}


            List<Country> countries = new List<Country>();

            countries = LoadCountries(path, provinces);
            foreach (Country country in countries)
            {
                country.Print();
            }
            Console.Read();
        }
        public static string LoadFile(string path)
        {
            string result;
            result = System.IO.File.ReadAllText(path);
            return result;
        }

        public static List<Province> LoadProvinces(string path)
        {
            List<Province> provinces = new List<Province>();
            foreach (string file in Directory.EnumerateFiles(path + @"\history\provinces\", "*.txt"))
            {
                string owner = "";
                string controller = "";
                string religion = "";
                string culture = "";
                string tradeGood = "";
                string capital = "";
                bool centerOfTrade = false;
                bool city = false;
                bool hre = false;
                int tax = 0;
                int production = 0;
                int manpower = 0;
                int id = 0;
                bool coal = false;
                bool fort = false;
                try
                {
                    id = int.Parse(Path.GetFileName(file).Split(' ')[0]);

                }
                catch
                {
                    try
                    {
                        id = int.Parse(Path.GetFileName(file).Split('-')[0]);

                    }
                    catch
                    {
                    }
                }
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    string[] statement = line.Split(' ');
                    switch (statement[0])
                    {
                        case "owner":
                            owner = statement[2].Substring(0, 3);
                            break;
                        case "controller":
                            controller = statement[2].Substring(0, 3);
                            break;
                        case "culture":
                            culture = statement[2];
                            break;
                        case "religion":
                            religion = statement[2];
                            break;
                        case "capital":
                            capital = Regex.Replace(statement[2], "\"", "");
                            break;
                        case "trade_goods":
                            tradeGood = statement[2];
                            break;
                        case "hre":
                            if (statement[2] == "yes")
                                hre = true;
                            break;
                        case "base_tax":
                            try
                            {
                                tax = int.Parse(Regex.Replace(statement[2], "[^.0-9]", ""));
                            }
                            catch
                            {

                            }
                            break;
                        case "base_production":
                            try
                            {
                                production = int.Parse(Regex.Replace(statement[2], "[^.0-9]", ""));
                            }
                            catch
                            {

                            }
                            break;
                        case "base_manpower":
                            try
                            {
                                manpower = int.Parse(Regex.Replace(statement[2], "[^.0-9]", ""));
                            }
                            catch
                            {

                            }
                            break;
                        case "is_city":
                            if (statement[2] == "yes")
                                city = true;
                            break;
                        case "	name":
                            if (statement[2] == "center_of_trade_modifier")
                                centerOfTrade = true;
                            break;
                        case "fort_15th":
                            if (statement[2] == "yes")
                                fort = true;
                            break;
                        default:
                            break;
                    }
                }
                Province province = new Province(id, owner, controller, capital, tradeGood, religion, culture, tax, production, manpower, centerOfTrade, city, hre, coal, fort);
                provinces.Add(province);
            }
            return provinces;
        }

        public static List<Country> LoadCountries(string path, List<Province> allProvinces)
        {
            List<Province> provinces = new List<Province>(); ;
            string tag = "";
            string name = "";
            string primaryCulture = "";
            string techGroup = "";
            string governmentType = "";
            string religion = "";
            int captialId = 0;
            int mercantilism = 0;
            string[] filePaths = File.ReadAllLines(path + @"\common\country_tags\00_countries.txt");
            List<Country> countries = new List<Country>();
            foreach (string file in Directory.EnumerateFiles(path + @"\history\countries\", "*.txt"))
            {
                provinces = new List<Province>();
                string[] lines = File.ReadAllLines(file);
                tag = Path.GetFileName(file).Split(' ')[0];
                name = Path.GetFileName(file).Split(' ')[2].Split('.')[0];
                if(name == "-")
                {
                    name = Path.GetFileName(file).Split(' ')[3].Split('.')[0];
                }
                foreach (string line in lines)
                {
                    string[] statement = line.Split(' ');
                    switch (statement[0])
                    {
                        case "primary_culture":
                            primaryCulture = statement[2];
                            break;
                        case "government":
                            governmentType = statement[2];
                            break;
                        case "mercantilism":
                            try
                            {
                                mercantilism = int.Parse(Regex.Replace(statement[2], "[^.0-9]", ""));
                            }
                            catch
                            {

                            }
                            break;
                        case "technology_group":
                            techGroup = statement[2];
                            break;
                        case "religion":
                            religion = statement[2];
                            break;

                        case "capital":
                            try
                            {
                                
                                captialId = int.Parse(Regex.Replace(statement[2], "[^.0-9]", ""));
                            }
                            catch
                            {
                            }
                            break;

                        default:
                            break;
                    }
                }

                foreach (Province province in allProvinces)
                {
                    if (province.owner == tag)
                    {
                        provinces.Add(province);
                    }
                }
                Country country = new Country(tag, name, primaryCulture, techGroup, governmentType, religion, provinces, captialId, mercantilism);
                if (name != "")
                {
                    foreach (string line in filePaths)
                    {
                        if (line.Split(' ')[0] == tag)
                        {
                            
                            string fileName = line.Split('"')[1];
                            string[] lines2 = File.ReadAllLines(path + @"\common\" + fileName);
                            foreach (string line2 in lines2)
                            {
                                if (line2.Split(' ')[0] == "color")
                                {
                                    country.SetColour(line2.Split(' ')[3], line2.Split(' ')[4], line2.Split(' ')[5]);

                                }
                            }
                        }
                    }
                }
                countries.Add(country);
            }
            return countries;

        }
     
    }
}
