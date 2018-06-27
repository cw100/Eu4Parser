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
            DateTime startDate = new DateTime(1444, 11, 12);
            string path = @"E:\Steam\SteamApps\common\Europa Universalis IV\";

            List<Province> provinces = new List<Province>();
            provinces = LoadProvinces(path, startDate);
            provinces = provinces.OrderBy(o => o.id).ToList();




            List<Country> countries = new List<Country>();

            countries = LoadCountries(path, provinces,startDate);
            //foreach (Country country in countries)
            //{
            //    country.Print();
            //}
            //foreach (Province province in provinces)
            //{
            //    province.Print();
            //}
            LoadDiplomacy(path, countries, startDate);
           
           
            foreach (Province province in provinces)
            {
                province.ResolveCores(countries);
            }
            foreach (Country country in countries)
            {
                country.ResolveCores(provinces);
            }
            foreach (Country country in countries)
            {
                country.PrintMonarchs();
            }
            Console.Read();
        }
        public static string LoadFile(string path)
        {
            string result;
            result = System.IO.File.ReadAllText(path);
            return result;
        }

        public static List<Province> LoadProvinces(string path,DateTime startDate)
        {
            List<Province> provinces = new List<Province>();
            foreach (string file in Directory.EnumerateFiles(path + @"\history\provinces\", "*.txt"))
            {
                List<string> cores = new List<string>();
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
                int fortLevel = 0;
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
                for (int i = 0; i < lines.Count(); i++)
                {

                    lines[i] = Regex.Replace(lines[i], @"\t", " ");
                    while (lines[i].Contains("  "))
                        lines[i] = Regex.Replace(lines[i], "  ", " ");
                    string[] statement = lines[i].Split(' ');

                    switch (statement[0])
                    {
                        case "owner":
                            if (owner == "")
                                owner = statement[2].Substring(0, 3);

                            break;
                        case "controller":
                            if (controller == "")
                                controller = statement[2].Substring(0, 3);
                            break;
                        case "culture":
                            if (culture == "")
                                culture = statement[2].Trim();
                            break;
                        case "religion":
                            if (religion == "")
                                religion = Regex.Replace(statement[2], "\"", "").Trim(); //Regex required for one province that has the religion in quotes for some insane reason
                            break;
                        case "capital":
                            if (capital == "")
                                for (int b = 2; b < statement.Length; b++)
                                {

                                    if (statement[b].Contains("#"))
                                    {
                                        break;
                                    }
                                    capital += Regex.Replace(statement[b], "\"", "") + " ";
                                }
                            capital = capital.Trim();

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
                                if (manpower == 0)
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
                            if (statement[2].Trim().Contains("center_of_trade_modifier"))
                                centerOfTrade = true;
                            break;
                        case "fort_15th":
                            if (statement[2].Trim().Contains("yes"))
                            {
                                fort = true;
                                fortLevel = 1;
                            }
                            if (statement[2].Trim().Contains("no"))
                            {
                                fort = false;
                                fortLevel = 0;
                            }
                            break;
                        case "fort_16th":
                            if (statement[2].Trim().Contains("yes"))
                            {
                                fort = true;
                                fortLevel = 2;
                            }
                            if (statement[2].Trim().Contains("no"))
                            {
                                fort = false;
                                fortLevel = 0;
                            }
                            break;
                        case "fort_17th":
                            if (statement[2].Trim().Contains("yes"))
                            {
                                fort = true;
                                fortLevel = 3;
                            }
                            if (statement[2].Trim().Contains("no"))
                            {
                                fort = false;
                                fortLevel = 0;
                            }
                            break;
                        case "fort_18th":
                            if (statement[2].Trim().Contains("yes"))
                            {
                                fort = true;
                                fortLevel = 4;
                            }
                            if (statement[2].Trim().Contains("no"))
                            {
                                fort = false;
                                fortLevel = 0;
                            }
                            break;
                        case "add_core":
                            cores.Add(statement[2]);
                            break;
                        case "remove_core":
                            cores.Remove(statement[2]);
                            break;
                        default:
                            break;
                    }
                    List<string> updateProvinceStatements = new List<string>();
                    string[] dateString = statement[0].Split('.');
                    if (dateString[0].StartsWith("1"))
                    {
                        
                        DateTime date = startDate.AddDays(1);
                        try
                        {

                            date = new DateTime(int.Parse(Regex.Replace(dateString[0], "[^.0-9]", ""))
                                , int.Parse(Regex.Replace(dateString[1], "[^.0-9]", "")),
                                int.Parse(Regex.Replace(dateString[2], "[^.0-9]", "")));
                        }
                        catch
                        {

                        }


                        if (date <= startDate)
                        {

                            foreach (String statement2 in lines[i].Split(' ', '{', '}'))
                            {
                                if (statement2 != "" && statement2 != " ")
                                {
                                    updateProvinceStatements.Add(statement2.TrimStart());
                                }
                            }
                            for (int j = i + 1; j < lines.Count(); j++)
                            {


                                if (!lines[j].StartsWith("}") && !lines[j].StartsWith("1"))
                                {

                                    foreach (String statement2 in lines[j].Split(' ', '{', '}'))
                                    {
                                        if (statement2 != "" && statement2 != " ")
                                        {
                                            updateProvinceStatements.Add(statement2.TrimStart());
                                        }
                                    }

                                }
                                else
                                {
                                    break;
                                }

                            }
                            for (int k = 0; k < updateProvinceStatements.Count(); k++)
                            {

                                switch (updateProvinceStatements[k].Trim())
                                {
                                    case "owner":

                                        owner = updateProvinceStatements[k + 2].Substring(0, 3);


                                        break;

                                    case "controller":

                                        controller = updateProvinceStatements[k + 2].Substring(0, 3);
                                        break;
                                    case "culture":

                                        culture = updateProvinceStatements[k + 2].Trim();
                                        break;
                                    case "religion":
                                        try
                                        {
                                            religion = Regex.Replace(statement[k + 2], "\"", "").Trim();

                                        }
                                        catch

                                        {

                                        }
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
                                    case "capital":
                                        if (capital == "")
                                            for (int b = 2; b < statement.Length; b++)
                                            {
                                                if (statement[b].Contains("#"))
                                                {
                                                    break;
                                                }
                                                capital += Regex.Replace(statement[b], "\"", "") + " ";
                                            }
                                        capital = capital.Trim();

                                        break;
                                    case "is_city":
                                        if (updateProvinceStatements[k + 2].Contains("yes"))
                                            city = true;
                                        if (updateProvinceStatements[k + 2].Contains("no"))
                                            city = false;
                                        break;
                                    case "	name":
                                        if (updateProvinceStatements[k + 2].Contains("center_of_trade_modifier"))
                                            centerOfTrade = true;
                                        break;
                                    case "fort_15th":
                                        if (updateProvinceStatements[k + 2].Contains("yes"))
                                        {
                                            fort = true;
                                            fortLevel = 1;
                                        }
                                        if (updateProvinceStatements[k + 2].Contains("no"))
                                        {
                                            fort = false;
                                            fortLevel = 0;
                                        }
                                        break;
                                    case "fort_16th":
                                        if (updateProvinceStatements[k + 2].Contains("yes"))
                                        {
                                            fort = true;
                                            fortLevel = 2;
                                        }
                                        if (updateProvinceStatements[k + 2].Contains("no"))
                                        {
                                            fort = false;
                                            fortLevel = 0;
                                        }
                                        break;
                                    case "fort_17th":
                                        if (updateProvinceStatements[k + 2].Contains("yes"))
                                        {
                                            fort = true;
                                            fortLevel = 3;
                                        }
                                        if (updateProvinceStatements[k + 2].Contains("no"))
                                        {
                                            fort = false;
                                            fortLevel = 0;
                                        }
                                        break;
                                    case "fort_18th":
                                        if (updateProvinceStatements[k + 2].Contains("yes"))
                                        {
                                            fort = true;
                                            fortLevel = 4;
                                        }
                                        if (updateProvinceStatements[k + 2].Contains("no"))
                                        {
                                            fort = false;
                                            fortLevel = 0;
                                        }
                                        break;
                                    case "add_core":
                                        cores.Add(updateProvinceStatements[k + 2]);
                                        break;
                                    case "remove_core":
                                        cores.Remove(updateProvinceStatements[k + 2]);
                                        break;
                                    default:
                                        break;

                                }
                            }
                        }
                    }
                }






                Province province = new Province(id, owner, controller, capital, tradeGood, religion, culture, tax, production, manpower, centerOfTrade, city, hre, coal, fort,fortLevel,cores);
                provinces.Add(province);
            }
            return provinces;
        }


        public static List<Country> LoadCountries(string path, List<Province> allProvinces, DateTime date)
        {
            List<Province> provinces = new List<Province>(); ;
            string tag = "";
            
            string primaryCulture = "";
            string techGroup = "";
            string governmentType = "";
            string religion = "";
            int captialId = 0;
           int mercantilism = 0;
            Monarch monarch;
            Monarch heir;
            string monarchName = "";
            string heirName = "";
            string monarchMonarchName = "";
            string heirMonarchName = "";
            string monarchDynasty = "";
            string heirDynasty="";
            int monarchAdm = 0;
            int monarchDip = 0;
            int monarchMil =0;
            int heirAdm = 0;
            int heirDip = 0;
            int heirMil =0;
            bool monarchToggle=false;
            bool leaderToggle = false;

            bool monarchFemale = false;
            bool monarchRegent = false;
            bool heirFemale = false;
            bool heirRegent = false;
            string[] filePaths = File.ReadAllLines(path + @"\common\country_tags\00_countries.txt");
            for (int i = 0; i < filePaths.Count(); i++)
            {
                filePaths[i] = Regex.Replace(filePaths[i], @"\t", " ");
            }
            List<Country> countries = new List<Country>();
            foreach (string file in Directory.EnumerateFiles(path + @"\history\countries\", "*.txt"))
            {
                DateTime lastDate = new DateTime(1,1,1);
                 primaryCulture = "";
                 techGroup = "";
                 governmentType = "";
                 religion = "";
                 mercantilism = 0;
                provinces = new List<Province>();
                string[] lines = File.ReadAllLines(file);
                tag = Path.GetFileName(file).Split(' ')[0];
                monarchName = "";
                heirName = "";
                monarchMonarchName = "";
                heirMonarchName = "";
                monarchDynasty = "";
                heirDynasty = "";
                monarchAdm = 0;
                monarchDip = 0;
                monarchMil = 0;
                heirAdm = 0;
                heirDip = 0;
                heirMil = 0;
                monarchToggle = false;
                leaderToggle = false;

                monarchFemale = false;
                monarchRegent = false;
                heirFemale = false;
                heirRegent = false;
                for (int i = 0; i < lines.Length; i++)
                {

                     
                    string[] statement = lines[i].Split(' ');
                    if (!statement[0].StartsWith("1"))
                        {
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
                    else
                    {
                        string[] dateString = statement[0].Split('.');
                        
                       
                      
                            DateTime thisDate = new DateTime(int.Parse(Regex.Replace(dateString[0], "[^.0-9]", ""))
                                , int.Parse(Regex.Replace(dateString[1], "[^.0-9]", "")),
                                int.Parse(Regex.Replace(dateString[2], "[^.0-9]", "")));
                        if (lastDate == new DateTime(1, 1, 1))
                        {
                            lastDate = thisDate;

                        }
                       
                        if (thisDate <= date)
                            {
                                List<string> endStatement = GetStatement(lines, i);
                            
                            if (lastDate != thisDate && endStatement[1]!= "leader=")
                            {
                                monarchName = "";
                                heirName = "";
                                monarchMonarchName = "";
                                heirMonarchName = "";
                                monarchDynasty = "";
                                heirDynasty = "";
                                monarchAdm = 0;
                                monarchDip = 0;
                                monarchMil = 0;
                                heirAdm = 0;
                                heirDip = 0;
                                heirMil = 0;
                                monarchToggle = false;
                                leaderToggle = false;

                                monarchFemale = false;
                                monarchRegent = false;
                                heirFemale = false;
                                heirRegent = false;
                                lastDate = thisDate;
                            }
                            for (int j = 0; j < endStatement.Count; j++)
                                {

                                    switch (endStatement[j])
                                    {
                                        case "monarch=":
                                            monarchToggle = true;
                                            leaderToggle = false;
                                            break;
                                        case "heir=":
                                            monarchToggle = false;
                                            leaderToggle = false;
                                            break;
                                        case "leader=":
                                            leaderToggle = true;
                                                break;
                                        case "name=":
                                            if(monarchToggle&&!leaderToggle)
                                            {
                                                monarchName = "";
                                                for (int k = j + 1; k < endStatement.Count; k++)
                                                {
                                                if (!endStatement[k].Contains("#") && !endStatement[k].Contains("\r") && !endStatement[k].Contains("birth_date=") && !endStatement[k].Contains("adm="))
                                                {
                                                    monarchName += Regex.Replace(endStatement[k], "\"", "") + " ";
                                                }
                                                if (endStatement[k].EndsWith("\"") || endStatement[k].Contains("#") || endStatement[k].Contains("\r") || endStatement[k].Contains("birth_date=") || endStatement[k].Contains("adm="))
                                                {
                                                    monarchName = monarchName.Trim();
                                                        break;
                                                    }

                                                }
                                               
                                            }
                                            if (!monarchToggle && !leaderToggle)
                                            {
                                               
                                            heirName = "";
                                            for (int k = j + 1; k < endStatement.Count; k++)
                                            {
                                                if (!endStatement[k].Contains("#") && !endStatement[k].Contains("\r") && !endStatement[k].Contains("birth_date=") && !endStatement[k].Contains("adm="))
                                                {
                                                    heirName += Regex.Replace(endStatement[k], "\"", "") + " ";
                                                }
                                                if (endStatement[k].EndsWith("\"") || endStatement[k].Contains("#") || endStatement[k].Contains("\r") || endStatement[k].Contains("birth_date=") || endStatement[k].Contains("adm="))
                                                {
                                                    heirName = heirName.Trim();
                                                    break;
                                                }

                                            }
                                        }
                                            break;
                                        case "monarch_name=":
                                            if (monarchToggle && !leaderToggle)
                                            {
                                                monarchMonarchName = "";
                                                for (int k = j +1; k < endStatement.Count; k++)
                                                {

                                                    monarchMonarchName += Regex.Replace(endStatement[k], "\"", "") + " ";
                                                    if (endStatement[k].EndsWith("\""))
                                                    {
                                                        monarchMonarchName = monarchMonarchName.Trim();
                                                        break;
                                                    }

                                                }
                                            }
                                            if (!monarchToggle && !leaderToggle)
                                            {
                                                heirMonarchName = "";
                                                for (int k = j +1; k < endStatement.Count; k++)
                                                {

                                                    heirMonarchName += Regex.Replace(endStatement[k], "\"", "") + " ";
                                                    if (endStatement[k].EndsWith("\""))
                                                    {
                                                        heirMonarchName = heirMonarchName.Trim();
                                                        break;
                                                    }

                                                }
                                            }
                                            break;
                                        case "dynasty=":
                                            if (monarchToggle && !leaderToggle)
                                            {
                                               
                                            monarchDynasty = "";
                                            for (int k = j + 1; k < endStatement.Count; k++)
                                            {
                                                if (!endStatement[k].Contains("#") && !endStatement[k].Contains("\r") && !endStatement[k].Contains("birth_date=") && !endStatement[k].Contains("adm="))
                                                {
                                                    monarchDynasty += Regex.Replace(endStatement[k], "\"", "") + " ";
                                                }
                                                if (endStatement[k].EndsWith("\"") || endStatement[k].Contains("#") || endStatement[k].Contains("\r") || endStatement[k].Contains("birth_date=") || endStatement[k].Contains("adm="))
                                                {
                                                    monarchDynasty = monarchDynasty.Trim();
                                                    break;
                                                }

                                            }
                                        }
                                            if (!monarchToggle && !leaderToggle)
                                            {
                                                
                                            heirDynasty = "";
                                            for (int k = j + 1; k < endStatement.Count; k++)
                                            {
                                                if (!endStatement[k].Contains("#") && !endStatement[k].Contains("\r") && !endStatement[k].Contains("birth_date=") && !endStatement[k].Contains("adm="))
                                                {
                                                    heirDynasty += Regex.Replace(endStatement[k], "\"", "") + " ";
                                                }
                                                if (endStatement[k].EndsWith("\"") || endStatement[k].Contains("#") || endStatement[k].Contains("\r") || endStatement[k].Contains("birth_date=") || endStatement[k].Contains("adm="))
                                                {
                                                    heirDynasty = heirDynasty.Trim();
                                                    break;
                                                }

                                            }
                                        }
                                            break;
                                        case "adm=":
                                            if (monarchToggle && !leaderToggle)
                                            {
                                                monarchAdm = int.Parse(endStatement[j + 1]);
                                            }
                                            if (!monarchToggle && !leaderToggle)
                                            {
                                                heirAdm = int.Parse(endStatement[j + 1]);
                                            }
                                            break;
                                        case "dip=":
                                            if (monarchToggle && !leaderToggle)
                                            {
                                                monarchDip = int.Parse(endStatement[j + 1]);
                                            }
                                            if (!monarchToggle && !leaderToggle)
                                            {
                                                heirDip = int.Parse(endStatement[j + 1]);
                                            }
                                            break;
                                        case "mil=":
                                            if (monarchToggle && !leaderToggle)
                                            {
                                                monarchMil = int.Parse(endStatement[j + 1]);
                                            }
                                            if (!monarchToggle && !leaderToggle)
                                            {
                                                heirMil = int.Parse(endStatement[j + 1]);
                                            }
                                            break;
                                        case "regent=":
                                            if(endStatement[j + 1].Contains("yes"))
                                                {
                                                if (monarchToggle && !leaderToggle)
                                                {
                                                    monarchRegent = true;
                                                }
                                                if (!monarchToggle && !leaderToggle)
                                                {
                                                    heirRegent = true;
                                                }
                                            }
                                            if (endStatement[j + 1].Contains("no"))
                                            {
                                                if (monarchToggle && !leaderToggle)
                                                {
                                                    monarchRegent = true;
                                                }
                                                if (!monarchToggle && !leaderToggle)
                                                {
                                                    heirRegent = true;
                                                }
                                            }
                                            break;
                                        case "female=":
                                            if (endStatement[j + 1].Contains("yes"))
                                            {
                                                if (monarchToggle && !leaderToggle)
                                                {
                                                    monarchFemale = true;
                                                }
                                                if (!monarchToggle && !leaderToggle)
                                                {
                                                    heirFemale = true;
                                                }
                                            }
                                            if (endStatement[j + 1].Contains("no"))
                                            {
                                                if (monarchToggle && !leaderToggle)
                                                {
                                                   monarchFemale = false;
                                                }
                                                if (!monarchToggle && !leaderToggle)
                                                {
                                                    heirFemale = false;
                                                }
                                            }
                                            break;
                                        case "government=":
                                            governmentType = endStatement[j +1];
                                            break;
                                        default:
                                            break;

                                    }
                                }
                            }
                       
                    }
                }

                foreach (Province province in allProvinces)
                {
                    if (province.owner == tag && province.city)
                    {

                        provinces.Add(province);
                    }


                }
                monarch = new Monarch(monarchName, monarchDynasty,monarchMonarchName, monarchAdm, monarchDip, monarchMil,monarchFemale, monarchRegent);
                heir = new Monarch(heirName, heirDynasty,heirMonarchName, heirAdm, heirDip, heirMil, heirFemale, heirRegent);
                Country country = new Country(tag, primaryCulture, techGroup, governmentType, religion, provinces, captialId, mercantilism, monarch, heir);
                if (tag != "")
                {
                    foreach (string line in filePaths)
                    {

                        if (line.Split(' ')[0] == tag)
                        {

                            string fileName = line.Split('"')[1];
                            country.SetName(fileName.Substring(10, fileName.Length - 14));
                            string[] lines2 = File.ReadAllLines(path + @"\common\" + fileName);
                            foreach (string line2 in lines2)
                            {

                                string lineHolder = Regex.Replace(line2, @"\t", " ");
                                while (lineHolder.Contains("  "))
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

        public static List<Country> LoadDiplomacy(string path, List<Country> countries, DateTime date)
        {
            Country hreEmp=null;
            Country celestialEmp=null;
            foreach (string file in Directory.EnumerateFiles(path + @"\history\diplomacy", "*.txt"))
            {

                string[] lines = File.ReadAllLines(file);


                for (int i = 0; i < lines.Length; i++)
                {
                    string lineHolder = Regex.Replace(lines[i], @"\t", " ");
                    while (lineHolder.Contains("  "))
                        lineHolder = Regex.Replace(lineHolder, "  ", " ");

                    string[] statement = lineHolder.Split(' ');
                    List<string> endStatement = new List<string>();
                    string first = "";
                    string second = "";
                    bool tradeLeague = false;

                    bool tributary = false;
                    DateTime startDate = new DateTime(1,1,1) ;
                    DateTime endDate  = new DateTime(1, 1, 1);
                    if (statement[0] == "alliance"||
                        statement[0] == "royal_marriage"||
                        statement[0] == "vassal"||
                        statement[0] == "union"||
                        statement[0] == "dependency"||
                        statement[0] == "guarantee"||
                        statement[0] == "march")
                    {

                        endStatement = GetStatement(lines, i);
                        for (int j = 0; j < endStatement.Count(); j++)
                        {
                            switch (endStatement[j])
                            {
                                case "first=":
                                    first = endStatement[j + 1];
                                    break;
                                case "second=":
                                    second = endStatement[j + 1];
                                    break;
                                case "start_date=":
                                    string[] dateString = endStatement[j + 1].Split('.');
                                    if (dateString[0].StartsWith("1"))
                                    {
                                        try
                                        {
                                            startDate = new DateTime(int.Parse(Regex.Replace(dateString[0], "[^.0-9]", ""))
                                                , int.Parse(Regex.Replace(dateString[1], "[^.0-9]", "")),
                                                int.Parse(Regex.Replace(dateString[2], "[^.0-9]", "")));
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    break;
                                case "end_date=":
                                     dateString = endStatement[j + 1].Split('.');
                                    if (dateString[0].StartsWith("1"))
                                    {


                                        try
                                        {

                                            endDate = new DateTime(int.Parse(Regex.Replace(dateString[0], "[^.0-9]", ""))
                                                , int.Parse(Regex.Replace(dateString[1], "[^.0-9]", "")),
                                                int.Parse(Regex.Replace(dateString[2], "[^.0-9]", "")));
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    break;
                                case "trade_league=":
                                    tradeLeague = true;
                                    break;
                                case "subject_type=":
                                    if(endStatement[j+1].Contains("tributary_state"))
                                    tributary = true;
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                    else
                    {
                        if(statement[0].StartsWith("1"))
                        {
                            string[] dateString = statement[0].Split('.');
                            
                                try
                                {
                                    startDate = new DateTime(int.Parse(Regex.Replace(dateString[0], "[^.0-9]", ""))
                                        , int.Parse(Regex.Replace(dateString[1], "[^.0-9]", "")),
                                        int.Parse(Regex.Replace(dateString[2], "[^.0-9]", "")));
                                if (startDate <= date)
                                    {
                                    endStatement = GetStatement(lines, i);
                                    for (int j = 0; j < endStatement.Count; j++)
                                    {
                                       
                                        switch (endStatement[j])
                                        {
                                            case "celestial_emperor=":
                                               
                                                if (celestialEmp == null)
                                                {
                                                    foreach (Country country in countries)
                                                    {
                                                        if(country.tag == endStatement[j+1])
                                                        {
                                                            country.isCelestialEmperor = true;
                                                            celestialEmp = country;
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    foreach (Country country in countries)
                                                    {
                                                        if (country.tag == endStatement[j + 1])
                                                        {
                                                            country.isCelestialEmperor = true;
                                                            celestialEmp.isCelestialEmperor = false;
                                                            celestialEmp = country;
                                                        }
                                                    }
                                                }
                                                break;
                                            case "emperor=":

                                                if (hreEmp == null)
                                                {
                                                    foreach (Country country in countries)
                                                    {
                                                        if (country.tag == endStatement[j +1])
                                                        {
                                                            country.isHREEmperor = true;
                                                            hreEmp = country;
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    foreach (Country country in countries)
                                                    {
                                                        if (country.tag == endStatement[j +1])
                                                        {
                                                            country.isHREEmperor = true;
                                                            hreEmp.isHREEmperor = false;
                                                            hreEmp = country;
                                                        }
                                                        if (endStatement[j + 2] == "---")
                                                        {
                                                            hreEmp.isHREEmperor = false;
                                                        }
                                                    }
                                                }
                                                break;
                                            default:
                                                break;

                                        }
                                    }
                                }
                                }
                                catch
                                {

                                }
                            
                        }
                    }
                    if (startDate <= date && date <= endDate)
                    {
                        Country firstCountry = null;
                        Country secondCountry = null;
                        for (int j = 0; j < countries.Count(); j++)
                        {
                            if (countries[j].tag == first)
                            {
                                firstCountry = countries[j];
                            }
                            if (countries[j].tag == second)
                            {
                                secondCountry = countries[j];
                            }
                        }
                        if (firstCountry != null && secondCountry != null)
                        { 
                        switch (statement[0])
                        {
                            case "alliance":
                                    if (!tradeLeague)
                                    {
                                     firstCountry.AddAlliance(secondCountry);
                                        secondCountry.AddAlliance(firstCountry);
                                    }
                                    else

                                    {
                                        firstCountry.AddTradeLeagueMember(secondCountry);
                                        firstCountry.isTradeLeader = true;
                                        firstCountry.tradeLeagueLeader = firstCountry;
                                        secondCountry.AddTradeLeagueMember(firstCountry);
                                        secondCountry.tradeLeagueLeader = firstCountry;
                                    }
                                    
                                break;

                                case "royal_marriage":
                                  
                                        firstCountry.AddRoyalMarriage(secondCountry);
                                        secondCountry.AddRoyalMarriage(firstCountry);
                                  
                                    break;
                                case "vassal":

                                    firstCountry.AddVassel(secondCountry);
                                    secondCountry.SetVassalOverlord(firstCountry);

                                    break;
                                case "march":

                                    firstCountry.AddMarch(secondCountry);
                                    secondCountry.SetMarchOverlord(firstCountry);

                                    break;
                                case "union":

                                    firstCountry.AddUnion(secondCountry);
                                   
                                    secondCountry.SetUnionOverlord(firstCountry);

                                    break;
                                case "dependency":
                                    if(tributary)
                                    {
                                        firstCountry.AddTributary(secondCountry);
                                        secondCountry.SetTributaryOverlord(firstCountry);
                                    }
                                    break;
                                case "guarantee":
                                    
                                        firstCountry.AddGuarentee(secondCountry);
                                        secondCountry.AddGuarenteedBy(firstCountry);
                                    
                                    break;

                                default:
                                break;
                        }
                    }
                    }
                }

            }

            return countries;

        }

        public static List<string> GetStatement(string[] lines, int startIndex)
        {
            List<string> endStatement = new List<string>();

            for (int i = startIndex; i < lines.Length; i++)
            {
                string lineHolder = Regex.Replace(lines[i], @"\t", " ");
                lineHolder = Regex.Replace(lineHolder, " =", " = ");
                lineHolder = Regex.Replace(lineHolder, " = ", "= ");

                while (lineHolder.Contains("  "))
                    lineHolder = Regex.Replace(lineHolder, "  ", " ");
                string[] line = lineHolder.Split(' ');
                foreach (string s in line)
                {
                    if (s != "}")
                    {
                        if(s != "{" && s != "")
                        endStatement.Add(s);
                    }
                    else

                    {
                        return endStatement;
                    }
                }

            }

            return endStatement;
        }
    }
}

