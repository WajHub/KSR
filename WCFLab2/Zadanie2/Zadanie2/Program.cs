using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadanie2.ServiceReference1;
using System.ServiceModel;

namespace Zadanie2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client_2 = new Zadanie2Client(new InstanceContext(new Zadanie2Callback()));
            await client_2.PodajZadaniaAsync();
            Console.ReadKey();
        }

        class Zadanie2Callback : IZadanie2Callback
        {

            public void Zadanie([MessageParameter(Name = "zadanie")] string zadanie1, int pkt, bool zaliczone)
            {
                Console.WriteLine($"zadanie1={zadanie1}, pkt={pkt}, zaliczone={zaliczone}");
            }
        }
    }
}
