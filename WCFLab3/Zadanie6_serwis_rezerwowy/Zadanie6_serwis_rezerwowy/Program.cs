using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_6_serwis_rezerwowy
{
    [ServiceContract]
    interface IUsluga
    {
        [OperationContract]
        int Dodaj(int a, int b);
    }

    class Usluga : IUsluga
    {
        public int Dodaj(int a, int b)
        {
            Console.WriteLine("Tu SERWIS ZAPASOWY");
            return a + b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Usluga));
            host.AddServiceEndpoint(typeof(IUsluga), new NetNamedPipeBinding(), "net.pipe://localhost/zadanie_6_serwis_rezerwowy");
            host.Open();
            Console.WriteLine("Zadanie 6 - Serwis Rezerwowy");
            Console.ReadLine();
            host.Close();
        }
    }
}