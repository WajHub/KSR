using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using KSR_WCF1;

namespace Zadanie1
{
    class Program
    {
        static void Main(string[] args)
        {
            var fact = new ChannelFactory<IZadanie1>(
                new NetNamedPipeBinding(),
                // tworzymy proxy
                new EndpointAddress("net.pipe://localhost/ksr-wcf1-test")
                );
            var client = fact.CreateChannel();
            // korzystamy z usługi
            Console.WriteLine("wynik={0}", client.Test("test"));

            // Zadanie 5 ----
            try
            {
                client.RzucWyjatek(true);
            }
            catch(FaultException<Wyjatek> ex)
            {
                Console.WriteLine("Wyjątek: {0}", ex.Detail.opis);
                Console.WriteLine(client.OtoMagia(ex.Detail.magia));
            }
            // ----


            ((IDisposable)client).Dispose();
            // fabrykę możemy zwolnić dopiero wtedy, gdy skończymy korzystać z utworzonych
            // przez nią kanałów
            fact.Close();

            Console.ReadKey();
        }
    }
}
