using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using KSR_WCF2;

namespace KSR_WCF2
{
    [ServiceContract]
    public interface IZadanie2
    {
        [OperationContract]
        string Test(string arg);
    }

    [ServiceContract]
    public interface IZadanie7
    {
        [OperationContract]
        [FaultContract(typeof(Wyjatek7))]
        void RzucWyjatek7(string a, int b);
    }

    [DataContract]
    public class Wyjatek7
    {
        [DataMember]
        public string opis { get; set; }
        [DataMember]
        public string a { get; set; }
        [DataMember]
        public int b { get; set; }
    }
}

namespace Zadanie2
{
    public class Zadanie2 : IZadanie2
    {
        public string Test(string arg)
        {
            Console.WriteLine("Test: " + arg);
            return arg;
        }
    
    }
    public class Zadanie7: IZadanie7
    {
        public void RzucWyjatek7(string a, int b)
        {
            throw new FaultException<Wyjatek7>(new Wyjatek7() { opis = "Wyjatek7 - Zadanie 7", a = a, b = b });
        }
    }
        

    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Zadanie7), new Uri[] {new Uri("http://localhost:1100"), new Uri("net.tcp://127.0.0.1:55765") });


            // ----- Zadanie 3 
            var b = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (b == null) b = new ServiceMetadataBehavior();
            host.Description.Behaviors.Add(b);
            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexNamedPipeBinding(),"net.pipe://localhost/metadane");
            // ----------------

            // ----- Zadanie 4
            //host.AddServiceEndpoint(typeof(IZadanie2), new NetTcpBinding(), "net.tcp://127.0.0.1:55765");
            // ---------------

            // -----Zadanie7

            host.AddServiceEndpoint(typeof(IZadanie7), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf1-zad7");
            // -------------



            //host.AddServiceEndpoint(typeof(IZadanie2), new NetNamedPipeBinding(), "net.pipe://localhost/ksr-wcf1-zad2");

            host.Open();
            // czekamy na zakończenie, np.:
            Console.ReadKey();
            host.Close();

        }
    }
}
