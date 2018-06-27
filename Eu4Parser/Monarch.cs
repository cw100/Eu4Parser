using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Parser
{
    class Monarch
    {
        public string name,dynasty, monarchName;
        
        public int dip,mil,adm;
        public bool female, regent;
        public bool isLeader;
        public Monarch(string name, string dynasty, string monarchName,  int adm, int dip,int mil,bool female, bool regent, bool isLeader)
        {
            this.name = name;
            this.dynasty = dynasty;
            this.monarchName = monarchName;
            this.dip = dip;
            this.mil = mil;
            this.adm = adm;
            this.female = female;
            this.regent = regent;
            this.isLeader = isLeader;
        }
        public void Print()
        {
            if (name != "")
            {

                if (regent)
                {
                    Console.Write("Regency: ");
                }
                if (monarchName == "")
                {
                    Console.WriteLine(name + " " + dynasty + "(" + adm + ", " + dip + ", " + mil + ")");
                }
                else

                {
                    Console.WriteLine(monarchName + " " + dynasty + "(" + adm + ", " + dip + ", " + mil + ")");
                }
            }
            else
            {
                Console.WriteLine();
            }
            

        }
        
    }
}
