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
  