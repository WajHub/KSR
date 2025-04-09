using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Discovery;
using System.ServiceModel;

namespace Zadanie1
{
    [ServiceContract]
    interface IUsluga
    {
        [OperationContract]
        string ScalNapisy(string a, string b);
    }

    class Usluga : IUsluga
    {
        public string ScalNapisy(string a, string b)
        {
            return a + b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Usluga));
            host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            host.AddServiceEndpoint(new UdpDiscoveryEndpoint("soap.udp://localhost:30703"));
            host.AddServiceEndpoint(typeof(IUsluga), new NetNamedPipeBinding(), "net.pipe://localhost/usluga");
            host.Open();
            Console.WriteLine("Usługa działa na soap.udp://localhost:30703");
            Console.WriteLine("Naciśnij Enter aby zakończyć...");
            Console.ReadLine();
            host.Close();
        }
    }
}
