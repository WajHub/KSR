﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie6_Router
{
    class Program
    {
        static void Main(string[] args)
        {
            const string service1Address = "net.pipe://localhost/zadanie_6_serwis_podstawowy";
            const string service2Address = "net.pipe://localhost/zadanie_6_serwis_rezerwowy";
            const string routerAddress = "net.pipe://localhost/6";

            var host = new ServiceHost(typeof(RoutingService));
            host.AddServiceEndpoint(typeof(IRequestReplyRouter), new NetNamedPipeBinding(), routerAddress);
            var contract = ContractDescription.GetContract(typeof(IRequestReplyRouter));
            var client1 = new ServiceEndpoint(contract, new NetNamedPipeBinding(), new EndpointAddress(service1Address));
            var client2 = new ServiceEndpoint(contract, new NetNamedPipeBinding(), new EndpointAddress(service2Address));
            var routeConfig = new RoutingConfiguration();
            routeConfig.FilterTable.Add(new MatchAllMessageFilter(), new[]
            {
                client1,
                client2
            });
            host.Description.Behaviors.Add(new RoutingBehavior(routeConfig));
            host.Open();
            Console.WriteLine("Zadanie 6 - Router.");
            Console.ReadLine();
            host.Close();
        }
    }
}