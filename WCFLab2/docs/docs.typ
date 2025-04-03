#set page(
  paper: "a4",
  margin: (x: 2cm, y: 2.75cm),
  numbering: "1",
  header: [
    #align(top + center)[
      #v(0.25cm)
      #grid(
        columns: (0.75fr, auto, 1fr, auto, 1fr),
      )
      #line(length: 100%, stroke: (thickness: 0.5pt))
    ]
  ],
)
#set heading(numbering: "1.")

#v(20pt)
#align(center, text(size: 25pt, weight: "bold")[
  Komponentowe systemy rozproszone 2024/25
])
#v(10pt)
#align(center, text(style: "italic")[
  Hubert Wajda 193511
])
#v(3pt)
#align(center, text(style: "italic")[
  Wersja 1.0
])

#v(1cm)

#outline(indent: 0pt, depth: 2, title: "Spis treści")

#pagebreak()

= Zadanie 1 

== Wynik

#figure(
  image("img/zad1.jpg", width: 70%),
  caption: [Zadanie 1 - Rezultat],
)

== Kod

#figure(
  image("img/zad1_kod.jpg", width: 70%),
  caption: [Zadanie 1 - Kod (Zadanie wykonane wykorzustując opcję „Generate asynchronous operations”)],
)
#pagebreak()
```csharp
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
```

= Zadanie 2

== Wynik

#figure(
  image("img/zad2.jpg", width: 70%),
  caption: [Zadanie 1 - Rezultat],
)

== Kod

#figure(
  image("img/zad2_kod.jpg", width: 70%),
  caption: [Zadanie 1 - Kod (Zadanie wykonane wykorzustując opcję „Generate task-based operations”)],
)
#pagebreak()
```csharp
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
```

= Zadanie 3 i 4 

== Wynik
#figure(
  image("img/zad3_4.png", width: 70%),
  caption: [Zadanie 3 i 4 - Rezultat],
)

== Kod

#figure(
  image("img/zad3_4_kod_1.png", width: 70%),
  caption: [Zadanie 3 i 4 - Kod (część 1)],
)
#figure(
  image("img/zad3_4_kod_2.png", width: 70%),
  caption: [Zadanie 3 i 4 - Kod (część 2)],
)
#pagebreak()

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadanie3_4;
using KSR_WCF2;
using System.ServiceModel;

namespace Zadanie3_4
{
    class Program
    {
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
        class Serwer : IZadanie3, IZadanie4
        {
            private int counter = 0;

            public void TestujZwrotny()
            {
                var callbackChannel = OperationContext.Current.GetCallbackChannel<IZadanie3Zwrotny>();
                for(int x = 0; x < 31; x++)
                {
                    callbackChannel.WolanieZwrotne(x, x * x * x - x * x);
                }
            }

            int IZadanie4.Dodaj(int v)
            {
                counter += v;
                return counter;
            }

            void IZadanie4.Ustaw(int v)
            {
                counter = v;
            }
        }
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Serwer), new Uri[] { new Uri("http://localhost:1100") });
            host.AddServiceEndpoint(typeof(IZadanie3), new
                NetNamedPipeBinding(),
                "net.pipe://localhost/ksr-wcf2-zad3");
            host.AddServiceEndpoint(typeof(IZadanie4), new
                NetNamedPipeBinding(),
                "net.pipe://localhost/ksr-wcf2-zad4");


            host.Open();
            Console.Read();
            host.Close();
        }
    }
}
  
```

= Zadanie 5 i 6

== Wynik

#figure(
  image("img/zad5_6.png", width: 70%),
  caption: [Zadanie 5 i 6 - Rezultat],
)
== Kod

#figure(
  image("img/zad5_6_kod.png", width: 70%),
  caption: [Zadanie 5 i 6 - Kod],
)

#pagebreak()
```csharp
namespace WcfService1
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class Service1 : IZadanie5, IZadanie6
	{

        string IZadanie5.ScalNapisy(string a, string b)
        {
            return a + b;
        }
        public void Dodaj(int a, int b)
        {
            var channel = OperationContext.Current.GetCallbackChannel<IZadanie6Zwrotny>();
            channel.Wynik(a + b);
        }
    }
}
```

= Zadanie 7 

== Wynik
#figure(
  image("img/zad7.png", width: 70%),
  caption: [Zadanie 7 - Rezultat],
)
== Kod
#figure(
  image("img/zad7_kod.png", width: 70%),
  caption: [Zadanie 7 - Kod],
)
#pagebreak()
```csharp

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
```