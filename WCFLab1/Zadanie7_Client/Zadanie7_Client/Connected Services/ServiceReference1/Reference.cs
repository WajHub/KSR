﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zadanie7_Client.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IZadanie7")]
    public interface IZadanie7 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IZadanie7/RzucWyjatek7", ReplyAction="http://tempuri.org/IZadanie7/RzucWyjatek7Response")]
        void RzucWyjatek7(string a, int b);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IZadanie7/RzucWyjatek7", ReplyAction="http://tempuri.org/IZadanie7/RzucWyjatek7Response")]
        System.Threading.Tasks.Task RzucWyjatek7Async(string a, int b);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IZadanie7Channel : Zadanie7_Client.ServiceReference1.IZadanie7, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Zadanie7Client : System.ServiceModel.ClientBase<Zadanie7_Client.ServiceReference1.IZadanie7>, Zadanie7_Client.ServiceReference1.IZadanie7 {
        
        public Zadanie7Client() {
        }
        
        public Zadanie7Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Zadanie7Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Zadanie7Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Zadanie7Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void RzucWyjatek7(string a, int b) {
            base.Channel.RzucWyjatek7(a, b);
        }
        
        public System.Threading.Tasks.Task RzucWyjatek7Async(string a, int b) {
            return base.Channel.RzucWyjatek7Async(a, b);
        }
    }
}
