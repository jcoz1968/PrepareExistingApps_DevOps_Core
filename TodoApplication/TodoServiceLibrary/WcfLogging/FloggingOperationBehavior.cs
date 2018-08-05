using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace TodoServiceLibrary.WcfLogging
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FloggingOperationBehaviorAttribute : Attribute, IOperationBehavior
    {
        bool _enabled;
        public FloggingOperationBehaviorAttribute(bool enabled)
        {
            _enabled = enabled;
        }
        public void ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation)
        {
            var p = dispatchOperation.Parent.Type;
            if (_enabled && p != null)
                dispatchOperation.ParameterInspectors.Add(
                    new FloggingParameterInspector(dispatchOperation.Parent.Type.FullName));
        }

        public void AddBindingParameters(OperationDescription operationDescription, 
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, 
            ClientOperation clientOperation)
        {
        }        

        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}
