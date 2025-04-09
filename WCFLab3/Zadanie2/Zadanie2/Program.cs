﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie2
{
    [ServiceContract]
    interface IUsluga
    {
        [OperationContract]
        string ScalNapisy(string a, string b);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint("soap.udp://localhost:30703"));
            var endpoints = discoveryClient.Find(new FindCriteria(typeof(IUsluga))).Endpoints;
            discoveryClient.Close();

            if (endpoints.Count < 1)
            {
                return;
            }
            var proxy = ChannelFactory<IUsluga>.CreateChannel(new NetNamedPipeBinding(), endpoints[0].Address);
            Console.WriteLine($"Scalenie napisow: {proxy.ScalNapisy("Kocham-", "-Windows")}");
            Console.WriteLine("Naciśnij Enter aby zakończyć...");
            (proxy as IDisposable).Dispose();
            Console.ReadLine();
        }
    }
}