using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Parser
{
    class Leader
    {
        public string name, type, monarchType;
        public int fire, shock, manuever, seige;
       
        public Leader(string name, string type, int fire, int shock, int manuever, int seige, string monarchType)
        {
            this.monarchType = monarchType;
            this.name = name;
            this.type = type;
            this.fire = fire;
            this.shock = shock;
            this.manuever = manuever;
            this.seige = seige;
        }
        public void Print()

        {
            Console.WriteLine(type + ": " + name + " " + " (" + fire + ", " + shock + ", " + manuever + ", " + seige + ") ");
        }
    }
}
