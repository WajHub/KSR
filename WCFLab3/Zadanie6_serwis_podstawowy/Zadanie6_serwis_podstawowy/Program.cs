using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie6_serwis_podstawowy
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
            Console.WriteLine("Tu SERWIS PODSTAWOWY");
            return a + b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Usluga));
            host.AddServiceEndpoint(typeof(IUsluga), new NetNamedPipeBinding(), "net.pipe://localhost/zadanie_6_serwis_podstawowy");
            host.Open();
            Console.WriteLine("Zadanie 6 - Serwis Podstawowy");
            Console.ReadLine();
            host.Close();
        }
    }
}