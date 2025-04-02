using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadanie1.ServiceReference1;
namespace Zadanie1
{
    class Program
    {
    static void Main(string[] args)
        {
            var client_1 = new Zadanie1Client();
            IAsyncResult res = client_1.BeginDlugieObliczenia(null, null);
            foreach (var x in Enumerable.Range(0, 21))
            {
                client_1.Szybciej(x, 3 * x * x - 2 * x);
            }
            var wynik = client_1.EndDlugieObliczenia(res);
            Console.WriteLine(wynik);
            Console.ReadKey();

        }
    }
}
