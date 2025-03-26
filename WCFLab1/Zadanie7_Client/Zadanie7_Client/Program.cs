using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Zadanie7_Client.ServiceReference2;

namespace Zadanie7_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference2.Zadanie7Client();

            try
            {
                client.RzucWyjatek7("TEEST", 69);
            }
            catch (FaultException<ServiceReference2.Wyjatek7> ex)
            {
                Console.WriteLine(ex.Detail.opis);
                Console.WriteLine(ex.Detail.a);
                Console.WriteLine(ex.Detail.b);
            }
            Console.ReadKey();
        }
    }
}
