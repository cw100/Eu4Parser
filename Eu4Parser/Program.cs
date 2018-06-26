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
            string path = @"E:\Steam\SteamApps\common\Europa Universalis IV\";

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
                for(int i = 0; i<lines.Count();i++)
                {
                     
                    string[] statement = lines[i].Split(' ');
                    switch (statement[0])
                    {
                        case "owner":
                            if(owner == "")
                            owner = statement[2].Substring(0, 3);
                           
                            break;
                        case "controller":
                            if (controller == "")
                                controller = statement[2].Substring(0, 3);
                            break;
                        case "culture":
                            if (culture == "")
                                culture = statement[2];
                            break;
                        case "religion":
                            if (religion == "")
                                religion = statement[2];
                            break;
                        case "capital":
                            if (capital == "")
                                for (int j = 2; j < statement.Length; j++)
                                {
                                    capital += Regex.Replace(statement[j], "\"", "") + " ";
                                }
                            capital.Trim();
                           
                            break;
                        case "trade_goods":
                            if (tradeGood == "")
                                tradeGood = statement[2];
                            break;
                        case "hre":
                            if (statement[2].Trim().Contains("yes"))
                                hre = true;
                            break;
                        case "base_tax":
                            try
                            {
                                if (tax == 0)
                                {
                                    tax = int.Parse(Regex.Replace(statement[2], "[^.0-9]", ""));
                                    }
                            }
                            catch
                            {

                            }
                            break;
                        case "base_production":
                            try
                            {
                                if (production == 0)
                                {
                                    production = int.Parse(Regex.Replace(statement[2], "[^.0-9]", ""));
                                }
                            }
                            catch
                            {

                            }
                            break;
                        case "base_manpower":
                            try
                            {
                                if (manpower ==0)
                                {
                                    manpower = int.Parse(Regex.Replace(statement[2], "[^.0-9]", ""));
                                }
                            }
                            catch
                            {

                            }
                            break;
                        case "is_city":
                            
                            if (statement[2].Trim().Contains("yes"))
                                city = true;
                            break;
                        case "	name":
                            if (statement[2].Trim().Contains("center_of_trade_modifier") )
                                centerOfTrade = true;
                            break;
                        case "fort_15th":
                            if (statement[2].Trim().Contains("yes"))
                                fort = true;
                            break;
                        default:
                            break;
                    }
                    try
                    {
                        string[] dateString = statement[0].Split('.');
                        if (dateString[0].StartsWith("1"))
                        {
                            DateTime date = new DateTime(int.Parse(dateString[0]), int.Parse(dateString[1]), int.Parse(dateString[2]));
                            DateTime startDate = new DateTime(1444, 11, 12);
                            
                            if (date <= startDate)
                            {
                                List<string> updateProvinceStatements = new List<string>();
                                foreach (String statement2 in lines[i].Split(' ', '{', '}'))
                                    updateProvinceStatements.Add(statement2.TrimStart());
                                for (int j = i+1; j <= lines.Count(); j++)
                                { 
                                    if (!(lines[j].Substring(0, 1) == "}") && !(lines[j].Substring(0,1) == "1"))
                                    {
                                       
                                            foreach (String statement2 in lines[j].Split(' ','{', '}'))
                                                updateProvinceStatements.Add(statement2.TrimStart());
                                        

                                    }
                                    else
                                    {
                                        j = lines.Count() + 1;
                                    }

                                }
                                for (int k = 0; k<=updateProvinceStatements.Count();k++)
                                {

                                    if (owner == "MNG")
                                    {
                                        var asd = 0;
                                    }
                                    switch (updateProvinceStatements[k].Trim())
                                    {
                                        case "owner":

                                            owner = updateProvinceStatements[k+2].Substring(0, 3);

                                            
                                            break;
                                      
                                        case "controller":

                                            controller = updateProvinceStatements[k + 2].Substring(0, 3);
                                            break;
                                        case "culture":

                                            culture = updateProvinceStatements[k + 2];
                                            break;
                                        case "religion":

                                            religion = updateProvinceStatements[k + 2];
                                            break;
                                        case "capital":

                                            for (int l = 2; l < statement.Length; l++)
                                            {
                                                capital += Regex.Replace(statement[k + l], "\"", "") + " ";
                                            }
                                            capital.Trim();
                                            break;
                                        case "trade_goods":

                                            tradeGood = updateProvinceStatements[k + 2];
                                            break;
                                        case "hre":
                                            if (updateProvinceStatements[k + 2].Contains("yes"))
                                                hre = true;
                                            break;
                                        case "base_tax":
                                            try
                                            {

                                                tax = int.Parse(Regex.Replace(updateProvinceStatements[k + 2], "[^.0-9]", ""));

                                            }
                                            catch
                                            {

                                            }
                                            break;
                                        case "base_production":
                                            try
                                            {

                                                production = int.Parse(Regex.Replace(updateProvinceStatements[k + 2], "[^.0-9]", ""));

                                            }
                                            catch
                                            {

                                            }
                                            break;
                                        case "base_manpower":
                                            try
                                            {

                                                manpower = int.Parse(Regex.Replace(updateProvinceStatements[k + 2], "[^.0-9]", ""));

                                            }
                                            catch
                                            {

                                            }
                                            break;
                                        case "is_city":
                                            if (updateProvinceStatements[k + 2].Contains("yes"))
                                                city = true;
                                            break;
                                        case "	name":
                                            if (updateProvinceStatements[k + 2].Contains( "center_of_trade_modifier"))
                                                centerOfTrade = true;
                                            break;
                                        case "fort_15th":
                                            if (updateProvinceStatements[k + 2].Contains("yes"))
                                                fort = true;
                                            break;
                                        default:
                                            break;

                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
               
                if (!city)
                {
                    owner = "";
                    controller = "";
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
           for(int i=0;i<filePaths.Count();i++)
            { 
                filePaths[i] = Regex.Replace(filePaths[i], @"\t", " ");
            }
            List<Country> countries = new List<Country>();
            foreach (string file in Directory.EnumerateFiles(path + @"\history\countries\", "*.txt"))
            {
                provinces = new List<Province>();
                string[] lines = File.ReadAllLines(file);
                tag = Path.GetFileName(file).Split(' ')[0];
                
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
                    if (province.owner == tag && province.city)
                    {
                        
                            provinces.Add(province);
                    }
                    

                }
                Country country = new Country(tag, primaryCulture, techGroup, governmentType, religion, provinces, captialId, mercantilism);
                if (tag != "")
                {
                    foreach (string line in filePaths)
                    {
                       
                        if (line.Split(' ')[0] == tag)
                        {
                        
                            string fileName = line.Split('"')[1];
                            country.SetName(fileName.Substring(10,fileName.Length-14));
                            string[] lines2 = File.ReadAllLines(path + @"\common\" + fileName);
                            foreach (string line2 in lines2)
                            {
                               
                               string lineHolder = Regex.Replace(line2, @"\t", " ");
                                while(lineHolder.Contains("  "))
                                 lineHolder = Regex.Replace(lineHolder, "  ", " ");

                                lineHolder = Regex.Replace(lineHolder, "{", "");
                                lineHolder = Regex.Replace(lineHolder, "}", "");
                                string[] splitLine = lineHolder.Split(' ');

                               
                                if (splitLine[0] == "color")
                                {

                                   
                                        country.SetColour(splitLine[3], splitLine[4], splitLine[5]);
                                    
                                   

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
