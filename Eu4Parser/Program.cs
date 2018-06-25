using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Parser
{
    class Program
    {
        static void Main(string[] args)
        {

        }
        public static string LoadFile(string path)
        {
            string result;
            result = System.IO.File.ReadAllText(path);
            return result;
        }
    }
}
