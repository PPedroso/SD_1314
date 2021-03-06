﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StandAuto.Broker {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ISEL.BROKER.STAND", ConfigurationName="Broker.IBrokerStand")]
    public interface IBrokerStand {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ISEL.BROKER.STAND/IBrokerStand/registerStand", ReplyAction="http://ISEL.BROKER.STAND/IBrokerStand/registerStandResponse")]
        void registerStand(string standEndpoint);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBrokerStandChannel : StandAuto.Broker.IBrokerStand, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BrokerStandClient : System.ServiceModel.ClientBase<StandAuto.Broker.IBrokerStand>, StandAuto.Broker.IBrokerStand {
        
        public BrokerStandClient() {
        }
        
        public BrokerStandClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BrokerStandClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BrokerStandClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BrokerStandClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void registerStand(string standEndpoint) {
            base.Channel.registerStand(standEndpoint);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ISEL.BROKER.CLIENT", ConfigurationName="Broker.IBrokerClient")]
    public interface IBrokerClient {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://ISEL.BROKER.CLIENT/IBrokerClient/submitQueryByBrand")]
        void submitQueryByBrand(string client, string brand);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://ISEL.BROKER.CLIENT/IBrokerClient/submitQueryByMinumumYearRegistration")]
        void submitQueryByMinumumYearRegistration(string client, int yearRegistration);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://ISEL.BROKER.CLIENT/IBrokerClient/submitQueryByMaxPrice")]
        void submitQueryByMaxPrice(string client, int maxPrice);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBrokerClientChannel : StandAuto.Broker.IBrokerClient, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BrokerClientClient : System.ServiceModel.ClientBase<StandAuto.Broker.IBrokerClient>, StandAuto.Broker.IBrokerClient {
        
        public BrokerClientClient() {
        }
        
        public BrokerClientClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BrokerClientClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BrokerClientClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BrokerClientClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void submitQueryByBrand(string client, string brand) {
            base.Channel.submitQueryByBrand(client, brand);
        }
        
        public void submitQueryByMinumumYearRegistration(string client, int yearRegistration) {
            base.Channel.submitQueryByMinumumYearRegistration(client, yearRegistration);
        }
        
        public void submitQueryByMaxPrice(string client, int maxPrice) {
            base.Channel.submitQueryByMaxPrice(client, maxPrice);
        }
    }
}
