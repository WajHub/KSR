using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zadanie5
{
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        [WebInvoke(UriTemplate = "Dodaj/{a}/{b}")]
        int Dodaj(string a, string b);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ChannelFactory<IService>(new WebHttpBinding(), new EndpointAddress("http://localhost:30703/Service1.svc/"));
            factory.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
            var channel = factory.CreateChannel();
            Console.WriteLine($"Wykonanie funkcji dodawania: 8 + 44 = {channel.Dodaj("8", "44")}");
            factory.Close();
            Console.ReadLine();
        }
    }
}