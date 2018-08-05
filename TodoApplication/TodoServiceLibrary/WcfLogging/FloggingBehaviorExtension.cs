using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace TodoServiceLibrary.WcfLogging
{
    public class FloggingBehaviorExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(FloggingServiceBehaviorAttribute);

        protected override object CreateBehavior()
        {
            return new FloggingServiceBehaviorAttribute(Enabled);
        }

        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
            set { base["enabled"] = value; }
        }
    }
}
