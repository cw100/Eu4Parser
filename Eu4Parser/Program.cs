using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Parser
{
    class Program
    {

        static void Main(string[] args)
        {
            List<Province> provinces = new List<Province>();
            provinces = LoadProvinces(@"E:\Steam\SteamApps\common\Europa Universalis IV\history\provinces");
            provinces = provinces.OrderBy(o => o.id).ToList();
            foreach (Province province in provinces)
            {
                province.Print();
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
            foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
            {
                string owner="";
                string controller="";
                string religion = "";
                string culture = "";
                string tradeGood = "";
                string capital = "";
                bool centerOfTrade = false;
                bool city = false;
                bool hre = false;
                int tax= 0;
                int production=0;
                int manpower=0;
                int id=0;
                bool coal = false;
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
                            owner = statement[2].Substring(0,3);
                            break;
                        case "controller ":
                            controller = statement[2].Substring(0, 3);
                            break;
                        case "culture":
                            culture = statement[2];
                            break;
                        case "religion":
                            religion = statement[2];
                            break;
                        case "capital":
                            capital = statement[2];
                            break;
                        case "trade_goods":
                            tradeGood = statement[2];
                            break;
                        case "hre":
                          if(statement[2]=="yes")
                                hre=true;
                            break;
                        case "base_tax":
                            try
                            {
                                tax = int.Parse(statement[2]);
                            }
                            catch
                            {

                            }
                            break;
                        case "base_production":
                            try
                            {
                                production = int.Parse(statement[2]);
                            }
                            catch
                            {

                            }
                            break;
                        case "base_manpower":
                            try
                            {
                                manpower = int.Parse(statement[2]);
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
                        default:
                            break;
                    }
                }
                Province province = new Province(id,owner,controller,capital,tradeGood,religion,culture,tax,production,manpower,centerOfTrade,city,hre,coal);
                provinces.Add(province);
            }
            return provinces;
        }
    }
}
