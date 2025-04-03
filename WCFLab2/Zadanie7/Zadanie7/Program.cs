using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Zadanie7.ServiceReference1;

namespace Zadanie7
{
    class Program
    {
        static void Main(string[] args)
        {
            var client5 = new Zadanie5Client();
            var client6 = new Zadanie6Client(new InstanceContext(new Zadanie6Callback()));
            Console.WriteLine(("ZADANIE 5 \n") + client5.ScalNapisy("Scalony_", " _napiS"));
            Console.WriteLine();
            client6.Dodaj(111, 333);

            Console.ReadKey();
        }

        class Zadanie6Callback : IZadanie6Callback
        {
            public void Wynik(int wyn)
            {
              
                Console.WriteLine($"ZADANIE 6 \nwyninik dodawania =>{wyn}");
            }
        }
    }
}