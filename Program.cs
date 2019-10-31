using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using netDxf;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Tables;

namespace Mason
{
    class Program
    {
        public static void Main()
        {
            List<BrickInscription> inscriptions = BrickInscription.ParseBrickInscriptionsFromCSV(@"C:\Users\Hunter\source\repos\Mason\orders.csv");
            BrickInscription.PrintInscriptionsToDXFFile(inscriptions);
        }
    }
}