using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace TodoServiceLibrary.WcfLogging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FloggingServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        bool _enabled;
        public FloggingServiceBehaviorAttribute(bool enabled)
        {
            _enabled = enabled;
        }
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase)
        {
            if (_enabled)
            {
                foreach (var endpoint in serviceDescription.Endpoints)
                    foreach (var op in endpoint.Contract.Operations)
                        op.OperationBehaviors
                            .Add(new FloggingOperationBehaviorAttribute(_enabled));                        
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {            
        }
        
        public void Validate(ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase)
        {            
        }
    }
}
